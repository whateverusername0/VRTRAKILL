﻿using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Arms.Patches
{
    [HarmonyPatch(typeof(Punch))] internal sealed class PunchP
    {
        public static Vector3 _Thing; public static Vector3 Thing
        {
            get
            {
                if (Vars.Config.MBP.ToggleVelocity) return _Thing;
                else return Vars.NonDominantHand.transform.forward;
            }
        }
        // Unholster
        /*[HarmonyPostfix] [HarmonyPatch(nameof(Punch.Start))]*/ static void Start(Punch __instance)
        {
            GameObject H = Object.Instantiate(__instance.gameObject, __instance.transform.parent);
        } 
        [HarmonyPrefix] [HarmonyPatch(nameof(Punch.Update))] static bool Update(Punch __instance)
        {
            if (MonoSingleton<OptionsManager>.Instance.paused) return false;

            if (Vars.NDHC.Speed >= Vars.Config.MBP.PunchingSpeed
                && MonoSingleton<InputManager>.Instance.InputSource.Punch.IsPressed
                && __instance.ready && !__instance.shopping
                && /* __instance.fc.fistCooldown <= 0f && */ __instance.fc.activated
                && !GameStateManager.Instance.PlayerInputLocked)
            {
                //__instance.fc.weightCooldown += __instance.cooldownCost * 0.25f + __instance.fc.weightCooldown * __instance.cooldownCost * 0.1f;
                //__instance.fc.fistCooldown += .1f; //__instance.fc.weightCooldown;
                __instance.PunchStart();
                __instance.holdingInput = true;
            }

            if (__instance.holdingInput && MonoSingleton<InputManager>.Instance.InputSource.Punch.WasCanceledThisFrame)
                __instance.holdingInput = false;

            float layerWeight = __instance.anim.GetLayerWeight(1);
            if (__instance.shopping && layerWeight < 1f)
                __instance.anim.SetLayerWeight(1, Mathf.MoveTowards(layerWeight, 1f, Time.deltaTime / 10f + 5f * Time.deltaTime * (1f - layerWeight)));
            else if (!__instance.shopping && layerWeight > 0f)
                __instance.anim.SetLayerWeight(1, Mathf.MoveTowards(layerWeight, 0f, Time.deltaTime / 10f + 5f * Time.deltaTime * layerWeight));

            if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo()
                && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && __instance.shopping)
                __instance.anim.SetTrigger("ShopTap");

            if (__instance.returnToOrigRot)
            {
                __instance.transform.parent.localRotation =
                    Quaternion.RotateTowards(__instance.transform.parent.localRotation,
                                             Quaternion.identity,
                                             (Quaternion.Angle(__instance.transform.parent.localRotation,
                                                               Quaternion.identity) * 5f + 5f)
                                                               * Time.deltaTime * 5f);
                if (__instance.transform.parent.localRotation == Quaternion.identity)
                    __instance.returnToOrigRot = false;
            }

            if (__instance.fc.shopping && !__instance.shopping) __instance.ShopMode();
            else if (!__instance.fc.shopping && __instance.shopping) __instance.StopShop();

            if (__instance.holding && (bool)__instance.heldItem)
            {
                if (!__instance.heldItem.noHoldingAnimation && __instance.fc.forceNoHold <= 0)
                {
                    __instance.anim.SetBool("SemiHolding", value: false);
                    __instance.anim.SetBool("Holding", value: true);
                }
                else __instance.anim.SetBool("SemiHolding", value: true);
            }
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(Punch.ActiveStart))] static bool ActiveStart(Punch __instance)
        {
            if (__instance.ignoreDoublePunch) { __instance.ignoreDoublePunch = false; return false; }
            __instance.returnToOrigRot = false;
            __instance.hitSomething = false;
            if (__instance.type == FistType.Standard)
            {
                Collider[] array = Physics.OverlapSphere(Vars.NonDominantHand.transform.position, 0.01f,
                                                         __instance.deflectionLayerMask, QueryTriggerInteraction.Collide);
                List<Transform> list = new List<Transform>();
                if (array.Length != 0)
                {
                    foreach (Collider collider in array)
                    {
                        list.Add(collider.transform);
                        if (__instance.CheckForProjectile((collider.attachedRigidbody != null) 
                                                           ? collider.attachedRigidbody.transform
                                                           : collider.transform)) break;
                    }
                }
                if ((!Physics.Raycast(Vars.NonDominantHand.transform.position,
                                      Thing,
                                      out __instance.hit, 4f,
                                      __instance.deflectionLayerMask)
                    && !Physics.BoxCast(Vars.NonDominantHand.transform.position,
                                        Vector3.one * 0.3f,
                                        Thing,
                                        out __instance.hit,
                                        Vars.NonDominantHand.transform.rotation, 4f,
                                        __instance.deflectionLayerMask))
                    || list.Contains(__instance.hit.transform)
                    || !__instance.CheckForProjectile(__instance.hit.transform))
                {
                    if (__instance.ppz == null) __instance.ppz = __instance.transform.parent.GetComponentInChildren<ProjectileParryZone>();
                    if (__instance.ppz != null)
                    {
                        Projectile projectile = __instance.ppz.CheckParryZone();
                        if (projectile != null
                            && !list.Contains(projectile.transform)
                            && !projectile.undeflectable
                            && (!__instance.alreadyBoostedProjectile || !projectile.playerBullet))
                        {
                            __instance.ParryProjectile(projectile);
                            __instance.hitSomething = true;
                        }
                    }
                }
                Collider[] array3 = Physics.OverlapSphere(Vars.NonDominantHand.transform.position + Thing * 3f,
                                                          3f, __instance.deflectionLayerMask, QueryTriggerInteraction.Collide);
                bool flag = false;
                foreach (Collider collider2 in array3)
                {
                    if ((collider2.attachedRigidbody
                        ? collider2.attachedRigidbody.TryGetComponent(out Nail nail)
                        : collider2.TryGetComponent(out nail)) && nail.sawblade && nail.punchable)
                    {
                        flag = true;
                        if (nail.stopped)
                        {
                            nail.stopped = false;
                            nail.rb.velocity = (Punch.GetParryLookTarget() - nail.transform.position).normalized * nail.originalVelocity.magnitude;
                        }
                        else nail.rb.velocity = (Punch.GetParryLookTarget() - nail.transform.position).normalized * nail.rb.velocity.magnitude;
                        nail.punched = true;
                        if (nail.magnets.Count > 0) nail.punchDistance = Vector3.Distance(nail.transform.position, nail.GetTargetMagnet().transform.position);
                    }
                }
                if (!flag)
                {
                    foreach (Collider collider3 in Physics.OverlapSphere(Vars.NonDominantHand.transform.position + Thing,
                                                                         1f, 1, QueryTriggerInteraction.Collide))
                    {
                        float num = Vector3.Distance(Vars.NonDominantHand.transform.position + Thing, collider3.transform.position);
                        if (num >= 6f && num <= 12f
                            && Mathf.Abs((Vars.NonDominantHand.transform.position + Thing).y - collider3.transform.position.y)
                                          <= 3f && collider3.TryGetComponent(out Magnet magnet) && magnet.sawblades.Count > 0)
                        {
                            float num2 = float.PositiveInfinity; int num3 = -1;
                            for (int j = magnet.sawblades.Count - 1; j >= 0; j--)
                            {
                                if (magnet.sawblades[j] == null)
                                {
                                    magnet.sawblades.RemoveAt(j);
                                    if (flag) num3--;
                                }
                                else
                                {
                                    float num4 = Vector3.Distance(magnet.sawblades[j].transform.position,
                                                                  Vars.NonDominantHand.transform.position);
                                    if (magnet.sawblades[j] != null && (num3 < 0 || num2 < num4))
                                    { num3 = j; num2 = num4; flag = true; }
                                }
                            }
                            if (flag && magnet.sawblades[num3].TryGetComponent(out Nail nail2))
                            {
                                nail2.transform.position = Vars.NonDominantHand.transform.position + __instance.cc.transform.forward;
                                if (nail2.stopped)
                                {
                                    nail2.stopped = false;
                                    nail2.rb.velocity = (Punch.GetParryLookTarget() - nail2.transform.position).normalized * nail2.originalVelocity.magnitude;
                                }
                                else nail2.rb.velocity = (Punch.GetParryLookTarget() - nail2.transform.position).normalized * nail2.rb.velocity.magnitude;
                                nail2.punched = true;
                                if (nail2.magnets.Count <= 0) break;
                                Magnet targetMagnet = nail2.GetTargetMagnet();
                                if (Vector3.Distance(nail2.transform.position + nail2.rb.velocity.normalized, targetMagnet.transform.position)
                                                     > Vector3.Distance(nail2.transform.position, targetMagnet.transform.position))
                                { nail2.MagnetRelease(targetMagnet); break; }
                                nail2.punchDistance = Vector3.Distance(nail2.transform.position, targetMagnet.transform.position); break;
                            }
                        }
                    }
                }
                if (flag)
                {
                    Object.Instantiate<AudioSource>(__instance.specialHit, __instance.transform.position, Quaternion.identity);
                    MonoSingleton<TimeController>.Instance.HitStop(0.1f);
                    __instance.anim.Play("Hook", -1, 0.065f);
                    __instance.hitSomething = true;
                }
            }
            else if (Physics.Raycast(Vars.NonDominantHand.transform.position, Thing,
                                     out __instance.hit, 4f, __instance.deflectionLayerMask)
                  || Physics.BoxCast(Vars.NonDominantHand.transform.position, Vector3.one * 0.3f,
                                     Thing, out __instance.hit,
                                     Vars.NonDominantHand.transform.rotation, 4f, __instance.deflectionLayerMask))
            {
                MassSpear component = __instance.hit.transform.gameObject.GetComponent<MassSpear>();
                if (component != null && component.hitPlayer)
                {
                    Object.Instantiate(__instance.specialHit, __instance.transform.position, Quaternion.identity);
                    MonoSingleton<TimeController>.Instance.HitStop(0.1f);
                    __instance.cc.CameraShake(0.5f * __instance.screenShakeMultiplier);
                    component.GetHurt(25f);
                    __instance.hitSomething = true;
                }
            }
            bool flag2 = __instance.holding;
            Collider[] array4 = Physics.OverlapSphere(Vars.NonDominantHand.transform.position, 0.1f,
                                                      __instance.ignoreEnemyTrigger, QueryTriggerInteraction.Collide);
            if (array4 != null && array4.Length != 0)
            {
                foreach (Collider collider4 in array4) __instance.PunchSuccess(Vars.NonDominantHand.transform.position, collider4.transform);
                __instance.hitSomething = true;
            }
            else if (Physics.Raycast(Vars.NonDominantHand.transform.position, Thing,
                                     out __instance.hit, 4f, __instance.ignoreEnemyTrigger, QueryTriggerInteraction.Collide)
                 || Physics.SphereCast(Vars.NonDominantHand.transform.position, 1f, Thing,
                                       out __instance.hit, 4f, __instance.ignoreEnemyTrigger, QueryTriggerInteraction.Collide))
            {
                bool flag3 = false;
                if (Physics.Raycast(Vars.NonDominantHand.transform.position, __instance.hit.point - Vars.NonDominantHand.transform.position,
                                    out RaycastHit raycastHit, 5f, __instance.environmentMask)
                    && Vector3.Distance(Vars.NonDominantHand.transform.position, __instance.hit.point)
                    > Vector3.Distance(Vars.NonDominantHand.transform.position, raycastHit.point)) flag3 = true;
                if (!flag3) { __instance.PunchSuccess(__instance.hit.point, __instance.hit.transform); __instance.hitSomething = true; }
            }
            if (Physics.CheckSphere(Vars.NonDominantHand.transform.position, 0.01f, __instance.environmentMask, QueryTriggerInteraction.Collide))
            {
                foreach (Collider collider5 in Physics.OverlapSphere(Vars.NonDominantHand.transform.position, 0.01f, __instance.environmentMask))
                {
                    __instance.hitSomething = true;
                    __instance.AltHit(collider5.transform);
                }
            }
            else if (Physics.Raycast(Vars.NonDominantHand.transform.position, Thing, out __instance.hit, 4f, __instance.environmentMask))
            {
                __instance.AltHit(__instance.hit.transform);
                if (!__instance.hitSomething && (__instance.hit.transform.gameObject.layer == 8 || __instance.hit.transform.gameObject.layer == 24))
                {
                    __instance.transform.parent.localRotation = Quaternion.identity;
                    __instance.cc.CameraShake(0.2f * __instance.screenShakeMultiplier);
                    Object.Instantiate<AudioSource>(__instance.normalHit, __instance.transform.position, Quaternion.identity);
                    __instance.currentDustParticle = Object.Instantiate<GameObject>(__instance.dustParticle, __instance.hit.point, __instance.transform.rotation);
                    __instance.currentDustParticle.transform.forward = __instance.hit.normal;
                    Breakable component2 = __instance.hit.transform.gameObject.GetComponent<Breakable>();
                    if (component2 != null && !component2.precisionOnly && (component2.weak || __instance.type == FistType.Heavy)) component2.Break();
                    if (__instance.hit.collider.gameObject.TryGetComponent<Bleeder>(out Bleeder bleeder))
                    {
                        if (__instance.type == FistType.Standard) bleeder.GetHit(__instance.hit.point, GoreType.Body);
                        else bleeder.GetHit(__instance.hit.point, GoreType.Head);
                    }
                    if (__instance.type == FistType.Heavy)
                    {
                        Glass component3 = __instance.hit.collider.gameObject.GetComponent<Glass>();
                        if (component3 != null && !component3.broken) component3.Shatter();
                    }
                }
            }
            if (flag2 && __instance.holding && __instance.heldItem != null)
            {
                __instance.ForceThrow();
                __instance.cc.CameraShake(0.2f * __instance.screenShakeMultiplier);
                return false;
            }
            __instance.cc.CameraShake(0.2f * __instance.screenShakeMultiplier);

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(Punch.BlastCheck))] static bool BlastCheck(Punch __instance)
        {
            if (MonoSingleton<InputManager>.Instance.InputSource.Punch.IsPressed)
            {
                __instance.holdingInput = false;
                __instance.anim.SetTrigger("PunchBlast");
                Vector3 position = Vars.NonDominantHand.transform.position + Vars.NonDominantHand.transform.forward * 2f;
                if (Physics.Raycast(Vars.NonDominantHand.transform.position, Vars.NonDominantHand.transform.forward, out var hitInfo,
                                    2f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies))) position = hitInfo.point - Thing * 0.1f;

                Object.Instantiate(__instance.blastWave, position, Vars.NonDominantHand.transform.rotation);
            }
            return false;
        }
    }
}

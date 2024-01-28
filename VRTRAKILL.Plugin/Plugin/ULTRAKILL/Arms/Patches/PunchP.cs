using HarmonyLib;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
        [HarmonyPrefix] [HarmonyPatch(nameof(Punch.Update))] static bool Update(Punch __instance)
        {
            if (MonoSingleton<OptionsManager>.Instance.paused)
                return false;

            __instance.fc.fistCooldown = 0f;

            if (Vars.NDHC.Speed >= Vars.Config.MBP.PunchingSpeed
            && MonoSingleton<InputManager>.Instance.InputSource.Punch.IsPressed
            && __instance.ready && !__instance.shopping
            && __instance.fc.activated
            && !GameStateManager.Instance.PlayerInputLocked)
            {
                __instance.heldAction = MonoSingleton<InputManager>.Instance.InputSource.Punch.Action;
                __instance.PunchStart();
            }

            if (__instance.holdingInput && __instance.heldAction.WasReleasedThisFrame())
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
                __instance.transform.parent.localRotation = Quaternion.RotateTowards(__instance.transform.parent.localRotation,
                                                                                     Quaternion.identity,
                                                                                     (Quaternion.Angle(__instance.transform.parent.localRotation,
                                                                                                       Quaternion.identity) * 5f + 5f)
                                                                                     * Time.deltaTime * 5f);
                if (__instance.transform.parent.localRotation == Quaternion.identity)
                    __instance.returnToOrigRot = false;
            }

            if (__instance.fc.shopping && !__instance.shopping)
                __instance.ShopMode();
            else if (!__instance.fc.shopping && __instance.shopping)
                __instance.StopShop();

            if (__instance.holding)
            {
                if (__instance.heldItem.Equals(null))
                    MonoSingleton<FistControl>.Instance.currentPunch.ResetHeldState();
                else if (!__instance.heldItem.noHoldingAnimation && __instance.fc.forceNoHold <= 0)
                {
                    __instance.anim.SetBool("SemiHolding", value: false);
                    __instance.anim.SetBool("Holding", value: true);
                }
                else __instance.anim.SetBool("SemiHolding", value: true);
            }
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(Punch.ActiveFrame))] static bool ActiveFrame(Punch __instance, bool firstFrame = false)
        {
            if (__instance.type == FistType.Standard && !__instance.parriedSomething)
            {
                Collider[] array = Physics.OverlapSphere(Vars.NonDominantHand.transform.position, 0.01f, __instance.deflectionLayerMask, QueryTriggerInteraction.Collide);
                List<Transform> list = new List<Transform>();
                Collider[] array2 = array;
                foreach (Collider collider in array2)
                {
                    list.Add(collider.transform);
                    if (__instance.TryParryProjectile((collider.attachedRigidbody != null)
                        ? collider.attachedRigidbody.transform : collider.transform, firstFrame))
                        break;
                }

                bool flag = Physics.Raycast(Vars.NonDominantHand.transform.position,
                                            Thing,
                                            out __instance.hit, 4f,
                                            __instance.deflectionLayerMask);
                if (!flag)
                    flag = Physics.BoxCast(Vars.NonDominantHand.transform.position,
                                           Vector3.one * 0.3f,
                                           Thing,
                                           out __instance.hit,
                                           Vars.NonDominantHand.transform.rotation, 4f,
                                           __instance.deflectionLayerMask);

                if (!flag || list.Contains(__instance.hit.transform) || !__instance.TryParryProjectile(__instance.hit.transform, firstFrame))
                {
                    if (__instance.ppz == null)
                        __instance.ppz = __instance.transform.parent.GetComponentInChildren<ProjectileParryZone>();

                    if (__instance.ppz != null)
                    {
                        Projectile projectile = __instance.ppz.CheckParryZone();
                        if (projectile != null)
                        {
                            bool flag2 = !__instance.alreadyBoostedProjectile && firstFrame;
                            if (!list.Contains(projectile.transform) && !projectile.undeflectable && (flag2 || !projectile.playerBullet))
                            {
                                __instance.ParryProjectile(projectile);
                                __instance.parriedSomething = true;
                                __instance.hitSomething = true;
                            }
                        }
                    }
                }
            }
            else if (!__instance.hitSomething
                 && (Physics.Raycast(Vars.NonDominantHand.transform.position,
                                     Thing,
                                     out __instance.hit, 4f, __instance.deflectionLayerMask)
                 || Physics.BoxCast(Vars.NonDominantHand.transform.position,
                                    Vector3.one * 0.3f,
                                    Thing,
                                    out __instance.hit,
                                    Vars.NonDominantHand.transform.rotation, 4f,
                                    __instance.deflectionLayerMask)))
            {
                MassSpear component = __instance.hit.transform.gameObject.GetComponent<MassSpear>();
                if (component != null && component.hitPlayer)
                {
                    Object.Instantiate(__instance.specialHit, __instance.transform.position, Quaternion.identity);
                    MonoSingleton<TimeController>.Instance.HitStop(0.1f);
                    component.GetHurt(25f);
                    __instance.hitSomething = true;
                }
            }

            bool flag3 = Physics.Raycast(Vars.NonDominantHand.transform.position,
                                         Thing,
                                         out __instance.hit, 4f,
                                         __instance.ignoreEnemyTrigger, QueryTriggerInteraction.Collide);
            if (!flag3)
                flag3 = Physics.SphereCast(Vars.NonDominantHand.transform.position,
                                           1f, Thing,
                                           out __instance.hit, 4f, __instance.ignoreEnemyTrigger,
                                           QueryTriggerInteraction.Collide);

            if (flag3)
            {
                if (!__instance.alreadyHitCoin && __instance.type == FistType.Standard && __instance.hit.collider.CompareTag("Coin"))
                {
                    Coin component2 = __instance.hit.collider.GetComponent<Coin>();
                    if ((bool)component2 && component2.doubled)
                    {
                        __instance.anim.Play("Hook", 0, 0.065f);
                        component2.DelayedPunchflection();
                        __instance.alreadyHitCoin = true;
                    }
                }

                if (__instance.hitSomething)
                    return false;

                bool flag4 = false;
                if (Physics.Raycast(Vars.NonDominantHand.transform.position,
                                    __instance.hit.point - Vars.NonDominantHand.transform.position,
                                    out var hitInfo, 5f,
                                    __instance.environmentMask)
                && Vector3.Distance(Vars.NonDominantHand.transform.position, __instance.hit.point)
                   > Vector3.Distance(Vars.NonDominantHand.transform.position, hitInfo.point)) flag4 = true;

                if (!flag4)
                {
                    __instance.PunchSuccess(__instance.hit.point, __instance.hit.transform);
                    __instance.hitSomething = true;
                }
            }

            if (__instance.hitSomething)
                return false;

            Collider[] array3 = Physics.OverlapSphere(Vars.NonDominantHand.transform.position, 0.1f,
                                                      __instance.ignoreEnemyTrigger, QueryTriggerInteraction.Collide);
            if (array3 != null && array3.Length != 0)
            {
                Collider[] array2 = array3;
                foreach (Collider collider2 in array2)
                    __instance.PunchSuccess(Vars.NonDominantHand.transform.position, collider2.transform);

                __instance.hitSomething = true;
            }

            if (__instance.type == FistType.Standard && !__instance.hitSomething && !__instance.parriedSomething)
            {
                Collider[] array4 = Physics.OverlapSphere(Vars.NonDominantHand.transform.position + Thing * 3f,
                                                          3f, __instance.deflectionLayerMask, QueryTriggerInteraction.Collide);
                bool flag5 = false;
                Collider[] array2 = array4;
                foreach (Collider collider3 in array2)
                {
                    Nail nail = ((!collider3.attachedRigidbody) ? collider3.GetComponent<Nail>() : collider3.attachedRigidbody.GetComponent<Nail>());
                    if (!(nail == null) && nail.sawblade && nail.punchable)
                    {
                        flag5 = true;
                        if (nail.stopped)
                        {
                            nail.stopped = false;
                            nail.rb.velocity = (Punch.GetParryLookTarget() - nail.transform.position).normalized * nail.originalVelocity.magnitude;
                        }
                        else nail.rb.velocity = (Punch.GetParryLookTarget() - nail.transform.position).normalized * nail.rb.velocity.magnitude;

                        nail.punched = true;
                        if (nail.magnets.Count > 0)
                            nail.punchDistance = Vector3.Distance(nail.transform.position, nail.GetTargetMagnet().transform.position);
                    }
                }

                if (!flag5)
                {
                    array2 = Physics.OverlapSphere(Vars.NonDominantHand.transform.position + Thing,
                                                   1f, 1, QueryTriggerInteraction.Collide);
                    foreach (Collider collider4 in array2)
                    {
                        float num = Vector3.Distance(Vars.NonDominantHand.transform.position + Thing,
                                                     collider4.transform.position);
                        if (num < 6f || num > 12f
                        || Mathf.Abs((Vars.NonDominantHand.transform.position + Thing).y
                           - collider4.transform.position.y) > 3f
                        || !collider4.TryGetComponent<Magnet>(out var component3) || component3.sawblades.Count <= 0)
                            continue;

                        float num2 = float.PositiveInfinity;
                        float num3 = 0f;
                        int num4 = -1;
                        for (int num5 = component3.sawblades.Count - 1; num5 >= 0; num5--)
                        {
                            if (component3.sawblades[num5] == null)
                            {
                                component3.sawblades.RemoveAt(num5);
                                if (flag5) num4--;
                            }
                            else
                            {
                                num3 = Vector3.Distance(component3.sawblades[num5].transform.position, Vars.NonDominantHand.transform.position);
                                if (component3.sawblades[num5] != null && (num4 < 0 || num2 < num3))
                                {
                                    num4 = num5;
                                    num2 = num3;
                                    flag5 = true;
                                }
                            }
                        }

                        if (!flag5 || !component3.sawblades[num4].TryGetComponent<Nail>(out var component4))
                            continue;

                        component4.transform.position = Vars.NonDominantHand.transform.position + Thing;
                        if (component4.stopped)
                        {
                            component4.stopped = false;
                            component4.rb.velocity = (Punch.GetParryLookTarget() - component4.transform.position).normalized
                                                      * component4.originalVelocity.magnitude;
                        }
                        else
                            component4.rb.velocity = (Punch.GetParryLookTarget() - component4.transform.position).normalized
                                                      * component4.rb.velocity.magnitude;

                        component4.punched = true;
                        if (component4.magnets.Count > 0)
                        {
                            Magnet targetMagnet = component4.GetTargetMagnet();
                            if (Vector3.Distance(component4.transform.position + component4.rb.velocity.normalized,
                              targetMagnet.transform.position)
                              > Vector3.Distance(component4.transform.position, targetMagnet.transform.position))
                                component4.MagnetRelease(targetMagnet);
                            else
                                component4.punchDistance = Vector3.Distance(component4.transform.position, targetMagnet.transform.position);
                        }

                        break;
                    }
                }

                if (flag5)
                {
                    Object.Instantiate(__instance.specialHit, __instance.transform.position, Quaternion.identity);
                    MonoSingleton<TimeController>.Instance.HitStop(0.1f);
                    __instance.anim.Play("Hook", -1, 0.065f);
                    __instance.parriedSomething = true;
                    __instance.hitSomething = true;
                }
            }

            if (Physics.CheckSphere(Vars.NonDominantHand.transform.position, 0.01f, __instance.environmentMask, QueryTriggerInteraction.Collide))
            {
                Collider[] array2 = Physics.OverlapSphere(Vars.NonDominantHand.transform.position, 0.01f, __instance.environmentMask);
                foreach (Collider collider5 in array2)
                    __instance.AltHit(collider5.transform);
            }
            else
            {
                if (!Physics.Raycast(Vars.NonDominantHand.transform.position,
                                     Thing,
                                     out __instance.hit, 4f,
                                     __instance.environmentMask))
                    return false;

                __instance.AltHit(__instance.hit.transform);
                if (__instance.hit.transform.gameObject.layer != 8 && __instance.hit.transform.gameObject.layer != 24)
                    return false;

                __instance.hitSomething = true;
                __instance.transform.parent.localRotation = Quaternion.identity;
                Object.Instantiate(__instance.normalHit, __instance.transform.position, Quaternion.identity);
                __instance.currentDustParticle = Object.Instantiate(__instance.dustParticle, __instance.hit.point, __instance.transform.rotation);
                __instance.currentDustParticle.transform.forward = __instance.hit.normal;
                Breakable component5 = __instance.hit.transform.gameObject.GetComponent<Breakable>();
                if (component5 != null && !component5.precisionOnly && (component5.weak || __instance.type == FistType.Heavy))
                    component5.Break();

                if (__instance.hit.collider.gameObject.TryGetComponent<Bleeder>(out var component6))
                {
                    if (__instance.type == FistType.Standard)
                        component6.GetHit(__instance.hit.point, GoreType.Body);
                    else
                        component6.GetHit(__instance.hit.point, GoreType.Head);
                }

                if (__instance.type == FistType.Heavy)
                {
                    Glass component7 = __instance.hit.collider.gameObject.GetComponent<Glass>();
                    if (component7 != null && !component7.broken)
                        component7.Shatter();
                }
            }
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(Punch.BlastCheck))] static bool BlastCheck(Punch __instance)
        {
            if (MonoSingleton<InputManager>.Instance.InputSource.Punch.IsPressed)
            {
                __instance.holdingInput = false;
                __instance.anim.SetTrigger("PunchBlast");
                Vector3 position = Vars.NonDominantHand.transform.position + Thing * 2f;
                if (Physics.Raycast(Vars.NonDominantHand.transform.position, Thing, out var hitInfo,
                                    2f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies))) position = hitInfo.point - Thing * 0.1f;

                Object.Instantiate(__instance.blastWave, position, Vars.NonDominantHand.transform.rotation);
            }
            return false;
        }
    }
}

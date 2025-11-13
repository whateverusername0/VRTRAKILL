using HarmonyLib;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.Arms;
using VRTRAKILL.Systems.VRAvatar.Armature;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRAKILL.Patches.ULTRAKILL;

[HarmonyPatch(typeof(Punch))] internal static class PatchPunch
{
    public static Vector3 Direction;

    [HarmonyPostfix] [HarmonyPatch(nameof(Punch.Start))]
    static void Start(Punch __instance)
    {
        Arm A = null;
        switch (__instance.type)
        {
            case FistType.Standard: A = Arm.FeedbackerPreset(__instance.transform); break;
            case FistType.Heavy: A = Arm.KnuckleblasterPreset(__instance.transform); break;
            case FistType.Spear: default: break;
        }
        VRArmTransformer AT = __instance.gameObject.AddComponent<VRArmTransformer>();
        VRArmControllerBase AC = __instance.gameObject.AddComponent<VRArmController>();
        AT.Arm = A; AC.Arm = A;

        // inshallah pls stop
        foreach (SkinnedMeshRenderer SMR in __instance.GetComponentsInChildren<SkinnedMeshRenderer>())
            SMR.updateWhenOffscreen = true;
    }
    
    [HarmonyPrefix] [HarmonyPatch(nameof(Punch.Update))]
    static bool Update(Punch __instance)
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

    [HarmonyPrefix] [HarmonyPatch(nameof(Punch.ActiveFrame))]
    static bool ActiveFrame(Punch __instance, bool firstFrame = false)
    {
        var _i = __instance;
        var nonDomHand = Vars.NDHC.transform;

        if (_i.type == FistType.Standard && !_i.parriedSomething)
        {
            Collider[] array = Physics.OverlapSphere(nonDomHand.position, 0.01f, 16384, QueryTriggerInteraction.Collide);
            List<Transform> list = new List<Transform>();
            Collider[] array2 = array;
            foreach (Collider collider in array2)
            {
                list.Add(collider.transform);
                if (_i.TryParryProjectile((collider.attachedRigidbody != null) ? collider.attachedRigidbody.transform : collider.transform, firstFrame))
                {
                    break;
                }
            }

            bool flag = Physics.Raycast(nonDomHand.position, Direction, out _i.hit, 4f, 16384);
            if (!flag)
            {
                flag = Physics.BoxCast(nonDomHand.position, Vector3.one * 0.3f, Direction, out _i.hit, nonDomHand.rotation, 4f, 16384);
            }

            if (!flag || list.Contains(_i.hit.transform) || !_i.TryParryProjectile(_i.hit.transform, firstFrame))
            {
                if (_i.ppz == null)
                {
                    _i.ppz = _i.transform.parent.GetComponentInChildren<ProjectileParryZone>();
                }

                if (_i.ppz != null)
                {
                    Projectile projectile = _i.ppz.CheckParryZone();
                    if (projectile != null)
                    {
                        bool flag2 = !_i.alreadyBoostedProjectile && firstFrame;
                        if (!list.Contains(projectile.transform) && !projectile.unparryable && !projectile.undeflectable && (flag2 || !projectile.playerBullet))
                        {
                            _i.ParryProjectile(projectile);
                            _i.parriedSomething = true;
                            _i.hitSomething = true;
                        }
                    }
                }
            }
        }
        else if (_i.type == FistType.Heavy && !_i.hitSomething)
        {
            Transform transform = null;
            Collider[] array3 = Physics.OverlapSphere(nonDomHand.position, 0.1f, 16384);
            if (array3.Length != 0)
            {
                transform = array3[0].transform;
            }
            else if (Physics.Raycast(nonDomHand.position, Direction, out _i.hit, 4f, 16384) || Physics.BoxCast(nonDomHand.position, Vector3.one * 0.3f, Direction, out _i.hit, nonDomHand.rotation, 4f, 16384))
            {
                transform = _i.hit.transform;
            }

            if (transform != null)
            {
                if (transform.TryGetComponent<MassSpear>(out var component) && component.hitPlayer)
                {
                    Object.Instantiate(_i.specialHit, _i.transform.position, Quaternion.identity);
                    MonoSingleton<TimeController>.Instance.HitStop(0.1f);
                    //cc.CameraShake(0.5f * screenShakeMultiplier);
                    component.GetHurt(10f);
                    _i.hitSomething = true;
                }

                if (transform.TryGetComponent<Chainsaw>(out var component2))
                {
                    MonoSingleton<WeaponCharges>.Instance.punchStamina = 2f;
                    component2.transform.position = Vars.NDHC.transform.position;
                    component2.transform.rotation = Quaternion.LookRotation(component2.transform.position - Punch.GetParryLookTarget());
                    component2.transform.position -= component2.transform.forward;
                    component2.rb.velocity = component2.transform.forward * -105f;
                    component2.stopped = false;
                    MonoSingleton<TimeController>.Instance.ParryFlash();
                    component2.TurnIntoSawblade();
                    _i.hitSomething = true;
                }
            }
        }

        bool flag3 = Physics.Raycast(nonDomHand.position, Direction, out _i.hit, 4f, LayerMaskDefaults.Get(LMD.Enemies), QueryTriggerInteraction.Collide);
        if (!flag3)
        {
            flag3 = Physics.SphereCast(nonDomHand.position, 1f, Direction, out _i.hit, 4f, LayerMaskDefaults.Get(LMD.Enemies), QueryTriggerInteraction.Collide);
        }

        if (flag3)
        {
            if (!_i.alreadyHitCoin && _i.type == FistType.Standard && _i.hit.collider.CompareTag("Coin"))
            {
                Coin component3 = _i.hit.collider.GetComponent<Coin>();
                if ((bool)component3 && component3.doubled)
                {
                    _i.anim.Play("Hook", 0, 0.065f);
                    component3.DelayedPunchflection();
                    _i.alreadyHitCoin = true;
                }
            }

            if (_i.hitSomething)
            {
                return false;
            }
            bool flag4 = false;
            if (Physics.Raycast(nonDomHand.position, _i.hit.point - nonDomHand.position, out var hitInfo, 5f, _i.environmentMask) && Vector3.Distance(nonDomHand.position, _i.hit.point) > Vector3.Distance(nonDomHand.position, hitInfo.point))
            {
                flag4 = true;
            }

            if (!flag4)
            {
                _i.PunchSuccess(_i.hit.point, _i.hit.transform);
                _i.hitSomething = true;
            }
        }

        if (_i.hitSomething)
        {
            return false;
        }

        Collider[] array4 = Physics.OverlapSphere(nonDomHand.position, 0.1f, LayerMaskDefaults.Get(LMD.Enemies), QueryTriggerInteraction.Collide);
        if (array4 != null && array4.Length != 0)
        {
            Collider[] array2 = array4;
            foreach (Collider collider2 in array2)
            {
                _i.PunchSuccess(nonDomHand.position, collider2.transform);
            }

            _i.hitSomething = true;
        }

        if (_i.type == FistType.Standard && !_i.hitSomething && !_i.parriedSomething)
        {
            Collider[] array5 = Physics.OverlapSphere(nonDomHand.position + Direction * 3f, 3f, 16384, QueryTriggerInteraction.Collide);
            bool flag5 = false;
            Collider[] array2 = array5;
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
                    else
                    {
                        nail.rb.velocity = (Punch.GetParryLookTarget() - nail.transform.position).normalized * nail.rb.velocity.magnitude;
                    }

                    nail.punched = true;
                    if (nail.magnets.Count > 0)
                    {
                        nail.punchDistance = Vector3.Distance(nail.transform.position, nail.GetTargetMagnet().transform.position);
                    }
                }
            }

            if (!flag5)
            {
                array2 = Physics.OverlapSphere(nonDomHand.position + Direction, 1f, 1, QueryTriggerInteraction.Collide);
                foreach (Collider collider4 in array2)
                {
                    float num = Vector3.Distance(nonDomHand.position + Direction, collider4.transform.position);
                    if (num < 6f || num > 12f || Mathf.Abs((nonDomHand.position + Direction).y - collider4.transform.position.y) > 3f || !collider4.TryGetComponent<Magnet>(out var component4) || component4.sawblades.Count <= 0)
                    {
                        continue;
                    }

                    float num2 = float.PositiveInfinity;
                    float num3 = 0f;
                    int num4 = -1;
                    for (int num5 = component4.sawblades.Count - 1; num5 >= 0; num5--)
                    {
                        if (component4.sawblades[num5] == null)
                        {
                            component4.sawblades.RemoveAt(num5);
                            if (flag5)
                            {
                                num4--;
                            }
                        }
                        else
                        {
                            num3 = Vector3.Distance(component4.sawblades[num5].transform.position, nonDomHand.position);
                            if (component4.sawblades[num5] != null && (num4 < 0 || num2 < num3))
                            {
                                num4 = num5;
                                num2 = num3;
                                flag5 = true;
                            }
                        }
                    }

                    if (!flag5 || !component4.sawblades[num4].TryGetComponent<Nail>(out var component5))
                    {
                        continue;
                    }

                    component5.transform.position = nonDomHand.position + Direction;
                    if (component5.stopped)
                    {
                        component5.stopped = false;
                        component5.rb.velocity = (Punch.GetParryLookTarget() - component5.transform.position).normalized * component5.originalVelocity.magnitude;
                    }
                    else
                    {
                        component5.rb.velocity = (Punch.GetParryLookTarget() - component5.transform.position).normalized * component5.rb.velocity.magnitude;
                    }

                    component5.punched = true;
                    if (component5.magnets.Count > 0)
                    {
                        Magnet targetMagnet = component5.GetTargetMagnet();
                        if (Vector3.Distance(component5.transform.position + component5.rb.velocity.normalized, targetMagnet.transform.position) > Vector3.Distance(component5.transform.position, targetMagnet.transform.position))
                        {
                            component5.MagnetRelease(targetMagnet);
                        }
                        else
                        {
                            component5.punchDistance = Vector3.Distance(component5.transform.position, targetMagnet.transform.position);
                        }
                    }

                    break;
                }
            }

            if (flag5)
            {
                Object.Instantiate(_i.specialHit, _i.transform.position, Quaternion.identity);
                MonoSingleton<TimeController>.Instance.HitStop(0.1f);
                _i.anim.Play("Hook", -1, 0.065f);
                _i.parriedSomething = true;
                _i.hitSomething = true;
            }
        }

        if (Physics.CheckSphere(nonDomHand.position, 0.01f, _i.environmentMask, QueryTriggerInteraction.Collide))
        {
            Collider[] array2 = Physics.OverlapSphere(nonDomHand.position, 0.01f, _i.environmentMask);
            foreach (Collider collider5 in array2)
            {
                _i.AltHit(collider5.transform);
            }
        }
        else
        {
            if (!Physics.Raycast(nonDomHand.position, Direction, out _i.hit, 4f, _i.environmentMask))
            {
                return false;
            }

            _i.AltHit(_i.hit.transform);
            if (!LayerMaskDefaults.IsMatchingLayer(_i.hit.transform.gameObject.layer, LMD.Environment))
            {
                return false;
            }

            _i.hitSomething = true;
            _i.transform.parent.localRotation = Quaternion.identity;
            //cc.CameraShake(0.2f * screenShakeMultiplier);
            Object.Instantiate(_i.normalHit, _i.transform.position, Quaternion.identity);
            _i.currentDustParticle = Object.Instantiate(_i.dustParticle, _i.hit.point, _i.transform.rotation);
            _i.currentDustParticle.transform.forward = _i.hit.normal;
            Breakable component6 = _i.hit.transform.gameObject.GetComponent<Breakable>();
            if (component6 != null && !component6.precisionOnly && !component6.specialCaseOnly && (component6.weak || _i.type == FistType.Heavy))
            {
                component6.Break();
            }

            if (_i.hit.collider.gameObject.TryGetComponent<Bleeder>(out var component7))
            {
                if (_i.type == FistType.Standard)
                {
                    component7.GetHit(_i.hit.point, GoreType.Body);
                }
                else
                {
                    component7.GetHit(_i.hit.point, GoreType.Head);
                }
            }

            if (_i.type == FistType.Heavy)
            {
                Glass component8 = _i.hit.collider.gameObject.GetComponent<Glass>();
                if (component8 != null && !component8.broken)
                {
                    component8.Shatter();
                }
            }

            _i.HitSurface(_i.hit);
        }
        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Punch.BlastCheck))]
    static bool BlastCheck(Punch __instance)
    {
        if (MonoSingleton<InputManager>.Instance.InputSource.Punch.IsPressed)
        {
            __instance.holdingInput = false;
            __instance.anim.SetTrigger("PunchBlast");
            Vector3 position = Vars.NonDominantHand.transform.position + Vars.NonDominantHand.transform.forward * 2f;
            if (Physics.Raycast(Vars.NonDominantHand.transform.position, Vars.NonDominantHand.transform.forward, out var hitInfo,
                                2f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies))) position = hitInfo.point - Vars.NonDominantHand.transform.forward * 0.1f;

            Object.Instantiate(__instance.blastWave, position, Vars.NonDominantHand.transform.rotation);
        }
        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Punch.GetParryLookTarget))]
    static bool GetParryLookTarget(ref Vector3 __result)
    {
        Vector3 vector = Vars.NDHC.transform.forward;
        //if ((bool)MonoSingleton<CameraFrustumTargeter>.Instance && (bool)MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
        //{
        //    vector = MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center - Vars.NDHC.transform.position;
        //}

        if (Physics.Raycast(Vars.NDHC.transform.position, vector, out var hitInfo, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Enemies), QueryTriggerInteraction.Ignore))
        {
            __result = hitInfo.point;
        }

        __result = Vars.NDHC.transform.position + vector * 1000f;
        return false;
    }
}

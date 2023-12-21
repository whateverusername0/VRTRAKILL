﻿using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] internal sealed class AimingP
    {
        // Replaces all guns shooting raycasts directions with controller directions

        // Revolver
        [HarmonyPrefix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.Shoot))] static bool RevolverA(int shotType, Revolver __instance)
        {
            __instance.cc.StopShake();
            __instance.shootReady = false;
            __instance.shootCharge = 0f;

            Vector3 AltShootPos;
            if (__instance.altVersion) { MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[__instance.gunVariation] = 2f; }
            AltShootPos = Vars.DominantHand.transform.position + (Vars.DominantHand.transform.forward * 1.25f) + new Vector3(0, .035f, 0);

            switch (shotType)
            {
                case 1:
                    {
                        GameObject gameObject2 =
                            Object.Instantiate(__instance.revolverBeam,
                                               Vars.DominantHand.transform.position,
                                               Vars.DominantHand.transform.rotation);
                        gameObject2.transform.localScale *= .5f;

                        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                            gameObject2.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);

                        RevolverBeam component2 = gameObject2.GetComponent<RevolverBeam>();
                        component2.sourceWeapon = __instance.gc.currentWeapon;
                        component2.alternateStartPoint = AltShootPos;
                        component2.gunVariation = __instance.gunVariation;

                        if (__instance.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                            component2.quickDraw = true;

                        __instance.currentGunShot = Random.Range(0, __instance.gunShots.Length);

                        __instance.gunAud.clip = __instance.gunShots[__instance.currentGunShot];
                        __instance.gunAud.volume = 0.55f;
                        __instance.gunAud.pitch = Random.Range(0.9f, 1.1f);
                        __instance.gunAud.Play();

                        //__instance.cam.fieldOfView += __instance.cc.defaultFov / 40f;

                        MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFire, __instance.gameObject);
                        break;
                    }
                case 2:
                    {
                        GameObject gameObject =
                            Object.Instantiate(__instance.revolverBeamSuper,
                                               Vars.DominantHand.transform.position,
                                               Vars.DominantHand.transform.rotation);
                        gameObject.transform.localScale *= .5f;

                        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                            gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);

                        RevolverBeam component = gameObject.GetComponent<RevolverBeam>();
                        component.sourceWeapon = __instance.gc.currentWeapon;
                        component.alternateStartPoint = AltShootPos;
                        component.gunVariation = __instance.gunVariation;

                        if (__instance.gunVariation == 2)
                            component.ricochetAmount = Mathf.Min(3, Mathf.FloorToInt(__instance.pierceShotCharge / 25f));

                        __instance.pierceShotCharge = 0f;

                        if (__instance.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                            component.quickDraw = true;

                        __instance.pierceReady = false;
                        __instance.pierceCharge = 0f;

                        if (__instance.gunVariation == 0)
                        {
                            __instance.screenAud.clip = __instance.chargingSound;
                            __instance.screenAud.loop = true;

                            if (__instance.altVersion) __instance.screenAud.pitch = 0.5f;
                            else __instance.screenAud.pitch = 1f;

                            __instance.screenAud.volume = 0.55f;
                            __instance.screenAud.Play();
                        }
                        else if (!__instance.wid || __instance.wid.delay == 0f)
                            __instance.wc.rev2charge -= (__instance.altVersion ? 300 : 100);

                        if ((bool)__instance.superGunAud)
                        {
                            __instance.currentGunShot = Random.Range(0, __instance.superGunShots.Length);

                            __instance.superGunAud.clip = __instance.superGunShots[__instance.currentGunShot];
                            __instance.superGunAud.volume = 0.5f;
                            __instance.superGunAud.pitch = Random.Range(0.9f, 1.1f);
                            __instance.superGunAud.Play();
                        }

                        if (__instance.gunVariation == 2 && (bool)__instance.twirlShotSound)
                            Object.Instantiate(__instance.twirlShotSound, __instance.transform.position, Quaternion.identity);

                        __instance.cam.fieldOfView += __instance.cc.defaultFov / 20f;
                        MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFireStrong, __instance.gameObject);
                        break;
                    }
            }

            if (!__instance.altVersion) __instance.cylinder.DoTurn();

            __instance.anim.SetFloat("RandomChance", Random.Range(0f, 1f));

            if (shotType == 1) __instance.anim.SetTrigger("Shoot");
            else __instance.anim.SetTrigger("ChargeShoot");

            __instance.gunReady = false;

            return false;
        }
        // Coin toss
        [HarmonyPrefix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.ThrowCoin))] static bool RevolverCoin(Revolver __instance)
        {
            if (__instance.punch == null || !__instance.punch.gameObject.activeInHierarchy)
                __instance.punch = MonoSingleton<FistControl>.Instance.currentPunch;

            if ((bool)__instance.punch) __instance.punch.CoinFlip();

            GameObject obj;
            if (Vars.Config.EnableMBP)
                obj = Object.Instantiate(__instance.coin,
                                         Vars.NonDominantHand.transform.position + Vars.NonDominantHand.transform.up * -.5f,
                                         Vars.NonDominantHand.transform.rotation);
            else if (Vars.Config.EnableCBS)
                obj = Object.Instantiate(__instance.coin,
                                         Vars.DominantHand.transform.position + Vars.DominantHand.transform.up * -.5f,
                                         Vars.DominantHand.transform.rotation);
            else obj = Object.Instantiate(__instance.coin,
                                          __instance.camObj.transform.position + __instance.camObj.transform.up * -0.5f,
                                          __instance.camObj.transform.rotation);

            obj.GetComponent<Coin>().sourceWeapon = __instance.gc.currentWeapon;

            MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.CoinToss);

            Vector3 zero = Vector3.zero;
            obj.GetComponent<Rigidbody>().AddForce(Vars.DominantHand.transform.forward * 20f + Vector3.up * 15f
                                                   + (MonoSingleton<NewMovement>.Instance.ridingRocket
                                                      ? MonoSingleton<NewMovement>.Instance.ridingRocket.rb.velocity
                                                      : MonoSingleton<NewMovement>.Instance.rb.velocity) + zero,
                                                   ForceMode.VelocityChange);
            __instance.pierceCharge = 0f;
            __instance.pierceReady = false;

            return false;
        }

        // Shotgun
        // Normal fire
        [HarmonyPrefix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Shoot))] static bool ShotgunA(Shotgun __instance)
        {
            __instance.gunReady = false;
            int num = 12;
            if (__instance.variation == 1)
            {
                switch (__instance.primaryCharge)
                {
                    case 0:
                        num = 10;
                        __instance.gunAud.pitch = Random.Range(1.15f, 1.25f);
                        break;
                    case 1:
                        num = 16;
                        __instance.gunAud.pitch = Random.Range(0.95f, 1.05f);
                        break;
                    case 2:
                        num = 24;
                        __instance.gunAud.pitch = Random.Range(0.75f, 0.85f);
                        break;
                    case 3:
                        num = 0;
                        __instance.gunAud.pitch = Random.Range(0.75f, 0.85f);
                        break;
                }
            }
            MonoSingleton<CameraController>.Instance.StopShake();
            Vector3 direction = Vars.DominantHand.transform.forward;
            if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
            {
                direction = __instance.targeter.CurrentTarget.bounds.center - Vars.DominantHand.transform.position;
            }
            __instance.rhits = Physics.RaycastAll(Vars.DominantHand.transform.position, direction, 4f, __instance.shotgunZoneLayerMask);
            if (__instance.rhits.Length != 0)
            {
                foreach (RaycastHit raycastHit in __instance.rhits)
                {
                    if (raycastHit.collider.gameObject.tag == "Body")
                    {
                        EnemyIdentifierIdentifier componentInParent = raycastHit.collider.GetComponentInParent<EnemyIdentifierIdentifier>();
                        if (componentInParent)
                        {
                            EnemyIdentifier eid = componentInParent.eid;
                            if (!eid.dead && !eid.blessed && __instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Equip"))
                            {
                                MonoSingleton<StyleHUD>.Instance.AddPoints(50, "ultrakill.quickdraw", __instance.gc.currentWeapon, eid, -1, "", "");
                            }
                            eid.hitter = "shotgunzone";
                            if (!eid.hitterWeapons.Contains("shotgun" + __instance.variation))
                            {
                                eid.hitterWeapons.Add("shotgun" + __instance.variation);
                            }
                            eid.DeliverDamage(raycastHit.collider.gameObject, (eid.transform.position - __instance.transform.position).normalized * 10000f, raycastHit.point, 4f, false, 0f, __instance.gameObject, false);
                        }
                    }
                }
            }
            MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFireProjectiles, __instance.gameObject);
            if (__instance.variation != 1 || __instance.primaryCharge != 3)
            {
                for (int j = 0; j < num; j++)
                {
                    GameObject gameObject = Object.Instantiate<GameObject>(__instance.bullet, Vars.DominantHand.transform.position, Vars.DominantHand.transform.rotation);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    component.weaponType = "shotgun" + __instance.variation;
                    component.sourceWeapon = __instance.gc.currentWeapon;
                    if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                    {
                        gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
                    }
                    if (__instance.variation == 1)
                    {
                        switch (__instance.primaryCharge)
                        {
                            case 0:
                                gameObject.transform.Rotate(Random.Range(-__instance.spread / 1.5f, __instance.spread / 1.5f),
                                                            Random.Range(-__instance.spread / 1.5f, __instance.spread / 1.5f),
                                                            Random.Range(-__instance.spread / 1.5f, __instance.spread / 1.5f));
                                break;
                            case 1:
                                gameObject.transform.Rotate(Random.Range(-__instance.spread, __instance.spread),
                                                            Random.Range(-__instance.spread, __instance.spread),
                                                            Random.Range(-__instance.spread, __instance.spread));
                                break;
                            case 2:
                                gameObject.transform.Rotate(Random.Range(-__instance.spread * 2f, __instance.spread * 2f),
                                                            Random.Range(-__instance.spread * 2f, __instance.spread * 2f),
                                                            Random.Range(-__instance.spread * 2f, __instance.spread * 2f));
                                break;
                        }
                    }
                    else
                    {
                        gameObject.transform.Rotate(Random.Range(-__instance.spread, __instance.spread),
                                                    Random.Range(-__instance.spread, __instance.spread),
                                                    Random.Range(-__instance.spread, __instance.spread));
                    }
                }
            }
            else
            {
                Vector3 position = Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward;
                RaycastHit raycastHit2;
                if (Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.forward, out raycastHit2, 1f, LayerMaskDefaults.Get(LMD.Environment)))
                {
                    position = raycastHit2.point - Vars.DominantHand.transform.forward * 0.1f;
                }
                GameObject gameObject2 = Object.Instantiate<GameObject>(__instance.explosion, position, Vars.DominantHand.transform.rotation);
                if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                {
                    gameObject2.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
                }
                foreach (Explosion explosion in gameObject2.GetComponentsInChildren<Explosion>())
                {
                    explosion.sourceWeapon = __instance.gc.currentWeapon;
                    explosion.enemyDamageMultiplier = 1f;
                    explosion.maxSize *= 1.5f;
                    explosion.damage = 50;
                }
            }
            if (__instance.variation != 1)
            {
                __instance.gunAud.pitch = Random.Range(0.95f, 1.05f);
            }
            __instance.gunAud.clip = __instance.shootSound;
            __instance.gunAud.volume = 0.45f;
            __instance.gunAud.panStereo = 0f;
            __instance.gunAud.Play();
            __instance.cc.CameraShake(1f);
            if (__instance.variation == 1)
            {
                __instance.anim.SetTrigger("PumpFire");
            }
            else
            {
                __instance.anim.SetTrigger("Fire");
            }
            foreach (Transform transform in __instance.shootPoints)
            {
                Object.Instantiate<GameObject>(__instance.muzzleFlash, transform.transform.position, transform.transform.rotation);
            }
            __instance.releasingHeat = false;
            __instance.tempColor.a = 1f;
            __instance.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", __instance.tempColor);
            if (__instance.variation == 1)
            {
                __instance.primaryCharge = 0;
            }

            return false;
        }
        // Grenade (core eject) (BA - set grenade vector to controller, BB - same thing but in diferent method)
        [HarmonyPrefix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Update))] static bool ShotgunBA(Shotgun __instance)
        {
            if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked && !__instance.charging)
            {
                if (!__instance.wid || __instance.wid.delay == 0f)
                {
                    __instance.Shoot();
                }
                else
                {
                    __instance.gunReady = false;
                    __instance.Invoke("Shoot", __instance.wid.delay);
                }
            }
            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && __instance.variation == 1 && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
            {
                __instance.gunReady = false;
                if (!__instance.wid || __instance.wid.delay == 0f)
                {
                    __instance.Pump();
                }
                else
                {
                    __instance.Invoke("Pump", __instance.wid.delay);
                }
            }
            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && __instance.variation == 0 && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
            {
                __instance.charging = true;
                if (__instance.grenadeForce < 60f)
                {
                    __instance.grenadeForce = Mathf.MoveTowards(__instance.grenadeForce, 60f, Time.deltaTime * 60f);
                }
                __instance.grenadeVector = new Vector3(Vars.DominantHand.transform.forward.x, Vars.DominantHand.transform.forward.y, Vars.DominantHand.transform.forward.z);
                if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                {
                    __instance.grenadeVector = Vector3.Normalize(__instance.targeter.CurrentTarget.bounds.center - Vars.DominantHand.transform.position);
                }
                __instance.grenadeVector += new Vector3(0f, __instance.grenadeForce * 0.002f, 0f);
                __instance.transform.localPosition = new Vector3(__instance.wpos.currentDefault.x + Random.Range(__instance.grenadeForce / 3000f * -1f, __instance.grenadeForce / 3000f), __instance.wpos.currentDefault.y + Random.Range(__instance.grenadeForce / 3000f * -1f, __instance.grenadeForce / 3000f), __instance.wpos.currentDefault.z + Random.Range(__instance.grenadeForce / 3000f * -1f, __instance.grenadeForce / 3000f));
                if (__instance.tempChargeSound == null)
                {
                    GameObject gameObject = Object.Instantiate<GameObject>(__instance.chargeSoundBubble);
                    __instance.tempChargeSound = gameObject.GetComponent<AudioSource>();
                    if (__instance.wid && __instance.wid.delay > 0f)
                    {
                        __instance.tempChargeSound.volume -= __instance.wid.delay * 2f;
                        if (__instance.tempChargeSound.volume < 0f)
                        {
                            __instance.tempChargeSound.volume = 0f;
                        }
                    }
                }
                MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.ShotgunCharge, __instance.tempChargeSound.gameObject).intensityMultiplier = __instance.grenadeForce / 60f;
                __instance.tempChargeSound.pitch = __instance.grenadeForce / 60f;
            }
            if ((MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasCanceledThisFrame || (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && !GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)) && __instance.variation == 0 && __instance.gunReady && __instance.gc.activated && __instance.charging)
            {
                __instance.charging = false;
                if (!__instance.wid || __instance.wid.delay == 0f)
                {
                    __instance.ShootSinks();
                }
                else
                {
                    __instance.gunReady = false;
                    __instance.Invoke("ShootSinks", __instance.wid.delay);
                }
                Object.Destroy(__instance.tempChargeSound.gameObject);
            }
            if (__instance.releasingHeat)
            {
                __instance.tempColor.a = __instance.tempColor.a - Time.deltaTime * 2.5f;
                __instance.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", __instance.tempColor);
            }
            __instance.UpdateMeter();

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.ShootSinks))] static bool ShotgunBB(Shotgun __instance)
        {
            __instance.gunReady = false;
            __instance.transform.localPosition = __instance.wpos.currentDefault;
            foreach (Transform transform in __instance.shootPoints)
            {
                GameObject gameObject = Object.Instantiate<GameObject>(__instance.grenade, Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward * 0.5f, Random.rotation);
                gameObject.GetComponentInChildren<Grenade>().sourceWeapon = __instance.gc.currentWeapon;
                gameObject.GetComponent<Collider>();
                gameObject.GetComponent<Rigidbody>().AddForce(__instance.grenadeVector * (__instance.grenadeForce + 10f), ForceMode.VelocityChange);
            }
            Object.Instantiate<GameObject>(__instance.grenadeSoundBubble).GetComponent<AudioSource>().volume = 0.45f * Mathf.Sqrt(Mathf.Pow(1f, 2f) - Mathf.Pow(__instance.grenadeForce, 2f) / Mathf.Pow(60f, 2f));
            __instance.anim.SetTrigger("Secondary Fire");
            __instance.gunAud.clip = __instance.shootSound;
            __instance.gunAud.volume = 0.45f * (__instance.grenadeForce / 60f);
            __instance.gunAud.panStereo = 0f;
            __instance.gunAud.pitch = Random.Range(0.75f, 0.85f);
            __instance.gunAud.Play();
            __instance.cc.CameraShake(1f);
            __instance.meterOverride = true;
            __instance.chargeSlider.value = 0f;
            __instance.sliderFill.color = Color.black;
            foreach (Transform transform2 in __instance.shootPoints)
            {
                Object.Instantiate<GameObject>(__instance.muzzleFlash, transform2.transform.position, transform2.transform.rotation);
            }
            __instance.releasingHeat = false;
            __instance.tempColor.a = 0f;
            __instance.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", __instance.tempColor);
            __instance.grenadeForce = 0f;

            return false;
        }

        // Nailgun
        // Normal fire / saw
        [HarmonyPrefix] [HarmonyPatch(typeof(Nailgun), nameof(Nailgun.Shoot))] static bool NailgunA(Nailgun __instance)
        {
            __instance.UpdateAnimationWeight();
            __instance.fireCooldown = __instance.currentFireRate;
            __instance.shotSuccesfully = true;
            if (__instance.variation == 1 && (!__instance.wid || __instance.wid.delay == 0f))
            {
                if (__instance.altVersion)
                {
                    __instance.wc.naiSaws -= 1f;
                }
                else
                {
                    __instance.wc.naiAmmo -= 1f;
                }
            }
            __instance.anim.SetTrigger("Shoot");
            __instance.barrelNum++;
            if (__instance.barrelNum >= __instance.shootPoints.Length)
            {
                __instance.barrelNum = 0;
            }
            GameObject gameObject;
            if (__instance.burnOut)
            {
                gameObject = Object.Instantiate<GameObject>(__instance.muzzleFlash2, __instance.shootPoints[__instance.barrelNum].transform);
            }
            else
            {
                gameObject = Object.Instantiate<GameObject>(__instance.muzzleFlash, __instance.shootPoints[__instance.barrelNum].transform);
            }
            if (!__instance.altVersion)
            {
                AudioSource component = gameObject.GetComponent<AudioSource>();
                if (__instance.burnOut)
                {
                    component.volume = 0.65f - __instance.wid.delay * 2f;
                    if (component.volume < 0f)
                    {
                        component.volume = 0f;
                    }
                    component.pitch = 2f;
                    __instance.currentSpread = __instance.spread * 2f;
                }
                else
                {
                    if (__instance.heatSinks < 1f)
                    {
                        component.pitch = 0.25f;
                        component.volume = 0.25f - __instance.wid.delay * 2f;
                        if (component.volume < 0f)
                        {
                            component.volume = 0f;
                        }
                    }
                    else
                    {
                        component.volume = 0.65f - __instance.wid.delay * 2f;
                        if (component.volume < 0f)
                        {
                            component.volume = 0f;
                        }
                    }
                    __instance.currentSpread = __instance.spread;
                }
            }
            else if (__instance.burnOut)
            {
                __instance.currentSpread = 45f;
            }
            else if (__instance.altVersion && __instance.variation == 0)
            {
                if (__instance.heatSinks < 1f)
                {
                    __instance.currentSpread = 45f;
                }
                else
                {
                    __instance.currentSpread = Mathf.Lerp(0f, 45f, Mathf.Max(0f, __instance.heatUp - 0.25f));
                }
            }
            else
            {
                __instance.currentSpread = 0f;
            }
            GameObject gameObject2;
            if (__instance.burnOut)
            {
                gameObject2 = Object.Instantiate<GameObject>(__instance.heatedNail, Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward, __instance.transform.rotation);
            }
            else
            {
                gameObject2 = Object.Instantiate<GameObject>(__instance.nail, Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward, __instance.transform.rotation);
            }
            if (__instance.altVersion && __instance.variation == 0 && __instance.heatSinks >= 1f)
            {
                __instance.heatUp = Mathf.MoveTowards(__instance.heatUp, 1f, 0.125f);
            }
            gameObject2.transform.forward = Vars.DominantHand.transform.forward;
            if (Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.forward, 1f, LayerMaskDefaults.Get(LMD.Environment)))
            {
                gameObject2.transform.position = Vars.DominantHand.transform.position;
            }
            if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
            {
                gameObject2.transform.position = Vars.DominantHand.transform.position + (__instance.targeter.CurrentTarget.bounds.center - Vars.DominantHand.transform.position).normalized;
                gameObject2.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
            }
            gameObject2.transform.Rotate(Random.Range(-__instance.currentSpread / 3f, __instance.currentSpread / 3f), Random.Range(-__instance.currentSpread / 3f, __instance.currentSpread / 3f), Random.Range(-__instance.currentSpread / 3f, __instance.currentSpread / 3f));
            Rigidbody rigidbody;
            if (gameObject2.TryGetComponent<Rigidbody>(out rigidbody))
            {
                rigidbody.velocity = gameObject2.transform.forward * 200f;
            }
            Nail nail;
            if (gameObject2.TryGetComponent<Nail>(out nail))
            {
                nail.sourceWeapon = __instance.gc.currentWeapon;
                nail.weaponType = __instance.projectileVariationTypes[__instance.variation];
                if (__instance.altVersion && __instance.variation == 0)
                {
                    if (__instance.heatSinks >= 1f)
                    {
                        nail.hitAmount = Mathf.Lerp(3f, 1f, __instance.heatUp);
                    }
                    else
                    {
                        nail.hitAmount = 1f;
                    }
                }
                if (nail.sawblade)
                {
                    nail.ForceCheckSawbladeRicochet();
                }
            }
            if (!__instance.burnOut)
            {
                __instance.cc.CameraShake(0.1f);
            }
            else
            {
                __instance.cc.CameraShake(0.35f);
            }
            if (__instance.altVersion)
            {
                MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Sawblade);
            }

            return false;
        }
        // Supersaw
        [HarmonyPrefix] [HarmonyPatch(typeof(Nailgun), nameof(Nailgun.SuperSaw))] static bool NailgunB(Nailgun __instance)
        {
            __instance.fireCooldown = __instance.currentFireRate;
            __instance.shotSuccesfully = true;
            __instance.anim.SetLayerWeight(1, 0f);
            __instance.anim.SetTrigger("SuperShoot");
            MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.SuperSaw);
            __instance.barrelNum++;
            if (__instance.barrelNum >= __instance.shootPoints.Length)
            {
                __instance.barrelNum = 0;
            }
            Object.Instantiate<GameObject>(__instance.muzzleFlash2, __instance.shootPoints[__instance.barrelNum].transform);
            __instance.currentSpread = 0f;
            GameObject gameObject = Object.Instantiate<GameObject>(__instance.heatedNail, Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward, __instance.transform.rotation);
            gameObject.transform.forward = Vars.DominantHand.transform.forward;
            if (Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.forward, 1f, LayerMaskDefaults.Get(LMD.Environment)))
            {
                gameObject.transform.position = Vars.DominantHand.transform.position;
            }
            if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
            {
                gameObject.transform.position = Vars.DominantHand.transform.position + (__instance.targeter.CurrentTarget.bounds.center - Vars.DominantHand.transform.position).normalized;
                gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
            }
            Rigidbody rigidbody;
            if (gameObject.TryGetComponent<Rigidbody>(out rigidbody))
            {
                rigidbody.velocity = gameObject.transform.forward * 200f;
            }
            Nail nail;
            if (gameObject.TryGetComponent<Nail>(out nail))
            {
                nail.weaponType = __instance.projectileVariationTypes[__instance.variation];
                nail.multiHitAmount = Mathf.RoundToInt(__instance.heatUp * 3f);
                nail.ForceCheckSawbladeRicochet();
                nail.sourceWeapon = __instance.gc.currentWeapon;
            }
            __instance.heatSinks -= 1f;
            __instance.heatUp = 0f;
            __instance.cc.CameraShake(0.5f);

            return false;
        }

        // Railgun (railing enemies in ultrakill :O)
        [HarmonyPrefix] [HarmonyPatch(typeof(Railcannon), nameof(Railcannon.Shoot))] static bool RailgunA(Railcannon __instance)
        {
            GameObject gameObject = Object.Instantiate<GameObject>(__instance.beam, Vars.DominantHand.transform.position, Vars.DominantHand.transform.rotation);
            if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
            {
                gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
            }
            if (__instance.variation != 1)
            {
                RevolverBeam revolverBeam;
                if (gameObject.TryGetComponent<RevolverBeam>(out revolverBeam))
                {
                    revolverBeam.sourceWeapon = __instance.gc.currentWeapon;
                    revolverBeam.alternateStartPoint = __instance.shootPoint.position;
                }
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 250f, ForceMode.VelocityChange);
            }
            Object.Instantiate<GameObject>(__instance.fireSound);
            __instance.anim.SetTrigger("Shoot");
            __instance.cc.CameraShake(2f);
            MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.GunFireStrong);

            return false;
        }

        // Rocket launcher
        // Normal fire
        [HarmonyPostfix] [HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.Update))] static void RL_VRIK(RocketLauncher __instance)
        {
            if (VRAvatar.VRigController.Instance != null)
            {
                __instance.transform.position = VRAvatar.VRigController.Instance.Rig.FeedbackerB.Forearm.position;
                __instance.transform.LookAt(VRAvatar.VRigController.Instance.Rig.FeedbackerB.Hand.Root);
            }
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.Shoot))] static bool RocketLauncherA(RocketLauncher __instance)
        {
            if (__instance.aud)
            {
                __instance.aud.pitch = Random.Range(0.9f, 1.1f);
                __instance.aud.Play();
            }
            if (__instance.variation == 1 && __instance.cbCharge > 0f)
            {
                __instance.chargeSound.Stop();
                __instance.cbCharge = 0f;
            }
            Object.Instantiate<GameObject>(__instance.muzzleFlash, __instance.shootPoint.position, Vars.DominantHand.transform.rotation);
            __instance.anim.SetTrigger("Fire");
            __instance.cooldown = __instance.rateOfFire;
            GameObject gameObject = Object.Instantiate<GameObject>(__instance.rocket, Vars.DominantHand.transform.position, __instance.transform.rotation);
            if (MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
            {
                gameObject.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
            }
            Grenade component = gameObject.GetComponent<Grenade>();
            if (component)
            {
                component.sourceWeapon = MonoSingleton<GunControl>.Instance.currentWeapon;
            }
            MonoSingleton<CameraController>.Instance.CameraShake(0.75f);
            MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFire, __instance.gameObject);

            return false;
        }
        // Cannonball
        [HarmonyPrefix] [HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.ShootCannonball))] static bool RocketLauncherB(RocketLauncher __instance)
        {
            if (__instance.aud)
            {
                __instance.aud.pitch = Random.Range(0.6f, 0.8f);
                __instance.aud.Play();
            }
            Object.Instantiate<GameObject>(__instance.muzzleFlash, __instance.shootPoint.position, Vars.DominantHand.transform.rotation);
            __instance.anim.SetTrigger("Fire");
            __instance.cooldown = __instance.rateOfFire;
            Rigidbody rigidbody = Object.Instantiate<Rigidbody>(__instance.cannonBall, Vars.DominantHand.transform.position + __instance.transform.forward, __instance.transform.rotation);
            if (MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
            {
                rigidbody.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
            }
            rigidbody.velocity = rigidbody.transform.forward * Mathf.Max(15f, __instance.cbCharge * 150f);
            Cannonball cannonball;
            if (rigidbody.TryGetComponent<Cannonball>(out cannonball))
            {
                cannonball.sourceWeapon = MonoSingleton<GunControl>.Instance.currentWeapon;
            }
            MonoSingleton<CameraController>.Instance.CameraShake(0.75f);
            __instance.cbCharge = 0f;

            return false;
        }
    }
}

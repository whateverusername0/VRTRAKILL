using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] internal class AimingP
    {
        // Replaces all guns shooting raycasts directions with controller directions
        [HarmonyPrefix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.Shoot))] static bool Pistol(int shotType, Revolver __instance)
        {
            __instance.cc.StopShake();
            __instance.shootReady = false;
            __instance.shootCharge = 0f;

            if (__instance.altVersion)
                MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[__instance.gunVariation] = 2f;

            switch (shotType)
            {
                case 1:
                    {
                        GameObject gameObject2 =
                            Object.Instantiate(__instance.revolverBeam,
                                               Vars.RightController.transform.position,
                                               Vars.RightController.transform.rotation);

                        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                            gameObject2.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);

                        RevolverBeam component2 = gameObject2.GetComponent<RevolverBeam>();
                        component2.sourceWeapon = __instance.gc.currentWeapon;
                        component2.alternateStartPoint = __instance.gunBarrel.transform.position;
                        component2.gunVariation = __instance.gunVariation;

                        if (__instance.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                            component2.quickDraw = true;

                        __instance.currentGunShot = Random.Range(0, __instance.gunShots.Length);

                        __instance.gunAud.clip = __instance.gunShots[__instance.currentGunShot];
                        __instance.gunAud.volume = 0.55f;
                        __instance.gunAud.pitch = Random.Range(0.9f, 1.1f);
                        __instance.gunAud.Play();

                        //__instance.cam.fieldOfView += __instance.cc.defaultFov / 40f;

                        MonoSingleton<RumbleManager>.Instance.SetVibrationTracked("rumble.gun.fire", __instance.gameObject);
                        break;
                    }
                case 2:
                    {
                        GameObject gameObject =
                            Object.Instantiate(__instance.revolverBeamSuper,
                                               Vars.RightController.transform.position,
                                               Vars.RightController.transform.rotation);

                        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                            gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);

                        RevolverBeam component = gameObject.GetComponent<RevolverBeam>();
                        component.sourceWeapon = __instance.gc.currentWeapon;
                        component.alternateStartPoint = __instance.gunBarrel.transform.position;
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
                        MonoSingleton<RumbleManager>.Instance.SetVibrationTracked("rumble.gun.fire_strong", __instance.gameObject);
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


    }
}

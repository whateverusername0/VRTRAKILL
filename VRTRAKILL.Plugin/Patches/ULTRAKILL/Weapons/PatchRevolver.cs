using HarmonyLib;
using Plugin.Systems;
using UnityEngine;

namespace Plugin.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(Revolver))] internal class PatchRevolver
{
    [HarmonyPrefix] [HarmonyPatch(nameof(Revolver.Shoot))]
    private static bool Shoot(int shotType, Revolver __instance)
    {
        __instance.shootReady = false;
        __instance.shootCharge = 0f;
        if (__instance.altVersion) MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[__instance.gunVariation] = 2f;
        var altShootPos = Vars.DominantHand.transform.position + (Vars.DominantHand.transform.forward * 1.25f) + new Vector3(0, .035f, 0);

        switch (shotType)
        {
            case 1:
                {
                    GameObject gameObject2 = Object.Instantiate(__instance.revolverBeam, Vars.DominantHand.transform.position, Vars.DominantHand.transform.rotation);
                    if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                        gameObject2.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);

                    RevolverBeam component2 = gameObject2.GetComponent<RevolverBeam>();
                    component2.sourceWeapon = __instance.gc.currentWeapon;
                    component2.alternateStartPoint = altShootPos;
                    component2.gunVariation = __instance.gunVariation;
                    if (__instance.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                        component2.quickDraw = true;

                    __instance.currentGunShot = Random.Range(0, __instance.gunShots.Length);
                    __instance.gunAud.clip = __instance.gunShots[__instance.currentGunShot];
                    __instance.gunAud.volume = 0.55f;
                    __instance.gunAud.pitch = Random.Range(0.9f, 1.1f);
                    __instance.gunAud.Play();
                    MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFire, __instance.gameObject);
                    break;
                }
            case 2:
                {
                    GameObject gameObject = Object.Instantiate(__instance.revolverBeamSuper, Vars.DominantHand.transform.position, Vars.DominantHand.transform.rotation);
                    if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                    {
                        gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
                    }

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

                    if ((bool)__instance.superGunSound)
                        Object.Instantiate(__instance.superGunSound);

                    if (__instance.gunVariation == 2 && (bool)__instance.twirlShotSound)
                        Object.Instantiate(__instance.twirlShotSound, __instance.transform.position, Quaternion.identity);
                    MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFireStrong, __instance.gameObject);
                    break;
                }
        }

        if (!__instance.altVersion)
            __instance.cylinder.DoTurn();

        __instance.anim.SetFloat("RandomChance", Random.Range(0f, 1f));
        if (shotType == 1) __instance.anim.SetTrigger("Shoot");
        else __instance.anim.SetTrigger("ChargeShoot");

        __instance.gunReady = false;

        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Revolver.ThrowCoin))]
    private static bool ThrowCoin(Revolver __instance)
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
}
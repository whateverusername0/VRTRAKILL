using HarmonyLib;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.VRAvatar;
using UnityEngine;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(RocketLauncher))] internal class PatchRocketLauncher
{
    [HarmonyPrefix] [HarmonyPatch(nameof(RocketLauncher.Shoot))]
    private static bool Shoot(RocketLauncher __instance)
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
        MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFire, __instance.gameObject);

        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(RocketLauncher.ShootCannonball))]
    private static bool ShootCannonball(RocketLauncher __instance)
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
        __instance.cbCharge = 0f;

        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(RocketLauncher.ShootNapalm))]
    private static bool ShootNapalm(RocketLauncher __instance)
    {
        __instance.anim.SetTrigger("Spray");
        __instance.napalmProjectileCooldown = 0.02f;
        MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel -= 0.015f;
        Rigidbody rigidbody = Object.Instantiate(__instance.napalmProjectile, Vars.DominantHand.transform.position + __instance.transform.forward, __instance.transform.rotation);
        if ((bool)MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
        {
            rigidbody.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
        }

        rigidbody.transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
        rigidbody.velocity = rigidbody.transform.forward * 150f;

        return false;
    }

    [HarmonyPostfix] [HarmonyPatch(nameof(RocketLauncher.Update))]
    private static void Update_AddIK(RocketLauncher __instance)
    {
        if (VRigController.Instance != null)
        {
            __instance.transform.position = VRigController.Instance.Rig.FeedbackerB.Forearm.position;
            __instance.transform.LookAt(Vars.DominantHand.transform/*VRigController.Instance.Rig.FeedbackerB.Hand.Root*/);
        }
    }
}

using HarmonyLib;
using Plugin.Data;
using UnityEngine;

namespace Plugin.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(Railcannon))] internal class PatchRailcannon
{
    [HarmonyPrefix] [HarmonyPatch(nameof(Railcannon.Shoot))]
    private static bool Shoot(Railcannon __instance)
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
        MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.GunFireStrong);

        return false;
    }
}
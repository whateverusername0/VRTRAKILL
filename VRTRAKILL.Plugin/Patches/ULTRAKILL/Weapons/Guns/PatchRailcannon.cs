using HarmonyLib;
using VRTRAKILL.Data;
using UnityEngine;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(Railcannon))] internal static class PatchRailcannon
{
    [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))]
    static void Transform(Railcannon __instance)
    {
        __instance.wpos.defaultPos += new Vector3(-.15f, .15f, -.175f);
        __instance.wpos.defaultScale = new(.25f, .25f, .25f);

        // add our own hand until hakita decides otherwise.
        Transform Hand = Object.Instantiate(Assets.HandPose_Railgun.transform);
        // Railcannon ******(Clone)/Railgun/Armature/Base
        Hand.SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);
        Hand.localPosition = Vector3.zero;

        Hand.GetChild(1).GetChild(0).localPosition = new(-.1f, -.325f, -.025f);
        Hand.GetChild(1).GetChild(0).localEulerAngles = new(30, 180, 0);
        Hand.GetChild(1).GetChild(0).localScale = new(350, 350, 350);
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Railcannon.Shoot))]
    static bool Shoot(Railcannon __instance)
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
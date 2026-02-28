using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(Nailgun))] internal static class PatchNailgun
{
    [HarmonyPostfix] [HarmonyPatch(nameof(Nailgun.Start))]
    static void Transform(Nailgun __instance)
    {
        if (__instance.altVersion) { WeaponTransform.ApplyTransform(ref __instance.wpos, new(-.165f, .2f, .065f), new(), new(.35f, .35f, .35f)); }
        else { WeaponTransform.ApplyTransform(ref __instance.wpos, new(-.165f, .1f, .045f), new(), new(.4f, .3275f, .4f)); }

        // add our own hand until hakita decides otherwise.
        // Nailgun ******(Clone)/Nailgun New New/Armature/Main
        // Sawblade Launcher ******(Clone)/Sawblade Launcher/Armature/Base
        if (__instance.altVersion)
        {
            Transform Hand = Object.Instantiate(Assets.HandPose_Sawblade.transform);
            Hand.SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);
            Hand.localPosition = Vector3.zero;

            Hand.GetChild(1).GetChild(0).localPosition = new(.001f, -.006f, .002f);
            Hand.GetChild(1).GetChild(0).localEulerAngles = new(0, 0, 0);
            Hand.GetChild(1).GetChild(0).localScale = new(3.5f, 3.5f, 3.5f);
        }
        else
        {
            Transform Hand = Object.Instantiate(Assets.HandPose_Nailgun.transform);
            Hand.SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);
            Hand.localPosition = Vector3.zero;

            Hand.localPosition = new(-.0008f, -.0053f, .0003f);
            Hand.localEulerAngles = new(0, 180, 0);
            Hand.GetChild(1).GetChild(0).localScale = new(.035f, .035f, .035f);
        }
    }

    // TODO
}
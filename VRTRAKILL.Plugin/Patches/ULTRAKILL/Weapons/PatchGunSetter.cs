using HarmonyLib;
using UnityEngine;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(GunSetter))] internal static class PatchGunSetter
{
    [HarmonyPostfix] [HarmonyPatch(nameof(GunSetter.ResetWeapons))]
    static void ResetWeapons(GunSetter __instance, bool firstTime = false)
    {
        __instance.transform.localPosition = Vector3.zero;
    }

    [HarmonyPostfix] [HarmonyPatch(nameof(GunSetter.Start))]
    static void EnableOffscreenRendering(GunSetter __instance)
    {
        foreach (SkinnedMeshRenderer SMR in __instance.GetComponentsInChildren<SkinnedMeshRenderer>())
            SMR.updateWhenOffscreen = true;
    }
}

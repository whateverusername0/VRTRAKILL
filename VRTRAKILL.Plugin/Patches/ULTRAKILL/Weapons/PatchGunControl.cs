using HarmonyLib;
using UnityEngine;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons;

[HarmonyPatch] internal static class PatchGunControl
{
    [HarmonyPostfix] [HarmonyPatch(nameof(GunControl.Start))]
    static void Start(GunControl __instance)
    {
        __instance.GetComponent<WalkingBob>().enabled = false;
        __instance.transform.localPosition = Vector3.zero;
    }
}
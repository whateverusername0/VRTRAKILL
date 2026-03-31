using HarmonyLib;
using VRTRAKILL.Data;
using Sandbox.Arm;
using System;
using UnityEngine;

namespace VRTRAKILL.Patches.Visual;

// removes hands from most weapons that have it.
[HarmonyPatch] internal static class PatchRemoveArms
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Revolver), nameof(Revolver.Start))]
    [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Start))]
    [HarmonyPatch(typeof(SandboxArm), nameof(SandboxArm.Awake))]
    [HarmonyPatch(typeof(FishingRodWeapon), nameof(FishingRodWeapon.Awake))]
    static void Discombobulate(MonoBehaviour __instance)
    {
        try
        {
            if (__instance is Shotgun)
            {
                __instance.gameObject.transform.Find("Shotgun_New")
                    .gameObject.transform.Find("RightArm")
                    .gameObject.SetActive(false);
            }
            //else __instance.gameObject.AddComponent<VRArmTransformer>();
        }
        catch (NullReferenceException) { GlobalVars.Log.LogWarning($"{__instance.gameObject.name} is null???"); }
    }
}

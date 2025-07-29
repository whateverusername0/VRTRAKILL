using HarmonyLib;
using Plugin.Systems;
using Plugin.Systems.Arms;
using Sandbox.Arm;
using System;

namespace Plugin.Patches.Visual;

// Removes hands from revolver & shotgun
[HarmonyPatch] internal class PatchRemoveArms
{
    [HarmonyPostfix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.Start))] static void RemoveRevolverArm(Revolver __instance)
    {
        try { __instance.gameObject.AddComponent<VRArmTransformer>(); }
        catch (NullReferenceException) { Vars.Log.LogWarning($"Revolver is null???"); }
    }
    [HarmonyPostfix] [HarmonyPatch(typeof(SandboxArm), nameof(SandboxArm.Awake))] static void RemoveSandboxArm(SandboxArm __instance)
    {
        try { __instance.gameObject.AddComponent<VRArmTransformer>(); }
        catch (NullReferenceException) { Vars.Log.LogWarning($"Sandbox arm is null???"); }
    }
    [HarmonyPostfix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Start))] static void RemoveShotgunArm(Shotgun __instance)
    {
        try
        {
            __instance.gameObject.transform.Find("Shotgun_New")
                .gameObject.transform.Find("RightArm")
                .gameObject.SetActive(false);
        }
        catch (NullReferenceException) { Vars.Log.LogWarning("Shotgun is null???"); }
    }
    [HarmonyPostfix] [HarmonyPatch(typeof(FishingRodWeapon), nameof(FishingRodWeapon.Awake))] static void RemoveFRArm(FishingRodWeapon __instance)
    {
        try { __instance.gameObject.AddComponent<VRArmTransformer>(); }
        catch (NullReferenceException) { Vars.Log.LogWarning("Fishing rod is null???"); }
    }
}

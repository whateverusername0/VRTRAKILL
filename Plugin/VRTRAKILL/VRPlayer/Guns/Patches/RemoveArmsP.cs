using HarmonyLib;
using Sandbox.Arm;
using System;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    // Removes hands from revolver & shotgun
    [HarmonyPatch] internal class RemoveArmsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.Start))] static void RemoveRevolverArm(Revolver __instance)
        {
            try { __instance.gameObject.AddComponent<Arms.ArmTransformer>(); }
            catch (NullReferenceException) { Vars.Log.LogWarning($"Revolver is null???"); }
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(SandboxArm), nameof(SandboxArm.Awake))] static void RemoveSandboxArm(SandboxArm __instance)
        {
            try { __instance.gameObject.AddComponent<Arms.ArmTransformer>(); }
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
            try
            {
                __instance.gameObject.transform.Find("MinosRevolver")
                    .gameObject.transform.Find("RightArm")
                    .gameObject.SetActive(false);
            }
            catch (NullReferenceException) { Vars.Log.LogWarning("Fishing rod is null???"); }
        }
    }
}

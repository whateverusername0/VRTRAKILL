using HarmonyLib;
using Sandbox.Arm;
using System;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    // Removes hands from revolver & shotgun
    [HarmonyPatch] internal class RemoveArmsP
    {
        // Hack to leave hand only (it just makes other hitngs too small that you're unable to see them)
        // ( Animator reverting my scaledefs is a meanie thing to do :C )

        [HarmonyPostfix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.Start))]
        static void RemoveRevolverArm(Revolver __instance)
        {
            try { __instance.gameObject.AddComponent<Arms.ArmRemover>(); }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning($"Revolver is null???"); }
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(SandboxArm), nameof(SandboxArm.Awake))] static void RemoveSandboxArm(SandboxArm __instance)
        {
            try
            {
                __instance.gameObject.AddComponent<Arms.ArmRemover>();
            }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning($"Sandbox arm is null???"); }
        }

        [HarmonyPostfix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Start))] static void RemoveShotgunArm(Shotgun __instance)
        {
            try
            {
                __instance.gameObject.transform.Find("Shotgun_New")
                .gameObject.transform.Find("RightArm")
                .gameObject.SetActive(false);
            }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning("Shotgun is null???"); }
        }
    }
}

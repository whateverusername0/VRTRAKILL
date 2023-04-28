using HarmonyLib;
using System;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    // Removes hands from revolver & shotgun
    [HarmonyPatch] static class RemoveArms
    {
        // Hack to leave hand only (it just makes other hitngs too small that you're unable to see them)
        // ( Animator reverting my scaledefs is a meanie thing to do :C )
        static Transform ArmT;

        [HarmonyPrefix] [HarmonyPostfix]
        [HarmonyPatch(typeof(Revolver), nameof(Revolver.LateUpdate))]
        static void RemoveRevolverArm(Revolver __instance)
        {
            try
            {
                ArmT = __instance.transform.GetChild(0);
                Arms.Feedbacker.Armature Arm = new Arms.Feedbacker.Armature(ArmT);
                Arm.RArmature.localScale = new Vector3(1, 1, 1);
                Arm.Hand.localScale = new Vector3(100, 100, 100);
            }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning($"Revolver is null???"); }
        }
        [HarmonyPrefix] [HarmonyPostfix]
        [HarmonyPatch(typeof(Sandbox.Arm.SandboxArm), nameof(Sandbox.Arm.SandboxArm.Update))]
        [HarmonyPatch(typeof(Sandbox.Arm.SandboxArm), nameof(Sandbox.Arm.SandboxArm.FixedUpdate))]
        static void RemoveSandboxArm(Sandbox.Arm.SandboxArm __instance)
        {
            try
            {
                ArmT = __instance.transform;
                Arms.Feedbacker.Armature Arm = new Arms.Feedbacker.Armature(ArmT);
                Arm.RArmature.localScale = new Vector3(1, 1, 1);
                Arm.Hand.localScale = new Vector3(100, 100, 100);
            }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning($"Sandbox arm is null???"); }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Start))]
        static void RemoveShotgunArm(Shotgun __instance)
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

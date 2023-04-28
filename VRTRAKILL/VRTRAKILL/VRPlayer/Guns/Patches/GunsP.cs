using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] static class GunsP
    {
        // Disables FRT (update method will no longer be called) and adds vrgunscontroller
        [HarmonyPrefix] [HarmonyPatch(typeof(RotateToFaceFrustumTarget), nameof(RotateToFaceFrustumTarget.Update))]
        static bool DisableFrustumRotation(RotateToFaceFrustumTarget __instance)
        {
            __instance.enabled = false;
            __instance.gameObject.AddComponent<VRGunsController>();
            return false;
        }

        [HarmonyPostfix] [HarmonyPatch(typeof(GunControl), nameof(GunControl.Start))] static void ChangeGCLayers(GunControl __instance)
        {
            __instance.transform.localPosition = Vector3.zero;
            Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }
        // Original method resets weapons' layers, so it changes it back to Default
        [HarmonyPostfix] [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.ResetWeapons))] static void ChangeGSLayers(GunSetter __instance)
        {
            __instance.transform.localPosition = Vector3.zero;
            Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }
    }
}
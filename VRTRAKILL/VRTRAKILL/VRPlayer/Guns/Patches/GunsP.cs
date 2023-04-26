using HarmonyLib;
using System;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] static class GunsP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(RotateToFaceFrustumTarget), nameof(RotateToFaceFrustumTarget.Update))]
        static bool DisableFrustumRotation(RotateToFaceFrustumTarget __instance)
        {
            __instance.enabled = false;
            __instance.gameObject.AddComponent<VRGunsController>();
            return false;
        }

        [HarmonyPostfix] [HarmonyPatch(typeof(GunControl), nameof(GunControl.Start))] static void ChangeAllGOLayers(GunControl __instance)
        {
            __instance.transform.localPosition = Vector3.zero;
            Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }
    }
}

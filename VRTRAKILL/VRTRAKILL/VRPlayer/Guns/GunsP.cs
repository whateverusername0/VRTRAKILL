using HarmonyLib;
using System;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns
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

        // Note to self:
        // revolver is too big, make it small (like size 0.25 or smh)
        // other guns are fine (leave at 0.5)
        // rocket launcher is too small (leave size untouched)

        // revolver is not aligned as other guns, align it manually
        // other guns can have their y val unchanged

        // arms animations are to be redone (esp red one)

        // guns are in priority!!
    }
}

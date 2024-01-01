﻿using HarmonyLib;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Guns.Patches
{
    [HarmonyPatch] internal sealed class GunsP
    {
        // Disables FRT (update method will no longer be called) and adds vrgunscontroller
        [HarmonyPrefix] [HarmonyPatch(typeof(RotateToFaceFrustumTarget), nameof(RotateToFaceFrustumTarget.Update))]
        static bool DisableFrustumRotation(RotateToFaceFrustumTarget __instance)
        {
            __instance.enabled = false;
            __instance.GetComponent<WalkingBob>().enabled = false;
            __instance.gameObject.AddComponent<VRGunsController>();
            return false;
        }

        // idk if this is needed, but i'm not removing it
        [HarmonyPostfix] [HarmonyPatch(typeof(GunControl), nameof(GunControl.Start))] static void RLPGC(GunControl __instance)
        { __instance.transform.localPosition = Vector3.zero; }
        [HarmonyPostfix] [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.ResetWeapons))] static void RLPGS(GunSetter __instance, bool firstTime = false)
        { __instance.transform.localPosition = Vector3.zero; }
        [HarmonyPostfix] [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.Start))] static void MakeThemUndisappear(GunSetter __instance)
        {
            foreach (SkinnedMeshRenderer SMR in __instance.GetComponentsInChildren<SkinnedMeshRenderer>())
                SMR.updateWhenOffscreen = true;
        }
    }
}
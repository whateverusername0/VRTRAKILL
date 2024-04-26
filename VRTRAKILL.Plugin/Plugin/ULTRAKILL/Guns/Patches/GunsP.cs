using HarmonyLib;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Guns.Patches
{
    [HarmonyPatch] internal sealed class GunsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(GunControl), nameof(GunControl.Start))] static void RLPGC(GunControl __instance)
        {
            __instance.gameObject.AddComponent<VRGunsController>();
            __instance.GetComponent<WalkingBob>().enabled = false;
            __instance.transform.localPosition = Vector3.zero;
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.ResetWeapons))] static void RLPGS(GunSetter __instance, bool firstTime = false)
        { __instance.transform.localPosition = Vector3.zero; }
        [HarmonyPostfix] [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.Start))] static void MakeThemUndisappear(GunSetter __instance)
        {
            foreach (SkinnedMeshRenderer SMR in __instance.GetComponentsInChildren<SkinnedMeshRenderer>())
                SMR.updateWhenOffscreen = true;
        }
    }
}
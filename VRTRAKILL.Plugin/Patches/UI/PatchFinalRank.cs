using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems.UI;

namespace VRTRAKILL.Patches.UI;

[HarmonyPatch(typeof(FinalRank))] internal static class PatchFinalRank
{
    [HarmonyPostfix] [HarmonyPatch(nameof(FinalRank.Start))]
    static void Start(FinalRank __instance)
    {
        // makes it visible and larger.
        VRUIConverter.ConvertCanvas(__instance.transform.parent.GetComponent<Canvas>(), force: true);
        __instance.transform.localPosition = Vector3.zero;
        __instance.transform.localScale = new Vector3(16, 8, 8);
    }
}
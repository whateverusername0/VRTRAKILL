using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems.UI;

namespace VRTRAKILL.Patches.UI;

[HarmonyPatch(typeof(Crosshair))] internal static class PatchCrosshair
{
    [HarmonyPostfix] [HarmonyPatch(nameof(Crosshair.Start))]
    static void Start(Crosshair __instance)
    {
        // reparent it to world space
        var go = new GameObject("Crosshair Container");
        go.gameObject.AddComponent<VRCrosshairController>();
        __instance.transform.SetParent(go.transform, false);

        // make it visible
        Canvas C = __instance.gameObject.AddComponent<Canvas>();
        VRUIConverter.ConvertCanvas(C, addComponent: false);

        __instance.transform.localScale /= 2;
        __instance.transform.localEulerAngles = Vector3.zero;
    }
}

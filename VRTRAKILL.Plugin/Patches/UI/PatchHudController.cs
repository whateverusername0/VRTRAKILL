using HarmonyLib;
using UnityEngine;

namespace VRTRAKILL.Patches.UI;

[HarmonyPatch(typeof(HudController))] internal static class PatchHudController
{
    // using shitcode magic again to make hud look good.
    [HarmonyPostfix] [HarmonyPatch(nameof(HudController.Start))]
    static void ReparentHUD(HudController __instance)
    {
        if (__instance.altHud) return;
        else
        {
            __instance.transform.parent = CanvasController.Instance.transform;
            __instance.transform.localPosition = Vector3.zero;
            __instance.transform.localEulerAngles = Vector3.zero;
        }
    }
}

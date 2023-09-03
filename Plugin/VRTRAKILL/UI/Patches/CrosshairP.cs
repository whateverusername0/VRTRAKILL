using HarmonyLib;
using Plugin.Util;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch(typeof(Crosshair))] internal sealed class CrosshairP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(Crosshair.Start))] static void SetCrosshair(Crosshair __instance)
        {
            Canvas C = __instance.gameObject.AddComponent<Canvas>();
            UIConverter.ConvertCanvas(C, DontAddComponent: true);

            __instance.transform.localScale /= 2;
            __instance.transform.localEulerAngles = Vector3.zero;
            __instance.gameObject.AddComponent<CrosshairController>();
        }
        [HarmonyPostfix] [HarmonyPatch(nameof(Crosshair.Start))] static void Reparent(Crosshair __instance)
        {
            __instance.transform.parent = null;
        }
    }
}

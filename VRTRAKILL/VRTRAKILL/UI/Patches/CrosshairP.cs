using HarmonyLib;
using Plugin.Helpers;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch(typeof(Crosshair))] internal class CrosshairP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(Crosshair.Start))] static void SetCrosshair(Crosshair __instance)
        {
            // set controller
            __instance.transform.parent = Vars.DominantHand.transform;

            Canvas C = __instance.gameObject.AddComponent<Canvas>();
            C.worldCamera = Vars.UICamera;
            C.renderMode = RenderMode.WorldSpace;
            __instance.gameObject.layer = (int)Vars.Layers.UI;

            if (__instance.gameObject.HasComponent<UICanvas>())
                Object.Destroy(__instance.gameObject.GetComponent<UICanvas>());

            __instance.transform.localScale /= 2; // to fit in player's size (shitcode, but it works!!)
            __instance.transform.localPosition += new Vector3(0, 0, .05f);
            __instance.transform.localEulerAngles = Vector3.zero;
            //__instance.gameObject.AddComponent<CrosshairController>();
        }
    }
}

using HarmonyLib;
using Plugin.Helpers;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch(typeof(Crosshair))] internal static class CrosshairP
    {
        public static Crosshair CrosshairRef;
        [HarmonyPrefix] [HarmonyPatch(nameof(Crosshair.Start))] static void SetCrosshair(Crosshair __instance)
        {
            CrosshairRef = __instance;

            // set controller
            if (Vars.Config.VRInputSettings.Hands.LeftHandMode)
                __instance.transform.parent = Vars.LeftController.transform;
            else __instance.transform.parent = Vars.RightController.transform;

            Canvas C = __instance.gameObject.AddComponent<Canvas>();
            C.worldCamera = Vars.VRUICamera;
            C.renderMode = RenderMode.WorldSpace;
            __instance.gameObject.layer = 0;

            if (__instance.gameObject.HasComponent<UICanvas>())
                Object.Destroy(__instance.gameObject.GetComponent<UICanvas>());
        }
    }
}

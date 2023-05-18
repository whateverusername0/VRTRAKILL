using HarmonyLib;
using Plugin.Helpers;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch(typeof(Crosshair))] internal static class CrosshairP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(Crosshair.Start))] static void SetCrosshair(Crosshair __instance)
        {
            // set controller
            if (Vars.Config.VRInputSettings.Hands.LeftHandMode)
                __instance.transform.parent = Vars.LeftController.transform;
            else __instance.transform.parent = Vars.RightController.transform;

            Canvas C = __instance.gameObject.AddComponent<Canvas>();
            C.worldCamera = Vars.VRUICamera;
            C.renderMode = RenderMode.WorldSpace;
            __instance.gameObject.layer = (int)Vars.Layers.Default;

            if (__instance.gameObject.HasComponent<UICanvas>())
                Object.Destroy(__instance.gameObject.GetComponent<UICanvas>());

            __instance.transform.localPosition += new Vector3(0, 0, .05f);
            __instance.transform.localEulerAngles = Vector3.zero;
            __instance.gameObject.AddComponent<CrossHair.CrosshairController>();
        }
    }
}

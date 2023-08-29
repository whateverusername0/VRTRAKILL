using HarmonyLib;
using Plugin.Util;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch(typeof(Crosshair))] internal sealed class CrosshairP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(Crosshair.Start))] static void SetCrosshair(Crosshair __instance)
        {
            __instance.transform.parent = VRPlayer.Controllers.GunController.Instance.GunOffset.transform;

            Canvas C = __instance.gameObject.AddComponent<Canvas>();
            C.worldCamera = Vars.UICamera;
            C.renderMode = RenderMode.WorldSpace;
            __instance.gameObject.layer = (int)Layers.UI;

            if (__instance.gameObject.HasComponent<UICanvas>())
                Object.Destroy(__instance.gameObject.GetComponent<UICanvas>());

            __instance.transform.localScale /= 2;
            //__instance.transform.localPosition += new Vector3(0, 0, 0);
            __instance.transform.localEulerAngles = Vector3.zero;
            __instance.gameObject.AddComponent<CrosshairController>();
        }
    }
}

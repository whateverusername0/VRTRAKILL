using HarmonyLib;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.UI.Patches
{
    [HarmonyPatch] internal class CrosshairP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(Crosshair), nameof(Crosshair.Start))] static void SetCrosshair(Crosshair __instance)
        {
            Canvas C = __instance.gameObject.AddComponent<Canvas>();
            UIConverter.ConvertCanvas(C, DontAddComponent: true);

            __instance.transform.localScale /= 2;
            __instance.transform.localEulerAngles = Vector3.zero;
            __instance.gameObject.AddComponent<CrosshairController>();
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(Crosshair), nameof(Crosshair.Start))] static void Reparent(Crosshair __instance)
        {
            __instance.transform.parent = null;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.HideShit))] static bool FixShit(StatsManager __instance)
        {
            if (__instance.shud == null) __instance.shud = MonoSingleton<StyleHUD>.Instance;

            __instance.shud.ComboOver();
            if (__instance.gunc == null) __instance.gunc = MonoSingleton<GunControl>.Instance;

            __instance.gunc.NoWeapon();
            if ((bool)__instance.crosshair)
                __instance.crosshair.transform.gameObject.SetActive(value: false);

            HudController.Instance.gunCanvas.SetActive(value: false);
            return false;
        }
    }
}

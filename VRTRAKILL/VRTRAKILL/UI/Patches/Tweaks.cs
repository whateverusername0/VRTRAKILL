using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] static class Tweaks
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(CanvasController), nameof(CanvasController.Awake))] static void ResizeCanvases(CanvasController __instance)
        {
            // stretches screen effects goatse style so it's not a fucking square in the middle of the hud
            string[] ScreenEffects =
            {
                "HurtScreen", "BlackScreen", "ParryFlash",
                "UnderwaterOverlay", "Black", "White" // leviathan specific
            };
            foreach (string ScreenEffect in ScreenEffects)
                try
                {
                    Transform T = __instance.gameObject.transform.Find(ScreenEffect);
                    T.transform.localScale *= 5;
                    for (int i = 0; i < T.childCount; i++)
                        T.GetChild(i).transform.localScale /= 5;
                }
                catch { continue; }

            // prime bosses specific
            try { Object.FindObjectOfType<FlashImage>().transform.localScale *= 5; } catch {}

            // disable useless stuffs
            string[] ScreenEffectsToDisable =
            {
                "PowerUpVignette",
            };
            foreach (string ScreenEffectToDisable in ScreenEffectsToDisable)
                try { __instance.gameObject.transform.Find(ScreenEffectToDisable).GetComponent<Image>().enabled = false; } catch { continue; }

            // Relayer stupid skybox in minos corpse level
            try { GameObject.Find("CityFromAbove").layer = 0; } catch {}
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(Crosshair), nameof(Crosshair.Start))] static void SetCrosshair(Crosshair __instance)
        {
            __instance.transform.parent = null;

            // set controller
            CrosshairController CHC = __instance.gameObject.AddComponent<CrosshairController>();
            if (Vars.Config.VRInputSettings.Hands.LeftHandMode) CHC.Hand = 0; else CHC.Hand = 1;

            Canvas C = __instance.gameObject.AddComponent<Canvas>();
            C.worldCamera = Vars.VRUICamera;
            C.renderMode = RenderMode.WorldSpace;

            if (__instance.gameObject.HasComponent<UICanvas>())
                Object.Destroy(__instance.gameObject.GetComponent<UICanvas>());
        }
    }
}

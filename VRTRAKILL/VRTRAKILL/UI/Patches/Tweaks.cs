using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] static class Tweaks
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(CanvasController), "Awake")] static void ResizeCanvases(CanvasController __instance)
        {
            // Stretches screen effects goatse style so it's not a fucking square in the middle of the hud
            string[] ScreenEffects =
            {
                "HurtScreen", "BlackScreen", "ParryFlash",
                "UnderwaterOverlay",
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

            try { Object.FindObjectOfType<FlashImage>().transform.localScale *= 5; } catch {}

            // Disable useless stuffs
            string[] ScreenEffectsToDisable =
            {
                "PowerUpVignette",
            };
            foreach (string ScreenEffectToDisable in ScreenEffectsToDisable)
                try { __instance.gameObject.transform.Find(ScreenEffectToDisable).GetComponent<Image>().enabled = false; } catch { continue; }
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(Crosshair), nameof(Crosshair.Start))] static void SetCrosshair(Crosshair __instance)
        {
            if (Vars.Config.VRSettings.EnableDefaultCrosshair == false)
                __instance.enabled = false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(FinalRank), nameof(FinalRank.Start))] static void TweakRankScreenTransform(FinalRank __instance)
        {
            // Placeholder
        }
    }
}

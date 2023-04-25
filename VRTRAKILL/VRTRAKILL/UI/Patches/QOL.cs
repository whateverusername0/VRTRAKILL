using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] static class QOL
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
        [HarmonyPrefix] [HarmonyPatch(typeof(FinalRank), nameof(FinalRank.Start))] static void TweakRankScreenTransform(FinalRank __instance)
        {
            __instance.transform.parent.localScale = new Vector3(0.003f, 0.002f, 0.001f);
            __instance.transform.parent.localPosition = new Vector3(0f, -0.2f, 2.2f);
        }
    }
}

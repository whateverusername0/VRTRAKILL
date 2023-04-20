using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] static class QOL
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(CanvasController), "Awake")] static void TweakUI(CanvasController __instance)
        {
            // Stretches screen effects goatse style so it's not a fucking square in the middle of the hud
            string[] ScreenEffects =
            {
                "HurtScreen", "BlackScreen", "ParryFlash",
                "UnderwaterOverlay",
            };
            foreach (string ScreenEffect in ScreenEffects)
                try { __instance.gameObject.transform.Find(ScreenEffect).transform.localScale *= 5; } catch { continue; }

            Object.FindObjectOfType<FlashImage>().transform.localScale *= 5;

            // Disable useless stuffs
            string[] ScreenEffectsToDisable =
            {
                "PowerUpVignette",
            };
            foreach (string ScreenEffectToDisable in ScreenEffectsToDisable)
                try { __instance.gameObject.transform.Find(ScreenEffectToDisable).GetComponent<Image>().enabled = false; } catch { continue; }
        }
    }
}

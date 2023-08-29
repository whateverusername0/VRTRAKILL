using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] internal sealed class CanvasControllerP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(CanvasController), nameof(CanvasController.Awake))] static void ResizeCanvases(CanvasController __instance)
        {
            // stretches screen effects goatse style so it's not a fucking square in the middle of the hud
            string[] ScreenEffects =
            {
                "HurtScreen", "BlackScreen", "ParryFlash",
                "UnderwaterOverlay", "Black", "White"
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

            // Deploy the "GTFOTW"
            GameObject UI_GTFOTW = Object.Instantiate<GameObject>(Assets.Vars.UI_GTFOTW, Vector3.zero, Quaternion.identity, __instance.transform);
            UI_GTFOTW.transform.SetSiblingIndex(0);
            GTFOTW GTFOTW = UI_GTFOTW.AddComponent<GTFOTW>();
            GTFOTW.DetectorTransform = Vars.MainCamera.transform;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using Plugin.Util;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] internal class HUDOptionsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(HUDOptions), nameof(HUDOptions.Start))] static void ResizeCanvases(HUDOptions __instance)
        {
            // Stretches screen effects so it's not a small square in the middle of the hud
            string[] ScreenEffects =
            {
                "HurtScreen", "BlackScreen", "ParryFlash",
                "UnderwaterOverlay", "Black", "White"
            };
            foreach (string ScreenEffect in ScreenEffects)
                try
                {
                    Transform T = __instance.gameObject.transform.Find(ScreenEffect);
                    T.transform.localScale *= 10;
                    for (int i = 0; i < T.childCount; i++)
                        T.GetChild(i).transform.localScale /= 10;
                }
                catch { continue; }

            // prime bosses specific
            try { Object.FindObjectOfType<FlashImage>().transform.localScale *= 10; } catch {}

            // disable unnecessary stuff (for now)
            string[] ScreenEffectsToDisable =
            {
                "PowerUpVignette",
            };
            foreach (string ScreenEffectToDisable in ScreenEffectsToDisable)
                try { __instance.gameObject.transform.Find(ScreenEffectToDisable).GetComponent<Image>().enabled = false; } catch { continue; }

            // Relayer skybox in 2-4
            try { GameObject.Find("CityFromAbove").layer = 0; } catch {}
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(HUDOptions), nameof(HUDOptions.Start))] static void DeployGTFOTW(HUDOptions __instance)
        {
            GameObject UI_GTFOTW = Object.Instantiate(Assets.UI_GTFOTW, Vector3.zero, Quaternion.identity, __instance.transform);

            Assets.UI_GTFOTW.transform.localScale = Vector3.zero;

            // Sets it's index to 0 so that it's above everything else
            UI_GTFOTW.transform.SetSiblingIndex(0);
            UI_GTFOTW.transform.localScale = Vector3.one;
            UI_GTFOTW.transform.localPosition = Vector3.zero;

            UIConverter.ConvertCanvas(UI_GTFOTW.GetComponent<Canvas>());
            Util.Unity.RecursiveChangeLayer(UI_GTFOTW, (int)Layers.UI);

            GTFOTW GTFOTW = UI_GTFOTW.AddComponent<GTFOTW>();
            GTFOTW.DetectorTransform = Vars.MainCamera.transform;
        }

        [HarmonyPostfix] [HarmonyPatch(typeof(ScreenZone), nameof(ScreenZone.OnTriggerEnter))] static void ConvertThing()
        {
            foreach (Canvas C in Resources.FindObjectsOfTypeAll(typeof(Canvas)))
                if (!C.gameObject.HasComponent<UICanvas>())
                    UIConverter.RecursiveConvertCanvas();
        }
    }
}

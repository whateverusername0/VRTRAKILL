using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] internal class HUDOptionsP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(HUDOptions), nameof(HUDOptions.Start))] static void ResizeCanvases(HUDOptions __instance)
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
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(HUDOptions), nameof(HUDOptions.Start))] static void DeployGTFOTW(HUDOptions __instance)
        {
            GameObject UI_GTFOTW = Object.Instantiate(Assets.Vars.UI_GTFOTW, Vector3.zero, Quaternion.identity, __instance.transform);

            Assets.Vars.UI_GTFOTW.transform.localScale = Vector3.zero;

            UI_GTFOTW.transform.SetSiblingIndex(0);
            UI_GTFOTW.transform.localScale = Vector3.one;
            UI_GTFOTW.transform.localPosition = Vector3.zero;

            UIConverter.ConvertCanvas(UI_GTFOTW.GetComponent<Canvas>());
            Util.Misc.RecursiveChangeLayer(UI_GTFOTW, (int)Layers.UI);

            GTFOTW GTFOTW = UI_GTFOTW.AddComponent<GTFOTW>();
            GTFOTW.DetectorTransform = Vars.MainCamera.transform;

            
        }
    }
}

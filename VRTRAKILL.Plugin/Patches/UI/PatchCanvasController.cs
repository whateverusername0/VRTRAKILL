using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using VRTRAKILL.Utilities;
using VRTRAKILL.Data;
using VRTRAKILL.Systems;
using VRTRAKILL.Systems.UI;

namespace VRTRAKILL.Patches.UI;

[HarmonyPatch(typeof(CanvasController))] internal static class PatchCanvasController
{
    [HarmonyPostfix] [HarmonyPatch(nameof(CanvasController.Awake))]
    static void Awake(CanvasController __instance)
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

    // deploy the "get the fuck out of the wall" feature
    [HarmonyPostfix] [HarmonyPatch(nameof(CanvasController.Awake))]
    static void DeployNoclipPrevention(CanvasController __instance)
    {
        GameObject UI_GTFOTW = Object.Instantiate(Assets.UI_GTFOTW, Vector3.zero, Quaternion.identity, __instance.transform);

        Assets.UI_GTFOTW.transform.localScale = Vector3.zero;

        // Sets it's index to 0 so that it's above everything else
        UI_GTFOTW.transform.SetSiblingIndex(0);
        UI_GTFOTW.transform.localScale = Vector3.one;
        UI_GTFOTW.transform.localPosition = Vector3.zero;

        VRUIConverter.ConvertCanvas(UI_GTFOTW.GetComponent<Canvas>());
        UnityExtensions.RecursiveChangeLayer(UI_GTFOTW, (int)Layers.UI);

        VRNoclipPreventionSystem GTFOTW = UI_GTFOTW.AddComponent<VRNoclipPreventionSystem>();
        GTFOTW.Pivot = Vars.MainCamera.transform;
    }
}

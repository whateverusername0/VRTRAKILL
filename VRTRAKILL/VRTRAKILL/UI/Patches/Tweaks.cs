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
            // I decided to just leave it as it is because I lost my sanity figuring out
            // why the fuck would it not work in the UI layer. :(
            __instance.transform.localPosition = new Vector3(__instance.transform.localPosition.x,
                                                             __instance.transform.localPosition.y,
                                                             __instance.transform.localPosition.z + Vars.Config.VRSettings.VRUI.CrosshairDistance);

            // set controller
            if (Vars.Config.VRInputSettings.Hands.LeftHandMode)
                __instance.transform.parent = Vars.LeftController.transform;
            else __instance.transform.parent = Vars.RightController.transform;

            Canvas C = __instance.gameObject.AddComponent<Canvas>();
            C.worldCamera = Vars.VRUICamera;
            C.renderMode = RenderMode.WorldSpace;
            __instance.gameObject.layer = 0;

            if (__instance.gameObject.HasComponent<UICanvas>())
                Object.Destroy(__instance.gameObject.GetComponent<UICanvas>());
        }
    }
}

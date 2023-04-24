using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch(typeof(HudController))] internal class HUDControllerP
    {
        // for some reason it starts later than other ui elements so it needs a golden fucking ticket to get converted
        [HarmonyPostfix] [HarmonyPatch(nameof(HudController.Start))] static void ConvertCanvases(HudController __instance)
        {
            foreach (Canvas C in GameObject.FindObjectsOfType<Canvas>())
                if (!Helpers.Misc.HasComponent<UICanvas>(C.gameObject))
                    UICanvas.ConvertCanvas(C);
        }
    }
}

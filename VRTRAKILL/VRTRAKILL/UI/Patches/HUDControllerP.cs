using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch(typeof(HudController))] internal class HUDControllerP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(HudController.Start))] static void ReparentHUD()
        {

        }
    }
}

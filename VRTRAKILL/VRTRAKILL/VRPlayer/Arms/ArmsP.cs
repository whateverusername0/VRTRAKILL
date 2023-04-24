using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    [HarmonyPatch(typeof(FistControl))] internal class ArmsP
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(FistControl.Start))] static void ConvertArms(FistControl __instance)
        {
            __instance.gameObject.AddComponent<VRArmsController>();
            Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }
    }
}

using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    [HarmonyPatch(typeof(FistControl))] internal class ArmsP
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(FistControl.Start))]
        [HarmonyPatch(nameof(FistControl.ResetFists))]
        static void ConvertArms(FistControl __instance)
        {
            __instance.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            __instance.transform.localPosition = new Vector3(0, 0, .25f);
            Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }
    }
}

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
            __instance.transform.localPosition = Vector3.zero;
            //__instance.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }
    }
}

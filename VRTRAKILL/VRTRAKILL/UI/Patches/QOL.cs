using System;
using UnityEngine.UI;
using HarmonyLib;

namespace Plugin.VRTRAKILL.UI.Patches
{
    // no more ui eye sore
    [HarmonyPatch] internal class QOL
    {
        // removes black color on death screen, hurt red color & parry flash
        [HarmonyPrefix] [HarmonyPatch(typeof(CanvasController), "Awake")] static void RemoveEyeSore(CanvasController __instance)
        {
            __instance.gameObject.transform.Find("HurtScreen").GetComponent<Image>().enabled = false;
            __instance.gameObject.transform.Find("BlackScreen").GetComponent<Image>().enabled = false;
            __instance.gameObject.transform.Find("ParryFlash").GetComponent<Image>().enabled = false;
        }
    }
}

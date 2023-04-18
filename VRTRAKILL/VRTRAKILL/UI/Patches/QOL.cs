using UnityEngine.UI;
using HarmonyLib;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] internal class QOL
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(CanvasController), "Awake")] static void RemoveEyeSore(CanvasController __instance)
        {
            string[] EyeSore = { "HurtScreen", "BlackScreen", "ParryFlash" }; // to be changed to a new ui system
            foreach (string Sore in EyeSore) __instance.gameObject.transform.Find(Sore).GetComponent<Image>().enabled = false;
        }
    }
}

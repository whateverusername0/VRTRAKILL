using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch(typeof(HUDPos))] internal class HUD
    {
        [HarmonyPrefix] [HarmonyPatch("Start")] static void FixPos(HUDPos __instance, ref Vector3 ___defaultPos)
        {
            // HuskVR you pretty
            if (__instance.gameObject.name == "GunCanvas")
            {
                __instance.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                ___defaultPos = new Vector3(-0.75f, -0.4f, 1.6f);
            }
            if (__instance.gameObject.name == "StyleCanvas")
            {
                __instance.transform.localScale = new Vector3(0.002f, 0.002f, 0.001f);
                ___defaultPos = new Vector3(1.3f, 0.2f, 1.6f);
            }
        }
    }
}

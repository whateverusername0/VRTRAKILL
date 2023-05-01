using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.HUD
{
    [HarmonyPatch] internal class TransformP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(HUDPos), nameof(HUDPos.Start))] static void ReparentHUD(HUDPos __instance)
        {
            __instance.transform.parent = HUDOptions.Instance.transform;

            // GunCanvas forces itself no matter what type of hud you set, so we have two options:
            // Remove Standard HUD or Classic HU-
            if (__instance.gameObject.name == "GunCanvas") __instance.GetComponent<Canvas>().enabled = false;

            UIConverter.ConvertCanvas(__instance.GetComponent<Canvas>(), Force: true, DontAddComponent: true);

            __instance.defaultPos = new Vector3(396, 48, -286);
            __instance.defaultRot = new Vector3(0, 64, 0);
            __instance.transform.localScale = new Vector3(.65f, .65f, .65f);
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(FinalRank), nameof(FinalRank.Start))] static void ReparentFR(FinalRank __instance)
        {
            __instance.transform.parent.transform.parent = HUDOptions.Instance.transform;

            UIConverter.ConvertCanvas(__instance.transform.parent.GetComponent<Canvas>(), Force: true);

            __instance.transform.localScale = new Vector3(16, 16, 16);
        }
    }
}

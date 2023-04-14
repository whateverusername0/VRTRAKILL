using System;
using HarmonyLib;

namespace Plugin.VRTRAKILL.Guns.Patches
{
    // Removes hands from revolver & shotgun
    [HarmonyPatch] internal class RemoveHands
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(Revolver), "Start")] static void RemoveRevolverHand(Revolver __instance)
        {
            try
            {
                __instance.gameObject.transform.Find("Revolver")
                .gameObject.transform.Find("RightArm")
                .gameObject.SetActive(false);
            }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning("Revolver / Marksman Revolver is null"); }
            try
            {
                __instance.gameObject.transform.Find("MinosRevolver")
                .gameObject.transform.Find("RightArm")
                .gameObject.SetActive(false);
            }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning("MinosRevolver is null"); }
            try
            {
                __instance.gameObject.transform.Find("PistolNew")
                .gameObject.transform.Find("RightArm")
                .gameObject.SetActive(false);
            }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning("Marksman is null"); }
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(Shotgun), "Start")] static void RemoveShotgunHand(Shotgun __instance)
        {
            try
            {
                __instance.gameObject.transform.Find("Shotgun_New")
                .gameObject.transform.Find("RightArm")
                .gameObject.SetActive(false);
            }
            catch (NullReferenceException) { Plugin.PLogger.LogWarning("Shotgun is null"); }
        }
    }
}

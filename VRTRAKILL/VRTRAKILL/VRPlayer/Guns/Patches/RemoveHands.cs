using System;
using HarmonyLib;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    // Removes hands from revolver & shotgun
    [HarmonyPatch] static class RemoveHands
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.Start))] static void RemoveRevolverHand(Revolver __instance)
        {
            string[] GameObjects = { "Revolver", "MinosRevolver", "PistolNew" };
            foreach (string GO in GameObjects)
                try
                {
                    __instance.gameObject.transform.Find(GO)
                    .gameObject.transform.Find("RightArm")
                    .gameObject.SetActive(false);
                }
                catch (NullReferenceException) { Plugin.PLogger.LogWarning($"{GO} is null"); }
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Start))] static void RemoveShotgunHand(Shotgun __instance)
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
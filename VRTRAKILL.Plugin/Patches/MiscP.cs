using HarmonyLib;
using UnityEngine;

namespace VRBasePlugin.Patches
{
    // contains necessary patches (and not so necessary)
    [HarmonyPatch] internal sealed class MiscP
    {
        // discord :)
        [HarmonyPrefix] [HarmonyPatch(typeof(DiscordController), nameof(DiscordController.SendActivity))]
        static void SendActivity(DiscordController __instance)
        => __instance.cachedActivity.State = "Playing in VR via VRTRAKILL";
    }
}

using HarmonyLib;

namespace Plugin.Patches.Misc;

// contains necessary patches (and not so necessary)
[HarmonyPatch] internal sealed class PatchDiscord
{
    // discord :)
    [HarmonyPrefix] [HarmonyPatch(typeof(DiscordController), nameof(DiscordController.SendActivity))]
    static void SendActivity(DiscordController __instance)
    => __instance.cachedActivity.State = "Playing in VR via VRTRAKILL";
}

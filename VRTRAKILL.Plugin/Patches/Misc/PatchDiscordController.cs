using HarmonyLib;

namespace VRTRAKILL.Patches.Misc;

// contains necessary patches (and not so necessary)
[HarmonyPatch(typeof(DiscordController))] internal static class PatchDiscordController
{
    // discord :)
    [HarmonyPrefix]
    [HarmonyPatch(nameof(DiscordController.SendActivity))]
    static void SendActivity(DiscordController __instance)
    {
        __instance.cachedActivity.State = "Playing in VR using VRTRAKILL.";
    }
}

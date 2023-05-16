using HarmonyLib;

namespace Plugin.VRTRAKILL.Patches
{
    [HarmonyPatch(typeof(DiscordController))] internal class DiscordIntegration
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(DiscordController.SendActivity))] static void SendActivity(DiscordController __instance)
        {
            // that's it, lol
            __instance.cachedActivity.Name = "VRTRAKILL";
        }
    }
}

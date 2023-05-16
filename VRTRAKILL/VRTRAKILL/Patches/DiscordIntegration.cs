using HarmonyLib;

namespace Plugin.VRTRAKILL.Patches
{
    [HarmonyPatch(typeof(DiscordController))] internal class DiscordIntegration
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(DiscordController.Awake))] public void SendActivity(DiscordController __instance)
        {
            // that's it, lol
            __instance.cachedActivity.Name = "VRTRAKILL";
        }
    }
}

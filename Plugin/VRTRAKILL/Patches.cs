using HarmonyLib;

namespace Plugin.VRTRAKILL
{
    [HarmonyPatch] internal class Patches
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(DiscordController), nameof(DiscordController.SendActivity))]
        static void SendActivity(DiscordController __instance)
        => __instance.cachedActivity.State += " in VR";
    }
}

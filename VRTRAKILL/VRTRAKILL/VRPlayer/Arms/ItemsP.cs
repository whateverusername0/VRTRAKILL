using HarmonyLib;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    [HarmonyPatch] internal class ItemsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Skull), nameof(Skull.PunchWith))] static void ReLayerSkull(Skull __instance)
        {
            //Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }
    }
}

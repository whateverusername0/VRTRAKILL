using HarmonyLib;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch(typeof(GunControl))] internal class ChangeGunsLayer
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(GunControl.Start))] static void ChangeAllGOLayers(GunControl __instance)
        {
            Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }
    }
}

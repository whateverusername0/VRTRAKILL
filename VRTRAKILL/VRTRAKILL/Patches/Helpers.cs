using HarmonyLib;


namespace Plugin.VRTRAKILL.Patches
{
    internal class Helpers
    {
        // ty huskvr you pretty
        // might remove later because it's unnecessary
        [HarmonyPrefix] [HarmonyPatch(typeof(PostProcessV2_Handler), "SetupRTs")] static bool DontUpdateRTsIfCamNull(PostProcessV2_Handler __instance)
        {
            if (Traverse.Create(__instance).Field("mainCam").GetValue() != null
                && __instance.hudCam != null) return true;
            return false;
        }
    }
}

using HarmonyLib;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch(typeof(RotateToFaceFrustumTarget))] static class GunsP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(RotateToFaceFrustumTarget.Update))] static bool DisableRotation(RotateToFaceFrustumTarget __instance)
        {
            __instance.enabled = false;
            __instance.gameObject.AddComponent<VRGunsController>();
            return false;
        }
    }
}

using HarmonyLib;

namespace Plugin.VRTRAKILL.VRPlayer.Guns
{
    [HarmonyPatch(typeof(RotateToFaceFrustumTarget))] internal class GunsP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(RotateToFaceFrustumTarget.Update))] static bool ConvertGuns(RotateToFaceFrustumTarget __instance)
        {
            __instance.enabled = false;
            __instance.gameObject.AddComponent<VRGunsController>();
            return false;
        }
    }
}

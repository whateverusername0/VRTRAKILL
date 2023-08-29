using HarmonyLib;

namespace Plugin.Util.Patches
{
    [HarmonyPatch] internal class HelpersP
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CameraController), nameof(CameraController.Update))]
        [HarmonyPatch(typeof(CameraFrustumTargeter), nameof(CameraFrustumTargeter.Update))]
        [HarmonyPatch(typeof(CameraFrustumTargeter), nameof(CameraFrustumTargeter.LateUpdate))]
        static bool DoNothing()
        {
            // do nothing
            return false;
        }
    }
}

using HarmonyLib;

namespace VRBasePlugin.ULTRAKILL.VRPlayer
{
    [HarmonyPatch] public class Patches
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void AddVRPC(NewMovement __instance)
        {
            __instance.gameObject.AddComponent<VRPlayerController>();
        }
    }
}

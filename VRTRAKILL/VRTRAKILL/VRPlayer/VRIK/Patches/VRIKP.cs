using HarmonyLib;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK.Patches
{
    internal class VRIKP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void AttachVRIK(NewMovement __instance)
        {
            if (!__instance.gameObject.HasComponent<VRIKController>())
            {
                __instance.gameObject.AddComponent<VRIKController>();
                MonoSingleton<VRIKController>.Instance.Rig = MetaRig.CreateV1Preset(__instance.gameObject);
            }
        }
    }
}

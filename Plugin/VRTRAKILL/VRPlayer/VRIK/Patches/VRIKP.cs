using HarmonyLib;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK.Patches
{
    [HarmonyPatch] internal class VRIKP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void AttachVRIK(NewMovement __instance)
        {
            if (!__instance.gameObject.HasComponent<VRigController>())
            {
                __instance.gameObject.AddComponent<VRigController>();
                VRigController.Instance.Rig = MetaRig.CreateVCustomPreset(Vars.VRCameraContainer);
                VRigController.Instance.Rig.Root.localPosition = new UnityEngine.Vector3(0, 0, 0);
            }
        }
    }
}

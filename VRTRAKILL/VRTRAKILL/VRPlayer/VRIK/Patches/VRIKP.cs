using HarmonyLib;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK.Patches
{
    internal class VRIKP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void AttachVRIK(NewMovement __instance)
        {
            if (!__instance.gameObject.HasComponent<VRigController>())
            {
                __instance.gameObject.AddComponent<VRigController>();
                try
                {
                    VRigController.Instance.Rig = MetaRig.CreateV1CustomPreset(Vars.VRCameraContainer);
                    VRigController.Instance.Rig.Root.localPosition = new UnityEngine.Vector3(0, 0, 0);
                }
                catch (System.NullReferenceException)
                {
                    Vars.Config.Game.VRB.EnableVRIK = false;
                    UnityEngine.Object.Destroy(VRigController.Instance);
                }
            }
        }
    }
}

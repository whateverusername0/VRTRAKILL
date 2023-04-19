using HarmonyLib;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch(typeof(NewMovement))] internal class FixCameraOnRespawn
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.OnEnable))] static void SetPlayerRotation(NewMovement __instance)
        {
            NewMovement.Instance.gameObject.transform.eulerAngles = Vars.VRCameraContainer.transform.eulerAngles; // idk if it's necessary

            NewMovement.Instance.gameObject.transform.right = Vars.VRCameraContainer.transform.right;
            NewMovement.Instance.gameObject.transform.forward = Vars.VRCameraContainer.transform.forward;
        }
    }
}

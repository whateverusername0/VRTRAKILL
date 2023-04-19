using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch(typeof(NewMovement))] internal class FixCameraOnRespawn
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Respawn))] static void SetPlayerRotation(NewMovement __instance)
        {
            CameraConverter.Container.transform.localPosition = Vector3.zero;
            CameraConverter.Container.transform.localRotation = Vars.MainCamera.transform.rotation;

            __instance.transform.localRotation = CameraConverter.Container.transform.localRotation;
        }
    }
}

using HarmonyLib;
using Valve.VR;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch] internal class CrashBandicootCam
    {
        public static GameObject Container;
        /*[HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))]*/ static void ConvertCamera(CameraController __instance)
        {
            // Since the game doesn't want me to use (move/rotate/etc.) it's own camera
            // I'm making a new one (out of spite)
            if (__instance.platformerCamera)
            {
                Container = new GameObject("CameraRig");
                Container.transform.parent = __instance.transform.parent.parent;
                Container.AddComponent<VRCameraController>();

                Camera NewCam = Object.Instantiate(__instance.gameObject.GetComponent<Camera>(), Container.transform);
                Object.Destroy(NewCam.gameObject.GetComponent<CameraController>());
                NewCam.gameObject.name = "VRCamera";
                NewCam.cameraType |= CameraType.VR;
                NewCam.stereoTargetEye = StereoTargetEyeMask.Both;
                NewCam.gameObject.AddComponent<SteamVR_Camera>();

                __instance.gameObject.GetComponent<Camera>().enabled = false;
            }
        }
        // First person stuff
        [HarmonyPrefix] [HarmonyPatch(typeof(PlatformerMovement), nameof(PlatformerMovement.Start))] static void RemoveV1Model(PlatformerMovement __instance)
        { if (Vars.Config.Game.EnableFP4SCam) __instance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false); }
        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))] static void FirstPersonCamera(CameraController __instance)
        { if (__instance.platformerCamera && Vars.Config.Game.EnableFP4SCam) Container.transform.localPosition = new Vector3(0, 0, 0); }
    }
}

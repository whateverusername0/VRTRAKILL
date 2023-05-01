using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch] static class CameraConverter
    {
        // ty huskvr you pretty
        public static GameObject Container;
        public static Camera DesktopCam;

        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void Containerize(NewMovement __instance)
        {
            Container = new GameObject("Main Camera Rig");
            Container.transform.parent = Vars.MainCamera.transform.parent;
            Container.transform.localPosition = Vector3.zero;
            Container.transform.localRotation = Vars.MainCamera.transform.rotation;

            Container.AddComponent<VRCameraController>();

            Vars.MainCamera.transform.parent = Container.transform;

            if (Vars.Config.VRSettings.CreateDesktopCam)
            {
                DesktopCam = new GameObject("Desktop View").AddComponent<Camera>();
                DesktopCam.nearClipPlane = 0.1f;
                DesktopCam.depth = 69; //69 so it dosn't fight with other cameras (haha funny number)
                DesktopCam.stereoTargetEye = StereoTargetEyeMask.None;
                DesktopCam.transform.parent = Vars.MainCamera.transform;
                DesktopCam.transform.localPosition = Vector3.zero;
            }
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))] static void ConvertCameras(CameraController __instance)
        {
            while (__instance.cam == null && __instance.hudCamera == null) {}

            __instance.cam.stereoTargetEye = StereoTargetEyeMask.Both;
            __instance.hudCamera.stereoTargetEye = StereoTargetEyeMask.Both;

            __instance.cam.depth++;
            __instance.hudCamera.depth++;

            XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;

            // for some particular reason destroying it is a bad idea.
            GameObject.Find("Virtual Camera").SetActive(false);
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Update))] static bool IgnoreCC(CameraController __instance)
        {
            // do nothing
            return false;
        }
    }
}

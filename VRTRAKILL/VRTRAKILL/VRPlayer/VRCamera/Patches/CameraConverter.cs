using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch] static class CameraConverter
    {
        // ty huskvr you pretty
        public static GameObject Container;
        public static Camera DesktopCam, DesktopUICam;

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
                GameObject DesktopView = new GameObject("Desktop View");
                DesktopView.transform.parent = Container.transform;
                DesktopView.transform.localPosition = Vector3.zero;

                DesktopCam = new GameObject("Main Camera").AddComponent<Camera>();
                DesktopCam.transform.parent = DesktopView.transform; DesktopCam.transform.localPosition = Vector3.zero;
                DesktopCam.nearClipPlane = 0.1f; DesktopCam.depth = 69;
                DesktopCam.clearFlags = Vars.MainCamera.clearFlags; DesktopCam.cullingMask = Vars.MainCamera.cullingMask;
                DesktopCam.stereoTargetEye = StereoTargetEyeMask.None;

                DesktopUICam = new GameObject("UI Camera").AddComponent<Camera>();
                DesktopUICam.transform.parent = DesktopView.transform; DesktopUICam.transform.localPosition = Vector3.zero;
                DesktopUICam.nearClipPlane = 0.1f; DesktopUICam.depth = 70;
                DesktopUICam.clearFlags = Vars.VRUICamera.clearFlags; DesktopUICam.cullingMask = Vars.VRUICamera.cullingMask;
                DesktopUICam.stereoTargetEye = StereoTargetEyeMask.None;
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

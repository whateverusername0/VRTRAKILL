using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

using Valve.VR;
namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch] internal class CameraConverterP
    {
        // ty huskvr you pretty
        public static GameObject Container;
        public static Camera DesktopWorldCam, DesktopUICam;

        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void Containerize()
        {
            Container = new GameObject("Main Camera Rig");
            Container.transform.parent = Vars.MainCamera.transform.parent;

            Container.transform.localPosition = new Vector3(0, -4.3f, 0);
            Container.transform.localRotation = Vars.MainCamera.transform.rotation;

            Container.AddComponent<VRCameraController>();

            Vars.MainCamera.transform.parent = Container.transform;
            Vars.UICamera.transform.parent = Container.transform;

            // Desktop View
            if (Vars.Config.DesktopView.EnableDV)
            {
                DesktopWorldCam = new GameObject("Desktop World Camera").AddComponent<Camera>();
                DesktopWorldCam.gameObject.AddComponent<DesktopCamera>();

                DesktopUICam = new GameObject("Desktop UI Camera").AddComponent<Camera>();
                DesktopUICam.gameObject.AddComponent<DesktopUICamera>();
            }
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void ScaleObjects()
        {
            // this should've been bigger, but i've changed my mind a thousand years ago and it works
            // this mod is officially considered my opus magnum spaghetti code and dumpster fire
            Container.transform.localScale = new Vector3(2, 2, 2);
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))] static void ConvertCameras(CameraController __instance)
        {
            while (__instance.cam == null && __instance.hudCamera == null) {}

            __instance.cam.nearClipPlane = .01f;
            __instance.cam.stereoTargetEye = StereoTargetEyeMask.Both;
            // some binary magic (that i don't understand) to enable another layer
            __instance.cam.cullingMask |= 1 << (int)Vars.Layers.AlwaysOnTop;
            __instance.cam.depth++;
            
            __instance.hudCamera.stereoTargetEye = StereoTargetEyeMask.Both;
            __instance.hudCamera.depth++;

            XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;

            // for some particular reason destroying it is a bad idea.
            GameObject.Find("Virtual Camera").SetActive(false);
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))] static void AddSVRCam(CameraController __instance)
        { __instance.gameObject.AddComponent<SteamVR_Camera>(); }
    }
}

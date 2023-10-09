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
        public static Camera DesktopWorldCam, DesktopUICam, SpectatorCam;

        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void Containerize()
        {
            Container = new GameObject("Main Camera Rig");
            Container.transform.parent = Vars.MainCamera.transform.parent;

            Container.transform.localPosition = new Vector3(0, -4.3f, 0);
            Container.transform.localRotation = Vars.MainCamera.transform.rotation;

            Container.AddComponent<VRCameraController>();

            Vars.MainCamera.transform.parent = Container.transform;
            Vars.UICamera.transform.parent = Container.transform;

            #region Desktop View
            DesktopWorldCam = new GameObject("Desktop World Camera").AddComponent<Camera>();
            DesktopWorldCam.transform.parent = Vars.MainCamera.transform;
            DesktopWorldCam.transform.localPosition = Vector3.zero;
            DesktopWorldCam.gameObject.AddComponent<DesktopCamera>();
            DesktopWorldCam.gameObject.SetActive(false);

            DesktopUICam = new GameObject("Desktop UI Camera").AddComponent<Camera>();
            DesktopUICam.transform.parent = Vars.MainCamera.transform;
            DesktopUICam.transform.localPosition = Vector3.zero;
            DesktopUICam.gameObject.AddComponent<DesktopUICamera>();
            DesktopUICam.gameObject.SetActive(false);
            #endregion

            #region Spectator Camera
            GameObject SCContainer = new GameObject("Spectator Camera");
            SCContainer.transform.localPosition = Vector3.zero;

            GameObject SCOffset = new GameObject("SC Limiter");
            SCOffset.transform.parent = SCContainer.transform;
            SCOffset.transform.localPosition = Vector3.zero;

            SpectatorCam = new GameObject("SC Camera").AddComponent<Camera>();
            SpectatorCam.transform.parent = SCContainer.transform;
            SpectatorCam.transform.localPosition = Vector3.zero;
            SpectatorCam.stereoTargetEye = StereoTargetEyeMask.None;

            // Pushback 2.0
            Rigidbody SCRB = SpectatorCam.gameObject.AddComponent<Rigidbody>();
            SCRB.mass = 0; SCRB.drag = 0; SCRB.angularDrag = 0; SCRB.useGravity = false;
            SCRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            SCRB.constraints = RigidbodyConstraints.FreezeRotation;
            SphereCollider SCSC = SpectatorCam.gameObject.AddComponent<SphereCollider>(); SCSC.radius = .01f;

            ThirdPersonCamera SPC = SCContainer.AddComponent<ThirdPersonCamera>();
            SPC.FollowTarget = Vars.MainCamera.transform;
            SPC.Offset = SCOffset.transform;
            SPC.RB = SCRB;

            SCContainer.SetActive(false);
            #endregion
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void ScaleObjects(NewMovement __instance)
        {
            // this should've been bigger, but i've changed my mind a thousand years ago and it works
            // this mod is officially my opus magnum spaghetti code and dumpster fire
            Container.transform.localScale = new Vector3(2, 2, 2);
            __instance.gameObject.AddComponent<VRKeybindsController>();
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))] static void ConvertCameras(CameraController __instance)
        {
            while (__instance.cam == null && __instance.hudCamera == null) {}

            __instance.cam.nearClipPlane = .01f;
            __instance.cam.stereoTargetEye = StereoTargetEyeMask.Both;
            // some binary magic (that i don't understand) to enable another layer
            __instance.cam.cullingMask |= 1 << (int)Layers.AlwaysOnTop;
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

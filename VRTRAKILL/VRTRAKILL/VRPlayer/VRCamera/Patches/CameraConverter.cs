using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch] static class CameraConverter
    {
        // ty huskvr you pretty
        public static GameObject Container;

        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))] static void Containerize(NewMovement __instance)
        {
            Container = new GameObject("Main Camera Rig");
            Container.transform.parent = Vars.MainCamera.transform.parent;
            Container.transform.localPosition = Vector3.zero;
            Container.transform.localRotation = Vars.MainCamera.transform.rotation;

            Container.AddComponent<VRCameraController>();

            Vars.MainCamera.transform.parent = Container.transform;
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))] static void ConvertCameras(CameraController __instance)
        {
            while (__instance.cam == null) {}
            __instance.gameObject.AddComponent<SteamVR_CameraHelper>(); // dunno if this is needed

            __instance.cam.depth++;

            __instance.cam.stereoTargetEye = StereoTargetEyeMask.Both;
            XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;

            if (PostProcessV2_Handler.Instance != null)
                PostProcessV2_Handler.Instance.mainCam = __instance.cam;

            GameObject.Find("Virtual Camera").SetActive(false);
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Update))] static bool IgnoreCC(CameraController __instance)
        {
            // do nothing
            return false;
        }
    }
}

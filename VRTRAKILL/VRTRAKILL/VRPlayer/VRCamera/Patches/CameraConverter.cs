using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch] internal class CameraConverter
    {
        // ty huskvr you pretty
        public static GameObject Container;

        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Start")] static void Containerize(NewMovement __instance)
        {
            Container = new GameObject("Main Camera Rig");
            Container.transform.parent = Vars.MainCamera.transform.parent;
            Container.transform.localPosition = Vector3.zero;
            Container.transform.localRotation = Vars.MainCamera.transform.rotation;

            Container.AddComponent<VRCameraController>();

            Vars.MainCamera.transform.parent = Container.transform;
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), "Start")] static void ConvertMainCamera(CameraController __instance)
        {
            // MainCamera
            while (__instance.cam == null) {}
            __instance.gameObject.AddComponent<SteamVR_CameraHelper>();

            __instance.cam.depth++;

            __instance.cam.stereoTargetEye = StereoTargetEyeMask.Both;
            XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;

            if (PostProcessV2_Handler.Instance != null)
                Traverse.Create(PostProcessV2_Handler.Instance).Field("mainCam").SetValue(__instance.cam);

            GameObject.Find("Virtual Camera").SetActive(false);
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(CameraController), "Start")] static void MainCameraTweaks(CameraController __instance)
        {
            __instance.enabled = false; // disable mouselook
        }
    }
}

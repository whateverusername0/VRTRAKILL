using System.Collections;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Plugin.VRTRAKILL.Patches
{
    [HarmonyPatch] internal class VRCamera
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), "Start")] static void ConvertMainCamera(CameraController __instance)
        {
            // MainCamera
            while (__instance.cam == null) {}
            __instance.gameObject.AddComponent<SteamVR_CameraHelper>();

            //__instance.cam.useOcclusionCulling = true;
            //__instance.cam.backgroundColor = Color.black;
            //__instance.cam.depth++;

            __instance.cam.stereoTargetEye = StereoTargetEyeMask.Both;
            XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;

            if (PostProcessV2_Handler.Instance != null)
            {
                Traverse.Create(PostProcessV2_Handler.Instance).Field("mainCam").SetValue(__instance.cam);
            }

            GameObject.Find("Virtual Camera").SetActive(false);
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(CameraController), "Start")] static void MainCameraTweaks(CameraController __instance)
        {
            __instance.enabled = false; // this should disable mouselook
        }
    }
}

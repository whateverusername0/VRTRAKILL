using System.Collections;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.XR;
using Valve.VR;

namespace Plugin.VRTRAKILL.Patches
{
    [HarmonyPatch] internal class VRCamera
    {
        // ty huskvr for le cod
        public static GameObject Container;

        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Start")] static void Containerize(NewMovement __instance)
        {
            Container = new GameObject("Main Camera Container");
            Container.transform.parent = GameObject.Find("Main Camera").transform.parent;
            Container.transform.localPosition = Vector3.zero;
            Container.transform.localRotation = GameObject.Find("Main Camera").transform.rotation;

            GameObject.Find("Main Camera").transform.parent = Container.transform;
        }
        // No snap turn because motion sickness should not be considered a problem, ESPECIALLY when playing ULTRAKILL.
        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Update")] static void SmoothTurn(NewMovement __instance)
        {
            if (__instance.dead) return;
            __instance.transform.rotation =
                Quaternion.Euler(__instance.transform.rotation.eulerAngles.x,
                                 GameObject.Find("Main Camera").transform.rotation.eulerAngles.y,
                                 __instance.transform.rotation.eulerAngles.z);

            Container.transform.rotation = Quaternion.Euler(0f, VRInputManager.TurnOffset, 0f);
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), "Start")] static void ConvertMainCamera(CameraController __instance)
        {
            // MainCamera
            while (__instance.cam == null) {}
            __instance.gameObject.AddComponent<SteamVR_CameraHelper>();

            //__instance.cam.useOcclusionCulling = true;
            //__instance.cam.backgroundColor = Color.black;
            __instance.cam.depth++;

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

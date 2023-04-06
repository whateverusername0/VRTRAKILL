using System;
using UnityEngine;
using HarmonyLib;
using System.Collections;

namespace Plugin.Patches
{
    // Converts existing camera into a VR one
    // Thanks HuskVR for le code

    // Big shoutout to Hakita for making me suffer for 6 hours.
    // Breaking my head trying to access *private* vars and it was THAT easy :(
    [HarmonyPatch] internal class VRCameraP
    {
        //Plugin.PLogger.LogMessage("");
        private static IEnumerator ConvertCamera(Camera Cam, Action<Camera> Callback, Action<Camera> PostProcFound)
        {
            GameObject GO = Cam.gameObject;

            // Camera
            Plugin.PLogger.LogMessage($"Saving {GO.name} values");
            float CDepth = Cam.depth;
            CameraType CCameraType = Cam.cameraType;
            CameraClearFlags CClearFlags = Cam.clearFlags;
            int CCullingMask = Cam.cullingMask;
            RenderTexture CTargetTexture = Cam.targetTexture;
            float CNearClipPlane = Cam.nearClipPlane;
            float CFarClipPlane = Cam.farClipPlane;

            // Autoaim
            CameraFrustumTargeter CFT = GO.GetComponent<CameraFrustumTargeter>();
            RectTransform AACrosshair = null;
            LayerMask AAMask = -1;
            LayerMask AAOcclusionMask = -1;
            float AAMaximumRange = 0f;
            float AAMaxHorAim = 0f;

            bool ReplaceAutoaim = false;
            if (GO.GetComponent<CameraFrustumTargeter>() != null)
            {
                ReplaceAutoaim = true;
                Plugin.PLogger.LogMessage("Detected autoaim, saving values & removing");
                AACrosshair = (RectTransform)Traverse.Create(CFT).Field("crosshair").GetValue();
                AAMask = (LayerMask)Traverse.Create(CFT).Field("mask").GetValue();
                AAOcclusionMask = (LayerMask)Traverse.Create(CFT).Field("occulsionMask").GetValue();
                AAMaximumRange = (float)Traverse.Create(CFT).Field("maximumRange").GetValue();
                AAMaxHorAim = (float)Traverse.Create(CFT).Field("maxHorAim").GetValue();

                GameObject.Destroy(GO.GetComponent<CameraFrustumTargeter>());
            }
            GameObject.Destroy(Cam);
            yield return new WaitForEndOfFrame();

            Plugin.PLogger.LogMessage($"Readding VR-ed {GO.name}");
            Camera NewCam = GO.AddComponent<Camera>();
            NewCam.depth = CDepth;
            NewCam.cameraType = CCameraType | CameraType.VR;
            if (GO.name == "Main Camera") NewCam.clearFlags = CClearFlags;
            else NewCam.clearFlags = CameraClearFlags.Depth;
            NewCam.cullingMask = CCullingMask;
            NewCam.targetTexture = CTargetTexture;
            NewCam.backgroundColor = Color.black;
            NewCam.nearClipPlane = CNearClipPlane;
            NewCam.farClipPlane = CFarClipPlane;

            UnityEngine.XR.XRSettings.gameViewRenderMode = UnityEngine.XR.GameViewRenderMode.RightEye;

            if (ReplaceAutoaim)
            {
                Plugin.PLogger.Log(0, "Readding autoaim");
                CameraFrustumTargeter NewCFT = GO.AddComponent<CameraFrustumTargeter>();
                Traverse.Create(NewCFT).Field("crosshair").SetValue(AACrosshair);
                Traverse.Create(NewCFT).Field("mask").SetValue(AAMask);
                Traverse.Create(NewCFT).Field("occulsionMask").SetValue(AAOcclusionMask);
                Traverse.Create(NewCFT).Field("maximumRange").SetValue(AAMaximumRange);
                Traverse.Create(NewCFT).Field("maxHorAim").SetValue(AAMaxHorAim);
            }


            Plugin.PLogger.LogMessage("Invoking Callback"); Callback(NewCam);
            yield return new WaitUntil(() => PostProcessV2_Handler.Instance != null);
            Plugin.PLogger.LogMessage("Invoking PostProcess Callback"); PostProcFound(NewCam);
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), "Start")] static void PreCameraInit(CameraController __instance)
        {
            __instance.StartCoroutine(ConvertCamera
            (
                __instance.cam,
                (Camera Cam) => { __instance.cam = Cam; },
                (Camera Cam) =>
                {
                    if (PostProcessV2_Handler.Instance != null)
                        Traverse.Create(PostProcessV2_Handler.Instance).Field("mainCam").SetValue(Cam);
                }
            ));
            Camera HUDCam = GameObject.Find("HUD Camera").GetComponent<Camera>();
            __instance.StartCoroutine(ConvertCamera
            (
                GameObject.Find("HUD Camera").GetComponent<Camera>(),
                (Camera Cam) => Traverse.Create(__instance).Field("hudCamera").SetValue(Cam),
                (Camera Cam) =>
                {
                    if (PostProcessV2_Handler.Instance != null)
                        PostProcessV2_Handler.Instance.hudCam = Cam;
                }
            ));
        }

        public static float Offset = 0;
        private static GameObject Container;
        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Start")] static void SetupCameraCorrection()
        {
            Container = new GameObject("Main Camera Container");
            Container.transform.parent = Helpers.UKStuff.MainCamera.transform.parent;
            Container.transform.localPosition = Vector3.zero;
            Container.transform.localRotation = Helpers.UKStuff.MainCamera.transform.rotation;

            Helpers.UKStuff.MainCamera.transform.parent = Container.transform;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Update")] static void CameraCorrection(NewMovement __instance)
        {
            if (__instance.dead) return;

            __instance.transform.rotation = Quaternion.Euler(__instance.transform.rotation.eulerAngles.x, 
                                                             Helpers.UKStuff.MainCamera.transform.rotation.eulerAngles.y,
                                                             __instance.transform.rotation.eulerAngles.z);
            Container.transform.rotation = Quaternion.Euler(0f, Offset, 0f);
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(PostProcessV2_Handler), "SetupRTs")] static bool DontUpdateRTsIfCamNull(PostProcessV2_Handler __instance)
        {
            return CameraController.Instance.cam != null && __instance.hudCam != null;
        }
    }
}
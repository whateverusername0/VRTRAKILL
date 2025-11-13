using HarmonyLib;
using UnityEngine.XR;
using UnityEngine;
using Valve.VR;
using VRTRAKILL.Data;

namespace VRTRAKILL.Patches.ULTRAKILL;

[HarmonyPatch(typeof(CameraController))] internal class PatchCameraController
{
    [HarmonyPrefix] [HarmonyPatch(nameof(CameraController.Start))]
    static void ConvertCameras(CameraController __instance)
    {
        while (__instance.cam == null && __instance.hudCamera == null) { }

        __instance.cam.nearClipPlane = .01f;
        __instance.cam.stereoTargetEye = StereoTargetEyeMask.Both;
        // binary magic to add another layer
        __instance.cam.cullingMask |= 1 << (int)Layers.AlwaysOnTop;
        __instance.cam.depth++;

        __instance.hudCamera.stereoTargetEye = StereoTargetEyeMask.Both;
        __instance.hudCamera.depth++;

        XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;

        // for some particular reason destroying it is a bad idea so we set it to fvlse.
        GameObject.Find("Virtual Camera").SetActive(false);
    }
    [HarmonyPostfix] [HarmonyPatch(nameof(CameraController.Start))]
    static void AddSVRCam(CameraController __instance)
    {
        __instance.gameObject.AddComponent<SteamVR_Camera>();
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(CameraController.Update))]
    static bool DoNothing()
    {
        // i can't remove it so let it do nothing.
        return false;
    }
}
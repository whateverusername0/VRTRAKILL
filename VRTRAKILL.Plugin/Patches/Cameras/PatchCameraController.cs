using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace VRTRAKILL.Plugin.Patches.Cameras;

[HarmonyPatch(typeof(CameraController))]
internal static class PatchCameraController
{
    private static ManualLogSource _log => Plugin.Instance.Log;

    [HarmonyPrefix, HarmonyPatch(nameof(CameraController.Start))]
    private static void DisableDefaultCamera(CameraController __instance)
    {
        if (__instance.cam == null)
        {
            _log.LogError("Cannot disable default camera on CC.Start. For some reason.");
            return;
        }

        var vc = GameObject.Find("Virtual Camera");
        if (vc != null) vc.SetActive(false);
        __instance.cam.enabled = false;
    }


}

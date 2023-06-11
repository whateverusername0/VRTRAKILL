using HarmonyLib;
using Valve.VR;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch] internal class _4SCameraP
    {
        static GameObject Container;
        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))] static void Convert4SCam(CameraController __instance)
        {
            if (__instance.platformerCamera)
            {
                // Containerize
                Container = new GameObject("CamRig");
                Container.transform.parent = __instance.transform.parent.parent;
                __instance.transform.parent.parent = Container.transform;
                Container.AddComponent<VRCameraController>();

                __instance.gameObject.AddComponent<SteamVR_Camera>();
            }
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), nameof(CameraController.Start))] static void FirstPerson4SCam(CameraController __instance)
        {
            if (__instance.platformerCamera && Vars.Config.Game.EnableFP4SCam)
                Container.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}

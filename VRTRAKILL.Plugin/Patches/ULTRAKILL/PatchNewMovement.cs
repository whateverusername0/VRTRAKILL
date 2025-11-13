using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.VRAvatar;
using VRTRAKILL.Systems.VRCamera;
using VRTRAKILL.Systems.VRPlayer;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Patches.ULTRAKILL.Movement;

[HarmonyPatch(typeof(NewMovement))] internal static partial class PatchNewMovement
{
    [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Start))]
    static void Start(NewMovement __instance)
    {
        __instance.walkSpeed *= Vars.Config.UpdateMultiplier;
        __instance.jumpPower *= Vars.Config.UpdateMultiplier;
        __instance.wallJumpPower *= Vars.Config.UpdateMultiplier;

        // vravatar
        if (!__instance.gameObject.HasComponent<VRigController>())
        {
            __instance.gameObject.AddComponent<VRigController>();
            VRigController.Instance.Rig = MetaRig.CreateVCustomPreset(Vars.VRCameraContainer);
            VRigController.Instance.Rig.Root.localPosition = new Vector3(0, 0, 0);
        }

        // camera controller
        var cameraContainer = new GameObject("Main Camera Rig");
        cameraContainer.transform.parent = Vars.MainCamera.transform.parent;

        cameraContainer.transform.localPosition = new Vector3(0, -4.3f, 0);
        cameraContainer.transform.localRotation = Vars.MainCamera.transform.rotation;

        cameraContainer.AddComponent<VRCameraController>();

        Vars.MainCamera.transform.parent = cameraContainer.transform;
        Vars.UICamera.transform.parent = cameraContainer.transform;

        // setup desktop camera, separate from the headset, for better view.
        var desktopCam = new GameObject("Desktop World Camera").AddComponent<Camera>();
        desktopCam.transform.parent = Vars.MainCamera.transform;
        desktopCam.transform.localPosition = Vector3.zero;
        desktopCam.gameObject.AddComponent<VRDesktopCamera>();

        var desktopUICam = new GameObject("Desktop UI Camera").AddComponent<Camera>();
        desktopUICam.transform.parent = Vars.MainCamera.transform;
        desktopUICam.transform.localPosition = Vector3.zero;
        desktopUICam.gameObject.AddComponent<VRDesktopUICamera>();

        if (!Vars.Config.DesktopView.Enabled)
        {
            desktopCam.gameObject.SetActive(false);
            desktopUICam.gameObject.SetActive(false);
        }

        // this scaleup exists so that every single object in the game is not as big as your forehead
        cameraContainer.transform.localScale = new Vector3(2, 2, 2);

        // this is important
        Vars.VRCameraContainer = cameraContainer.transform;
        Vars.DesktopCamera = desktopCam.gameObject;
        Vars.DesktopUICamera = desktopUICam.gameObject;

        __instance.gameObject.AddComponent<VRKeybindsController>();
    }

    [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Respawn))]
    static void Respawn(NewMovement __instance)
    {
        __instance.cc.enabled = false; // hi! fuck you
    }
}
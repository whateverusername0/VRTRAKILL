using HarmonyLib;
using UnityEngine;
using Valve.VR;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.VRCamera;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Patches.ULTRAKILL;

[HarmonyPatch(typeof(CameraController))] internal class PatchCameraController
{
    public static SteamVR_TrackedObject HMD;

    [HarmonyPrefix] [HarmonyPatch(typeof(Camera), "set_fieldOfView")]
    // Unity already prevents this, but it also nags you constantly about it.
    // Some games try to change the FOV every frame, and all those logs can reduce performance.
    private static bool Set_FieldOfView()
        => false;

    /// <summary>
    ///     PATCH: Add <see cref="Layers.AlwaysOnTop"/> to the Main Camera.
    ///     Also tweak a couple values for convenience.
    /// </summary>
    [HarmonyPrefix] [HarmonyPatch(nameof(CameraController.Start))]
    static void ConvertCameras(CameraController __instance)
    {
        while (__instance.cam == null && __instance.hudCamera == null) { }

        __instance.cam.nearClipPlane = .01f;
        __instance.cam.stereoTargetEye = StereoTargetEyeMask.Both;

        // AlwaysOnTop responds for your weapons view.
        __instance.cam.cullingMask |= 1 << (int)Layers.AlwaysOnTop;

        __instance.hudCamera.stereoTargetEye = StereoTargetEyeMask.Both;
    }

    /// <summary>
    ///     PATCH: Add head tracking, make it render on your headset.
    /// </summary>
    [HarmonyPostfix] [HarmonyPatch(nameof(CameraController.Start))]
    static void AddSVRCam(CameraController __instance)
    {
        var go = new GameObject("SteamVR Tracked HMD");
        HMD = go.AddComponent<SteamVR_TrackedObject>();
        HMD.index = SteamVR_TrackedObject.EIndex.Hmd;

        var svrb = __instance.gameObject.EnsureComponent<SteamVRBridge>();
        var vc = GameObject.Find("Virtual Camera");
        if (vc != null) svrb.RenderingCamera = vc.GetComponent<Camera>();
    }

    /// <summary>
    ///     PATCH: Overridde FoV behavior, make look value depend on <see cref="SteamVR_Actions.default_Turn"/>.
    /// </summary>
    [HarmonyPrefix] [HarmonyPatch(nameof(CameraController.LateUpdate))]
    static bool LateUpdate(CameraController __instance)
    {
        if (!__instance.nm)
            return false;

        var gravityDirection = __instance.player.GetGravityDirection();
        __instance.gravityVec = Vector3.Slerp(__instance.gravityVec, gravityDirection, 5f * Time.deltaTime);
        if (Vector3.Angle(__instance.gravityVec, gravityDirection) < 0.01f)
            __instance.gravityVec = gravityDirection;

        var fromDirection = __instance.gravityRotation * Vector3.up;
        var toDirection = -__instance.gravityVec;
        __instance.gravityRotation = Quaternion.FromToRotation(fromDirection, toDirection) * __instance.gravityRotation;

        var vec = SteamVR_Actions._default.Turn.delta;
        var speed = Vars.Config.Controllers.SmoothSpeed;
        var y = vec.y * speed;

        __instance.rotationY += __instance.reverseX ? -y : y;

        float f = Mathf.DeltaAngle(0f, __instance.rotationX);
        if (Mathf.Abs(f) > 90f)
            __instance.rotationX = 90f * Mathf.Sign(f);

        __instance.ApplyRotations();

        return false;
    }

    /// <summary>
    ///     PATCH: Seamlessly integrate HMD rotation to the player.
    ///     Seamlessly as in the player will not notice any camera bullshit happening in front of him.
    /// </summary>
    [HarmonyPostfix] [HarmonyPatch(nameof(CameraController.ApplyRotations))]
    static void ApplyRotations(CameraController __instance)
    {
        if (HMD == null) return;

        var hmd = HMD.transform;
        __instance.transform.localRotation *= hmd.localRotation;
        MonoSingleton<NewMovement>.Instance.transform.localEulerAngles += new Vector3(0, hmd.localEulerAngles.y, 0);
    }
}
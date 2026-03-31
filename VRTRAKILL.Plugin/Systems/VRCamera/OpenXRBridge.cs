using BepInEx.Logging;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Systems.VRCamera;

public class OpenXRBridge : MonoBehaviour
{
    private ManualLogSource Log => Plugin.Log;

    public Camera RenderingCamera, CloneCamera;
    public RenderTexture RenderTexture;

    protected void Start()
    {
        Log.LogDebug($"[{this}] - Start();");

        if (RenderingCamera == null)
        {
            Log.LogWarning($"[{this}] Rendering camera is null! Trying the fallback option.");
            var virtualCam = GameObject.Find("Virtual Camera").GetComponent<Camera>();
            RenderingCamera = virtualCam != null ? virtualCam : Camera.main;
        }

        // still?
        if (RenderingCamera == null)
        {
            Log.LogError($"[{this}] Rendering camera not found! Aborting.");
            return;
        }

        RenderTexture = !RenderTexture ? GetDefaultRenderTexture() : RenderTexture;
        if (RenderTexture == null)
        {
            Log.LogError($"[{this}] Render texture is null. Aborting.");
        }

        AttachCurrent();
    }

    protected void OnDestroy()
    {
        Log.LogDebug($"[{this}] - OnDestroy();");
        DetachCurrent();
    }

    public RenderTexture GetDefaultRenderTexture()
    {
        // log slop is here because i had a lot of trouble with nullrefs.
        // let it stay this way
        Log.LogDebug("Getting XR general settings");
        var xr = XRGeneralSettings.Instance;

        Log.LogDebug("Getting XR manager");
        var manager = xr.Manager;

        Log.LogDebug("Getting XR active loader");
        var loader = manager.activeLoader;

        Log.LogDebug("Getting XR display subsystem");
        var display = loader.GetLoadedSubsystem<XRDisplaySubsystem>();

        if (!display.running)
        {
            Log.LogWarning("XR Display Subsystem is not running??? Starting it up again!");
            display.Start();
        }
        Log.LogDebug("Getting texture.");

        var texture = display.GetRenderTextureForRenderPass(0);

        if (texture == null)
            Log.LogError("Default Render Texture is null.");

        return texture;
    }

    public void Attach(Camera cam, RenderTexture rt, out Camera clone)
    {
        clone = null;
        if (cam == null)
        {
            Log.LogError($"Camera is null. Aborting.");
            return;
        }

        if (cam.gameObject.TryGetComponent<OpenXRBridge>(out var existing) && existing != this)
        {
            Log.LogError($"{cam.name} already has a bridge. Aborting.");
            return;
        }

        if (rt == null)
        {
            Log.LogError("Render Texture is null. Aborting.");
            return;
        }

        clone = new GameObject().AddComponent<Camera>();
        clone.CopyFrom(cam);
        // then do nothing. it's for desktop view.

        if (cam.orthographic)
        {
            var fov = 2.0f * Mathf.Atan(cam.orthographicSize) * Mathf.Rad2Deg;
            cam.fieldOfView = fov; // it will most likely be ignored but i'll leave it jic
            cam.orthographic = false;
        }

        cam.stereoTargetEye = StereoTargetEyeMask.Both;
        cam.cameraType |= CameraType.VR;
        cam.targetTexture = rt;

        Log.LogInfo($"\"{cam.name}\" made stereo|VR and attached to \"{rt.name}\"");
    }

    public void AttachCurrent()
        => Attach(RenderingCamera, RenderTexture, out CloneCamera);

    public void Detach(Camera cam, Camera clone = null)
    {
        cam.targetTexture = null;
        cam.cameraType &= ~CameraType.VR; // excluding the VR flag

        if (clone != null)
        {
            if (clone.orthographic)
                cam.orthographic = true; // reverting
            Destroy(clone);
        }

        Log.LogInfo($"\"{cam.name}\" unmade stereo and dettached from any render textures.");
    }

    public void DetachCurrent()
        => Detach(RenderingCamera, CloneCamera);
}
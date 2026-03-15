using BepInEx.Logging;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using VRTRAKILL.Data;

namespace VRTRAKILL.Systems.VRCamera;

public class OpenVRBridge : MonoBehaviour
{
    private ManualLogSource Log => Vars.Log;

    public Camera RenderingCamera;

    protected void Start()
    {
        Log.LogMessage($"OpenVRBridge attached to {gameObject.name}. Begin conversion.");

        if (RenderingCamera == null)
        {
            Plugin.Log.LogWarning("Rendering camera is null! Trying the fallback option.");
            var virtualCam = GameObject.Find("Virtual Camera").GetComponent<Camera>();
            RenderingCamera = virtualCam != null ? virtualCam : Camera.main;
        }

        // still?
        if (RenderingCamera == null)
        {
            Plugin.Log.LogError("Rendering camera not found! Initiate spontaneous combustion!");
            return;
        }

        Plugin.Log.LogDebug("Getting XR general settings");
        var xr = XRGeneralSettings.Instance;

        Plugin.Log.LogDebug("Getting XR manager");
        var manager = xr.Manager;

        Plugin.Log.LogDebug("Getting XR active loader");
        var loader = manager.activeLoader;

        Plugin.Log.LogDebug("Getting XR display subsystem");
        var display = loader.GetLoadedSubsystem<XRDisplaySubsystem>();

        if (!display.running)
        {
            Plugin.Log.LogWarning("XR Display Subsystem is not running??? Starting it up again!");
            display.Start();
        }
        Plugin.Log.LogDebug("Getting texture.");

        var texture = display.GetRenderTextureForRenderPass(0);
        Plugin.Log.LogDebug($"Texture is {(texture == null ? "NULL!" : texture.name)}");

        RenderingCamera.stereoTargetEye = StereoTargetEyeMask.Both;
        RenderingCamera.targetTexture = texture;

        Log.LogMessage("OpenVR bridge initialization success!");
    }
}
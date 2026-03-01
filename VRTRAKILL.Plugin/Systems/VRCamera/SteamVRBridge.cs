using BepInEx.Logging;
using UnityEngine;
using Valve.VR;
using VRTRAKILL.Data;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Systems.VRCamera;

// Attach to the Virtual Camera or run from a small manager MonoBehaviour.
// This will ensure SteamVR_Render/SteamVR_Camera are present and leaves the camera rendering to backbuffer.
public class SteamVRBridge : MonoBehaviour
{
    private ManualLogSource Log => Vars.Log;

    public SteamVR_Render Render;
    public Camera RenderingCamera;

    protected void Start()
    {
        Log.LogMessage($"SteamVRBridge attached to {gameObject.name}. Begin conversion.");

        RenderingCamera = RenderingCamera == null ? GetComponent<Camera>() : RenderingCamera;

        if (RenderingCamera == null)
        {
            Log.LogWarning($"Unable to find a valid camera! Resorting to Virtual Camera, which should be present.");
            var obj = GameObject.Find("Virtual Camera");
            if (obj != null) RenderingCamera = obj.GetComponent<Camera>();
        }

        if (RenderingCamera == null)
        {
            Log.LogError("Virtual Camera not found or Render Source left unprovided!");
            enabled = false;
            return;
        }

        RenderingCamera.stereoTargetEye = StereoTargetEyeMask.Both;
        RenderingCamera.cameraType |= (CameraType)(1 << (int)CameraType.VR);

        // Ensure SteamVR_Render is attached once — it will submit frames to the SteamVR compositor.
        if (Render == null)
        {
            Log.LogWarning("SteamVR_Render is not attached to the bridge! Resolving.");
            var renders = FindObjectsOfType<SteamVR_Render>();
            if (renders.Length > 1)
            {
                Log.LogWarning("Found more than 1 instance of a SteamVR_Render! Resolving.");
                for (int i = 1; i < renders.Length; i++)
                    DestroyImmediate(renders[i]);
            }

            if (renders.Length == 0)
                Render = RenderingCamera.gameObject.EnsureComponent<SteamVR_Render>();
            else Render = renders[0];
        }

        RenderingCamera.gameObject.EnsureComponent<SteamVR_Camera>();

        Log.LogMessage("SteamVR bridge initialization success!");
    }
}
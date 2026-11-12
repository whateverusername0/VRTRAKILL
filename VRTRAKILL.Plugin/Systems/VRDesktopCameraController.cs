using BepInEx.Logging;
using UnityEngine;
using VRTRAKILL.Plugin.Data;
using VRTRAKILL.Plugin.Utilities;

namespace VRTRAKILL.Plugin.Systems;

[DefaultExecutionOrder(1100)] // should go after vrcc
public class VRDesktopCameraController : MonoSingleton<VRDesktopCameraController>
{
    private ManualLogSource _log => Plugin.Instance.Log;
    private VRCameraController _vrcc => VRCameraController.Instance;

    public Camera DesktopCamera { get; private set; }
    public Camera DesktopUICamera { get; private set; }

    protected void Start()
    {
        if (_vrcc == null || _vrcc.LeftEye == null)
        {
            _log.LogError("VRCameraController is not initialized yet. Weird.");
            return;
        }

        _vrcc.LeftEye.nearClipPlane = .01f;
        _vrcc.LeftEye.stereoTargetEye = StereoTargetEyeMask.None;
        _vrcc.LeftEye.cullingMask |= 1 << (int)Layers.AlwaysOnTop;

        Validate();
    }

    // yeah yeah everything is inside of Update whatever
    // i'm making this shit bulletproof

    protected void LateUpdate()
    {
        if (_vrcc == null || _vrcc.LeftEye == null)
            return;

        Validate();

        // setting it once doesn't work. maybe. test that later.
        DesktopCamera.CopyFrom(_vrcc.LeftEye);
        DesktopCamera.depth = 100;

        // setting it once doesn't work. maybe. test that later.
        DesktopUICamera.CopyFrom(_vrcc.LeftEye);
        DesktopUICamera.depth = 101;
    }

    protected void Validate()
    {
        if (DesktopCamera == null)
        {
            var c = AddChildCamera("Desktop camera", _vrcc.LeftEye);
            c.transform.SetParent(_vrcc.transform.parent);
            c.stereoTargetEye = StereoTargetEyeMask.None;
        }

        if (DesktopUICamera == null)
        {
            var c = AddChildCamera("Desktop UI camera", _vrcc.LeftEye);
            c.transform.SetParent(_vrcc.transform.parent);
            c.stereoTargetEye = StereoTargetEyeMask.None;
            c.cullingMask = 1 << (int)Layers.UI;
        }
    }

    protected Camera AddChildCamera(string name, Camera parent)
    {
        var cam = new GameObject(name).EnsureComponent<Camera>();
        cam.CopyFrom(parent);
        return cam;
    }
}

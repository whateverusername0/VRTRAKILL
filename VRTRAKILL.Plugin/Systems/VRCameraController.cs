using BepInEx.Logging;
using UnityEngine;
using VRTRAKILL.Plugin.Data;
using VRTRAKILL.Plugin.Utilities;

namespace VRTRAKILL.Plugin.Systems;

[DefaultExecutionOrder(1000)]
public class VRCameraController : MonoSingleton<VRCameraController>
{
    private ManualLogSource _log => Plugin.Instance.Log;
    private CameraController _cc => CameraController.Instance;

    public Camera LeftEye { get; private set; }

    public Camera RightEye { get; private set; }

    public Camera UICamera { get; private set; }

    protected void Start()
    {
        if (_cc == null || _cc.cam == null)
        {
            _log.LogError("CameraController is not initialized yet. Cannot configure cameras.");
            return;
        }

        _cc.cam.nearClipPlane = .01f;
        _cc.cam.stereoTargetEye = StereoTargetEyeMask.None;
        _cc.cam.cullingMask |= 1 << (int)Layers.AlwaysOnTop;

        if (LeftEye == null)
        {
            var c = AddChildCamera("Left eye camera", _cc.cam);
            c.transform.SetParent(_cc.transform.parent);
            c.stereoTargetEye = StereoTargetEyeMask.Left;
        }

        if (RightEye == null)
        {
            var c = AddChildCamera("Right eye camera", _cc.cam);
            c.transform.SetParent(_cc.transform.parent);
            c.stereoTargetEye = StereoTargetEyeMask.Right;
        }

        if (UICamera == null)
        {

        }
    }

    // yeah yeah everything is inside of Update whatever
    // i'm making this shit bulletproof

    protected void Update()
    {
        if (_cc == null || _cc.cam == null)
            return;

        
    }

    protected Camera AddChildCamera(string name, Camera parent)
    {
        var cam = new GameObject(name).EnsureComponent<Camera>();
        cam.CopyFrom(parent);
        return cam;
    }
}
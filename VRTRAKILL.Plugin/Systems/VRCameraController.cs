using BepInEx.Logging;
using UnityEngine;
using UnityEngine.XR;
using VRTRAKILL.Plugin.Data;
using VRTRAKILL.Plugin.Utilities;

namespace VRTRAKILL.Plugin.Systems;

[DefaultExecutionOrder(1000)]
public class VRCameraController : MonoSingleton<VRCameraController>
{
    private ManualLogSource _log => Plugin.Instance.Log;
    private CameraController _cc => CameraController.Instance;
    private PlatformerMovement _pm => PlatformerMovement.Instance;

    public Camera LeftEye { get; private set; }

    public Camera RightEye { get; private set; }

    public Camera UICamera { get; private set; }

    public Transform DefaultParent => _cc.transform.parent;

    protected void Start()
    {
        if (_cc == null || _cc.cam == null)
        {
            _log.LogError("CameraController is not initialized yet. Cannot configure cameras.");
            return;
        }

        // it's disabled but still used as a template
        _cc.cam.nearClipPlane = .01f;
        _cc.cam.stereoTargetEye = StereoTargetEyeMask.None;
        _cc.cam.cullingMask |= 1 << (int)Layers.AlwaysOnTop;

        Validate();
    }

    // yeah yeah everything is inside of Update whatever
    // i'm making this shit bulletproof

    protected void LateUpdate()
    {
        if (_cc == null || _cc.cam == null)
            return;

        Validate();

        var head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        if (head == null) return;

        head.TryGetFeatureValue(CommonUsages.centerEyePosition, out var headPos);
        head.TryGetFeatureValue(CommonUsages.centerEyeRotation, out var headRot);
        head.TryGetFeatureValue(CommonUsages.leftEyePosition, out var leftEyePos);
        head.TryGetFeatureValue(CommonUsages.rightEyePosition, out var rightEyePos);

        if (_cc.platformerCamera)
        {
            // reparents it to clashmode cam's parent
            PlaceEye(LeftEye.transform, _pm.platformerCamera, leftEyePos - headPos, headRot);
            PlaceEye(RightEye.transform, _pm.platformerCamera, rightEyePos - headPos, headRot);
        }
        else
        {
            // reparents it to cc's parent
            PlaceEye(LeftEye.transform, _cc.transform.parent, leftEyePos - headPos, headRot);
            PlaceEye(RightEye.transform, _cc.transform.parent, rightEyePos - headPos, headRot);
        }
        
        // portal movement goes here
    }

    protected void Validate()
    {
        if (LeftEye == null)
        {
            var c = AddChildCamera("Left eye camera", _cc.cam);
            c.stereoTargetEye = StereoTargetEyeMask.Left;
        }

        if (RightEye == null)
        {
            var c = AddChildCamera("Right eye camera", _cc.cam);
            c.stereoTargetEye = StereoTargetEyeMask.Right;
        }

        if (UICamera == null)
        {

        }
    }

    protected Camera AddChildCamera(string name, Camera parent)
    {
        var cam = new GameObject(name).EnsureComponent<Camera>();
        cam.CopyFrom(parent);
        return cam;
    }

    protected void PlaceEye(Transform eye, Transform origin, Vector3 offset, Quaternion rotation)
    {
        if (eye.parent != origin) eye.SetParent(origin);
        eye.SetPositionAndRotation(origin.TransformPoint(offset), origin.rotation * rotation);
    }
}
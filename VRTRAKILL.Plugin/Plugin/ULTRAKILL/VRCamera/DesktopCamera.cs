using UnityEngine;
using VRBasePlugin.Util;

namespace VRBasePlugin.ULTRAKILL.VRCamera
{
    internal class DesktopCamera : MonoBehaviour
    {
        Camera C;
        public void Start() { C = gameObject.GetComponent<Camera>(); }
        public void Update()
        {
            C.nearClipPlane = Vars.MainCamera.nearClipPlane;
            C.farClipPlane = Vars.MainCamera.farClipPlane;
            C.depth = 69;
            C.stereoTargetEye = StereoTargetEyeMask.None;
            C.backgroundColor = Vars.MainCamera.backgroundColor;
            C.cullingMask = Vars.MainCamera.cullingMask;
            C.clearFlags = Vars.MainCamera.clearFlags;
            C.fieldOfView = Vars.Config.DesktopView.WorldCamFOV;
        }
    }
}

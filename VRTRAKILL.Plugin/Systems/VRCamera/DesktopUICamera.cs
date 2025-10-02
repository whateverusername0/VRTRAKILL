using UnityEngine;
using Plugin.Util;
using Plugin.Data;

namespace Plugin.Systems.VRCamera
{
    internal class DesktopUICamera : MonoBehaviour
    {
        Camera C;
        public void Start() { C = gameObject.GetComponent<Camera>(); }
        public void Update()
        {
            C.nearClipPlane = 0.1f;
            C.depth = 70;
            C.stereoTargetEye = StereoTargetEyeMask.None;
            C.cullingMask = Vars.UICamera.cullingMask;
            C.clearFlags = Vars.UICamera.clearFlags;
            C.fieldOfView = Vars.Config.DesktopView.UICamFOV;
        }
    }
}

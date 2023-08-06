using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class DesktopUICamera : MonoBehaviour
    {
        Camera C;
        public void Start() { C = gameObject.GetComponent<Camera>(); }
        public void Update()
        {
            transform.parent = Vars.UICamera.transform;
            transform.localPosition = Vector3.zero;
            C.nearClipPlane = 0.1f;
            C.depth = 70;
            C.stereoTargetEye = StereoTargetEyeMask.None;
            C.cullingMask = Vars.UICamera.cullingMask;
            C.clearFlags = Vars.UICamera.clearFlags;
            C.fieldOfView = Vars.Config.DesktopView.UICamFOV;
        }
    }
}

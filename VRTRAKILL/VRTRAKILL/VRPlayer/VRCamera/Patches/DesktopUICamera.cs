using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    internal class DesktopUICamera : MonoBehaviour
    {
        Camera C;
        public void Start() { C = gameObject.GetComponent<Camera>(); }
        public void Update()
        {
            transform.parent = Vars.VRUICamera.transform;
            transform.localPosition = Vector3.zero;
            C.nearClipPlane = 0.1f;
            C.depth = 70;
            C.stereoTargetEye = StereoTargetEyeMask.None;
            C.cullingMask = Vars.VRUICamera.cullingMask;
            C.clearFlags = Vars.VRUICamera.clearFlags;
            C.fieldOfView = Vars.Config.VRSettings.DV.UICamFOV;
        }
    }
}

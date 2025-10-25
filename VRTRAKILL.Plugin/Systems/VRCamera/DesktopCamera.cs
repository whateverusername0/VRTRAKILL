using UnityEngine;
using VRTRAKILL.Data;

namespace VRTRAKILL.Systems.VRCamera
{
    internal class DesktopCamera : MonoBehaviour
    {
        Camera C;
        public void Start() { C = gameObject.GetComponent<Camera>(); }
        public void Update()
        {
            C.nearClipPlane = Camera.main.nearClipPlane;
            C.farClipPlane = Camera.main.farClipPlane;
            C.depth = 69;
            C.stereoTargetEye = StereoTargetEyeMask.None;
            C.backgroundColor = Camera.main.backgroundColor;
            C.cullingMask = Camera.main.cullingMask;
            C.clearFlags = Camera.main.clearFlags;
            C.fieldOfView = Vars.Config.DesktopView.WorldCamFOV;
        }
    }
}

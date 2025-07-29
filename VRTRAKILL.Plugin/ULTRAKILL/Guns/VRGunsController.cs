using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Guns
{
    internal class VRGunsController : MonoSingleton<VRGunsController>
    {
        public Camera TargeterCamera;

        public void Start()
        {
            if (TargeterCamera == null || TargeterCamera is null)
            {
                TargeterCamera = new GameObject("TargeterCamera").AddComponent<Camera>();
                TargeterCamera.enabled = false;
                TargeterCamera.stereoTargetEye = StereoTargetEyeMask.None;
                TargeterCamera.clearFlags = CameraClearFlags.Nothing;
                TargeterCamera.depth = 99;
                TargeterCamera.fieldOfView = 90;
                TargeterCamera.nearClipPlane = .01f;
                TargeterCamera.cullingMask = -1;

                TargeterCamera.transform.parent = transform;
                TargeterCamera.transform.localPosition = Vector3.zero;
                TargeterCamera.transform.localEulerAngles = Vector3.zero;
            }
            CameraFrustumTargeter.Instance.camera = TargeterCamera;
        }

        public void Update()
        {
            TargeterCamera.transform.position = Vars.DominantHand.transform.position;
            transform.position = Vars.DominantHand.transform.position;
            if ((bool)CameraFrustumTargeter.Instance?.isActiveAndEnabled && CameraFrustumTargeter.IsEnabled && (bool)CameraFrustumTargeter.Instance?.CurrentTarget)
                return;
            transform.rotation = Vars.DominantHand.transform.rotation;
        }
    }
}

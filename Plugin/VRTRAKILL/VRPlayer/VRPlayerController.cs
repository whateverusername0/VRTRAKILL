using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer
{
    internal class VRPlayerController : MonoSingleton<VRPlayerController>
    {
        SteamVR_Camera VRCam;
        Rigidbody RB;

        public void Start()
        {
            VRCam = CameraController.Instance.GetComponent<SteamVR_Camera>();
            RB = NewMovement.Instance.GetComponent<Rigidbody>();
        }

        public void LateUpdate() // after newmovement.update
        {
            // make it follow the camera
            RB.position = new Vector3(VRCam.transform.position.x, RB.position.y, VRCam.transform.position.z);
        }
    }
}

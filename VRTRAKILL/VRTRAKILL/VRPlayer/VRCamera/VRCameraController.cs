using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoBehaviour
    {
        Camera DeskopCam;
        private void Start ()
        {
            transform.position -= new Vector3(0, 1f, 0);
            DeskopCam = new GameObject().AddComponent<Camera>();
            DeskopCam.gameObject.name = "DesktopView";
            DeskopCam.depth = 50; //50 so it dosn't fight with other cameras
            DeskopCam.stereoTargetEye = StereoTargetEyeMask.None;
        }

        private void Update()
        {
            DeskopCam.transform.rotation = Vars.MainCamera.transform.rotation;
            DeskopCam.transform.position = Vars.MainCamera.transform.position;
            // Smooth turn
            if (NewMovement.Instance.dead) return;
            NewMovement.Instance.gameObject.transform.rotation =
                Quaternion.Euler(NewMovement.Instance.transform.rotation.eulerAngles.x,
                                 Vars.MainCamera.transform.rotation.eulerAngles.y,
                                 NewMovement.Instance.transform.rotation.eulerAngles.z);

            transform.rotation = Quaternion.Euler(0f, Input.VRInputVars.TurnOffset, 0f);
        }
    }
}
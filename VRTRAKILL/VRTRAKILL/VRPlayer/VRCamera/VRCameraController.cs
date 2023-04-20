using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoBehaviour
    {
        private void OnEnable()
        {

        }

        private void Update()
        {
            if (CameraController.Instance.activated || CameraController.Instance.enabled)
                { CameraController.Instance.activated = false; CameraController.Instance.enabled = false; }

            // Smooth turn
            if (NewMovement.Instance.dead) return;
            NewMovement.Instance.gameObject.transform.rotation =
                Quaternion.Euler(NewMovement.Instance.transform.rotation.eulerAngles.x,
                                 Vars.MainCamera.transform.rotation.eulerAngles.y,
                                 NewMovement.Instance.transform.rotation.eulerAngles.z);

            this.transform.rotation = Quaternion.Euler(0f, Input.VRInputVars.TurnOffset, 0f);
        }
    }
}

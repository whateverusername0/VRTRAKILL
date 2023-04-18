using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoBehaviour
    {
        // Camera getting posessed fix (also CLEAAAAAAAAN)
        private void Start()
        {
            NewMovement.Instance.gameObject.transform.eulerAngles = this.gameObject.transform.eulerAngles;
            NewMovement.Instance.gameObject.transform.forward = this.gameObject.transform.forward;
        }

        private void Update()
        {
            // Smooth turn
            if (NewMovement.Instance.dead) return;
            NewMovement.Instance.gameObject.transform.rotation =
                Quaternion.Euler(NewMovement.Instance.transform.rotation.eulerAngles.x,
                                 Vars.MainCamera.transform.rotation.eulerAngles.y,
                                 NewMovement.Instance.transform.rotation.eulerAngles.z);

            Vars.VRCameraContainer.transform.rotation = Quaternion.Euler(0f, Input.VRInputVars.TurnOffset, 0f);
        }
    }
}

using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoSingleton<VRCameraController>
    {
        private void Start ()
        {
            // Height prevention
            transform.position -= new Vector3(0, 1f, 0);
        }

        private void Update()
        {
            // Follow MC rotation
            if (NewMovement.Instance.dead) return;
            NewMovement.Instance.gameObject.transform.rotation =
                Quaternion.Euler(NewMovement.Instance.transform.rotation.eulerAngles.x,
                                 Vars.MainCamera.transform.rotation.eulerAngles.y,
                                 NewMovement.Instance.transform.rotation.eulerAngles.z);

            if (Vars.Config.VRInputSettings.SnapTurning)
                StartCoroutine(SnapTurn());
            else transform.rotation = Quaternion.Euler(0f, Input.VRInputVars.TurnOffset, 0f);
        }
        private IEnumerator SnapTurn()
        {
            transform.rotation = Quaternion.Euler(0f, Input.VRInputVars.TurnOffset, 0f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
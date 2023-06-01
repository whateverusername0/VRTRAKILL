using UnityEngine;
using System.Collections;
using Plugin.VRTRAKILL.Input;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoSingleton<VRCameraController>
    {
        public void Start()
        {
            if (Vars.Config.VRInputSettings.SnapTurning)
                StartCoroutine(SnapTurn());
            else StartCoroutine(SmoothTurn());
        }

        public void Update()
        {
            // Follow MC rotation
            if (NewMovement.Instance.dead) return;
            NewMovement.Instance.gameObject.transform.rotation =
                Quaternion.Euler(NewMovement.Instance.transform.rotation.eulerAngles.x,
                                 Vars.MainCamera.transform.rotation.eulerAngles.y,
                                 NewMovement.Instance.transform.rotation.eulerAngles.z);

            transform.rotation = Quaternion.Euler(0f, VRInputVars.TurnOffset, 0f);
        }

        private IEnumerator SmoothTurn()
        {
            while(true)
            {
                if (VRInputVars.TurnVector.x > 0 + Vars.Config.VRInputSettings.Deadzone)
                    VRInputVars.TurnOffset += Vars.Config.VRInputSettings.SmoothTurningSpeed * Time.deltaTime;
                if (VRInputVars.TurnVector.x < 0 - Vars.Config.VRInputSettings.Deadzone)
                    VRInputVars.TurnOffset -= Vars.Config.VRInputSettings.SmoothTurningSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        private IEnumerator SnapTurn()
        {
            bool IsTurning;
            while (true)
            {
                if (VRInputVars.TurnVector.x > 0 + Vars.Config.VRInputSettings.Deadzone
                    || VRInputVars.TurnVector.x < 0 - Vars.Config.VRInputSettings.Deadzone) IsTurning = true;
                else IsTurning = false;

                while (IsTurning)
                {
                    if (VRInputVars.TurnVector.x > 0 + Vars.Config.VRInputSettings.Deadzone)
                        VRInputVars.TurnOffset += Vars.Config.VRInputSettings.SnapTurningAngles;
                    else if (VRInputVars.TurnVector.x < 0 - Vars.Config.VRInputSettings.Deadzone)
                        VRInputVars.TurnOffset -= Vars.Config.VRInputSettings.SnapTurningAngles;

                    // alternative to wait for seconds but you can actually cancel it
                    for (float i = .2f; i <= 0; i -= Time.deltaTime) continue;
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using Plugin.VRTRAKILL.Input;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoSingleton<VRCameraController>
    {
        public void Start()
        {
            if (Vars.Config.Input.InputSettings.SnapTurning)
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

            transform.rotation = Quaternion.Euler(0f, InputVars.TurnOffset, 0f);
        }

        private IEnumerator SmoothTurn()
        {
            while(true)
            {
                if (InputVars.TurnVector.x > 0 + Vars.Config.Input.InputSettings.Deadzone)
                    InputVars.TurnOffset += Vars.Config.Input.InputSettings.SmoothTurningSpeed * Time.deltaTime;
                if (InputVars.TurnVector.x < 0 - Vars.Config.Input.InputSettings.Deadzone)
                    InputVars.TurnOffset -= Vars.Config.Input.InputSettings.SmoothTurningSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        private IEnumerator SnapTurn()
        {
            bool IsTurning;
            while (true)
            {
                if (InputVars.TurnVector.x > 0 + Vars.Config.Input.InputSettings.Deadzone
                    || InputVars.TurnVector.x < 0 - Vars.Config.Input.InputSettings.Deadzone) IsTurning = true;
                else IsTurning = false;

                while (IsTurning)
                {
                    if (InputVars.TurnVector.x > 0 + Vars.Config.Input.InputSettings.Deadzone)
                        InputVars.TurnOffset += Vars.Config.Input.InputSettings.SnapTurningAngles;
                    else if (InputVars.TurnVector.x < 0 - Vars.Config.Input.InputSettings.Deadzone)
                        InputVars.TurnOffset -= Vars.Config.Input.InputSettings.SnapTurningAngles;

                    // alternative to wait for seconds but you can actually cancel it
                    for (float i = .2f; i <= 0; i -= Time.deltaTime) continue;
                }
            }
        }
    }
}
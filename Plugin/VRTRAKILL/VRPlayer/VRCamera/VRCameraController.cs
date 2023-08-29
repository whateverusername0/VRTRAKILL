using UnityEngine;
using System.Collections;
using Plugin.VRTRAKILL.Input;
using Valve.VR.InteractionSystem;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoSingleton<VRCameraController>
    {
        public void Start()
        {
            if (Vars.Config.Controllers.SnapTurn)
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
                if (InputVars.TurnVector.x > 0 + Vars.Config.Controllers.Deadzone)
                    InputVars.TurnOffset += Vars.Config.Controllers.SmoothSpeed * Time.deltaTime;
                if (InputVars.TurnVector.x < 0 - Vars.Config.Controllers.Deadzone)
                    InputVars.TurnOffset -= Vars.Config.Controllers.SmoothSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private bool IsTurning;
        private IEnumerator SnapTurn()
        {
            while (true)
            {
                if (InputVars.TurnVector.x > 0 + Vars.Config.Controllers.Deadzone
                || InputVars.TurnVector.x < 0 - Vars.Config.Controllers.Deadzone) IsTurning = true;
                else IsTurning = false;

                if (IsTurning)
                {
                    if (InputVars.TurnVector.x > 0 + Vars.Config.Controllers.Deadzone)
                        InputVars.TurnOffset += Vars.Config.Controllers.SnapAngles;
                    else if (InputVars.TurnVector.x < 0 - Vars.Config.Controllers.Deadzone)
                        InputVars.TurnOffset -= Vars.Config.Controllers.SnapAngles;

                    for (float i = .2f; i <= 0; i -= Time.deltaTime)
                        if (InputVars.TurnVector.x == 0) break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
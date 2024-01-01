using UnityEngine;
using System.Collections;
using VRBasePlugin.ULTRAKILL.Input;
using Valve.VR.InteractionSystem;
using Valve.VR;

namespace VRBasePlugin.ULTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoSingleton<VRCameraController>
    {
        Vector2 TurnVector; float TurnOffset;

        public void Update()
        {
            TurnVector = InputVars.TurnVector;
            TurnOffset = InputVars.TurnOffset;

            if (Vars.Config.Controllers.SnapTurn) StartCoroutine(SnapTurn());
            else StartCoroutine(SmoothTurn());

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
                if (InputVars.TurnVector.x > 0 + Vars.Config.Controllers.Deadzone)
                    InputVars.TurnOffset += Vars.Config.Controllers.SmoothSpeed * Time.deltaTime;
                if (InputVars.TurnVector.x < 0 - Vars.Config.Controllers.Deadzone)
                    InputVars.TurnOffset -= Vars.Config.Controllers.SmoothSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
        }

        private bool IsTurning; private float SnapTurnTimer;
        private IEnumerator SnapTurn()
        {
                if (IsTurning)
                {
                    SnapTurnTimer += Time.deltaTime;
                    if (SnapTurnTimer >= .2f || InputVars.TurnVector.x == 0) { IsTurning = false; SnapTurnTimer = 0; }
                }
                else
                {
                    if (InputVars.TurnVector.x > 0 + Vars.Config.Controllers.Deadzone)
                    { IsTurning = true; InputVars.TurnOffset += Vars.Config.Controllers.SnapAngles; }
                    else if (InputVars.TurnVector.x < 0 - Vars.Config.Controllers.Deadzone)
                    { IsTurning = true; InputVars.TurnOffset -= Vars.Config.Controllers.SnapAngles; }
                }
                yield return new WaitForEndOfFrame();
        }
    }
}
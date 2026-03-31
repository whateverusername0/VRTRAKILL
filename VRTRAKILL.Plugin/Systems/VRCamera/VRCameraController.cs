using UnityEngine;
using System.Collections;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.Input;

namespace VRTRAKILL.Systems.VRCamera;

public class VRCameraController : MonoSingleton<VRCameraController>
{
    public float TurnOffset = 0;

    public void Update()
    {
        if (GlobalVars.Config.Controllers.SnapTurn) StartCoroutine(SnapTurn());
        else StartCoroutine(SmoothTurn());

        // Follow MC rotation
        if (NewMovement.Instance.dead) return;
        NewMovement.Instance.gameObject.transform.rotation =
            Quaternion.Euler(NewMovement.Instance.transform.rotation.eulerAngles.x,
                             GlobalVars.MainCamera.transform.rotation.eulerAngles.y,
                             NewMovement.Instance.transform.rotation.eulerAngles.z);

        transform.rotation = Quaternion.Euler(0f, TurnOffset, 0f);
    }

    private IEnumerator SmoothTurn()
    {
            if (SteamVRPlayerInput.LookVector.x > 0 + GlobalVars.Config.Controllers.Deadzone)
                TurnOffset += GlobalVars.Config.Controllers.SmoothSpeed * Time.deltaTime;
            if (SteamVRPlayerInput.LookVector.x < 0 - GlobalVars.Config.Controllers.Deadzone)
                TurnOffset -= GlobalVars.Config.Controllers.SmoothSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
    }

    private bool IsTurning; private float SnapTurnTimer;
    private IEnumerator SnapTurn()
    {
            if (IsTurning)
            {
                SnapTurnTimer += Time.deltaTime;
                if (SnapTurnTimer >= .2f || SteamVRPlayerInput.LookVector.x == 0) { IsTurning = false; SnapTurnTimer = 0; }
            }
            else
            {
                if (SteamVRPlayerInput.LookVector.x > 0 + GlobalVars.Config.Controllers.Deadzone)
                { IsTurning = true; TurnOffset += GlobalVars.Config.Controllers.SnapAngles; }
                else if (SteamVRPlayerInput.LookVector.x < 0 - GlobalVars.Config.Controllers.Deadzone)
                { IsTurning = true; TurnOffset -= GlobalVars.Config.Controllers.SnapAngles; }
            }
            yield return new WaitForEndOfFrame();
    }
}
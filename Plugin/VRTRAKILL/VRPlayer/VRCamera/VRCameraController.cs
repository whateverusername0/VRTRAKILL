using UnityEngine;
using System.Collections;
using Plugin.VRTRAKILL.Input;
using Valve.VR.InteractionSystem;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class VRCameraController : MonoSingleton<VRCameraController>
    {
        private Vector3 LastHeadPos;
        private bool Faded;

        private int DetectHit(Vector3 Pos)
        {
            int Hits = 0;
            Collider[] Things = Physics.OverlapSphere(Pos, .2f, 1 << (int)Layers.Environment, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < Things.Length; i++) Hits++;
            return Hits;
        }

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

            if (DetectHit(transform.position) > 0)
            {

            }
            else
            {
                LastHeadPos = transform.position;
            }
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
        
        private IEnumerator SnapTurn()
        {
            bool IsTurning;
            while (true)
            {
                if (InputVars.TurnVector.x > 0 + Vars.Config.Controllers.Deadzone
                    || InputVars.TurnVector.x < 0 - Vars.Config.Controllers.Deadzone) IsTurning = true;
                else IsTurning = false;

                while (IsTurning)
                {
                    if (InputVars.TurnVector.x > 0 + Vars.Config.Controllers.Deadzone)
                        InputVars.TurnOffset += Vars.Config.Controllers.SnapAngles;
                    else if (InputVars.TurnVector.x < 0 - Vars.Config.Controllers.Deadzone)
                        InputVars.TurnOffset -= Vars.Config.Controllers.SnapAngles;

                    // alternative to wait for seconds but you can actually cancel it
                    for (float i = .2f; i <= 0; i -= Time.deltaTime) continue;
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
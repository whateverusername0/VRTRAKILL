using UnityEngine;
using System.Collections;

namespace VRBasePlugin.ULTRAKILL.VRPlayer.Controllers
{
    public class ArmController : MonoSingleton<ArmController>
    {
        public ControllerController CC;
        public GameObject GunOffset;

        public Vector3 ArmIKOffset = new Vector3(0, .05f, -.11f);

        private Vector3 _PreviousPosition, _CurrentVelocity; public float Speed = 0; // for punch detection
        private Vector3 LastPosition, Velocity; // for direction
        // note: do not fucking delete this
        private IEnumerator CalculateSpeed()
        {
            _PreviousPosition = transform.position;

            yield return new WaitForEndOfFrame();

            _CurrentVelocity = (_PreviousPosition - transform.position) / Time.deltaTime;
            Speed = _CurrentVelocity.magnitude;
        }

        public override void Awake()
        {
            base.Awake();
            CC = gameObject.GetComponent<ControllerController>();
            GunOffset = CC.GunOffset;
            LastPosition = transform.position;

            if (Vars.Config.UIInteraction.ControllerBased)
                UI.UIConverter.UIEventCamera.transform.parent = Vars.NonDominantHand.transform;
        }

        public void Update()
        {
            CC.ArmOffset.transform.localPosition = ArmIKOffset;

            StartCoroutine(CalculateSpeed());

            if (LastPosition != transform.position)
            {
                Velocity = (transform.position - LastPosition).normalized;
                Arms.Patches.PunchP._Thing = Velocity;
                LastPosition = transform.position;
            }

            SetControllers();
        }

        private void SetControllers()
        {
            if ((FistControl.Instance?.spawnedArms.Count == 0 && !Vars.IsPlayerFrozen)
            || ((bool)!FistControl.Instance?.activated && FistControl.Instance?.spawnedArms.Count == 0)
            || Vars.IsMainMenu)
                CC.RenderModel.SetActive(true);
            else CC.RenderModel.SetActive(false);
        }
    }
}

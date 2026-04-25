using UnityEngine;
using System.Collections;
using VRTRAKILL.Data;
using VRTRAKILL.Patches.ULTRAKILL;

namespace VRTRAKILL.Systems.Controllers
{
    public class ArmsVRController : MonoSingleton<ArmsVRController>
    {
        public VRControllerController CC;
        public Transform GunOffset;

        public Vector3 ArmIKOffset = new(0, .05f, -.11f);

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

        public void Awake()
        {
            CC = gameObject.GetComponent<VRControllerController>();
            GunOffset = CC.GunOffset;
            LastPosition = transform.position;

            VRCanvasHelper.UIEventCamera.transform.parent = GlobalVars.NonDominantHand.transform;
        }

        public void Update()
        {
            CC.ArmOffset.transform.localPosition = ArmIKOffset;

            StartCoroutine(CalculateSpeed());

            if (LastPosition != transform.position)
            {
                Velocity = (transform.position - LastPosition).normalized;
                PatchPunch.Direction = Velocity;
                LastPosition = transform.position;
            }

            SetControllers();
        }

        private void SetControllers()
        {
            if ((FistControl.Instance?.spawnedArms.Count == 0 && !GlobalVars.IsPlayerFrozen)
            || ((bool)!FistControl.Instance?.activated && FistControl.Instance?.spawnedArms.Count == 0)
            || GlobalVars.IsMainMenu)
                CC.RenderModel.gameObject.SetActive(true);
            else CC.RenderModel.gameObject.SetActive(false);
        }
    }
}

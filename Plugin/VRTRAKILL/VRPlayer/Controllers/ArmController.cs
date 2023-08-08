using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    public class ArmController : MonoSingleton<ArmController>
    {
        public ControllerController CC;
        public GameObject GunOffset;

        public Vector3 ArmOffset = new Vector3(0, .05f, -.11f);

        private Vector3 _PreviousPosition;
        private Vector3 _CurrentVelocity;
        public float Speed = 0;

        private IEnumerator CalculateVelocity()
        {
            _PreviousPosition = transform.position;

            yield return new WaitForEndOfFrame();

            _CurrentVelocity = (_PreviousPosition - transform.position) / Time.deltaTime;
            Speed = _CurrentVelocity.magnitude;
        }

        public void Start()
        {
            CC = gameObject.GetComponent<ControllerController>();
            GunOffset = CC.GunOffset;
        }

        public void Update()
        {
            CC.ArmOffset.transform.localPosition = ArmOffset;
            StartCoroutine(CalculateVelocity());
        }
    }
}

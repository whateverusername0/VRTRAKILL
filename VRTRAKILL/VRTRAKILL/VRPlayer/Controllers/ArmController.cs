using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class ArmController : MonoSingleton<ArmController>
    {
        public ControllerController CC;
        public GameObject GunOffset;

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
            CC.ArmOffset.transform.localPosition = new Vector3(0, .1f, -.25f);
        }

        public void Update()
        {
            StartCoroutine(CalculateVelocity());
        }
    }
}

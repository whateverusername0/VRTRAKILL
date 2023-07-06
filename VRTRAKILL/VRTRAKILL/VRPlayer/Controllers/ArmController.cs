using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class ArmController : MonoSingleton<ArmController>
    {
        public GameObject GunOffset, UIOffset;


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
            GunOffset = gameObject.GetComponent<ControllerController>().GunOffset;
            UIOffset = gameObject.GetComponent<ControllerController>().UIOffset;
        }

        public void Update()
        {
            StartCoroutine(CalculateVelocity());
        }
    }
}

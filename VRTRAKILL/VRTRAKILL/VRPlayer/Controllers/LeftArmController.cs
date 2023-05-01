using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class LeftArmController : MonoSingleton<LeftArmController>
    {
        public GameObject Offset;
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
            Offset = gameObject.GetComponent<ControllerController>().Offset;
        }

        public void Update()
        {
            StartCoroutine(CalculateVelocity());
        }
    }
}

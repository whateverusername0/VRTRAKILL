using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class LeftArmController : MonoSingleton<LeftArmController>
    {
        public GameObject Offset = new GameObject("Offset");
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
            Offset.transform.parent = this.transform;
            Offset.transform.localPosition = Vector3.zero;
            Offset.transform.localRotation = Quaternion.Euler(45, 0, 0);
        }

        public void Update()
        {
            StartCoroutine(CalculateVelocity());
            if (Vars.Config.VRSettings.DrawControllerLines)
            {
                Debug.DrawLine(Offset.transform.position,
                               Offset.transform.position + Offset.transform.forward * 100,
                               Color.white, 0, false);
            }
        }
    }
}

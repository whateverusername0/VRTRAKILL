using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    public class ArmController : MonoSingleton<ArmController>
    {
        public ControllerController CC;
        public GameObject GunOffset;

        public Vector3 ArmOffset = new Vector3(0, .05f, -.11f);

        private Vector3 LastPosition, Velocity;
        public float Speed = 0;

        public void Start()
        {
            CC = gameObject.GetComponent<ControllerController>();
            GunOffset = CC.GunOffset;
            LastPosition = transform.position;
        }

        public void Update()
        {
            CC.ArmOffset.transform.localPosition = ArmOffset;
            if (LastPosition != transform.position)
            {
                Velocity = (transform.position - LastPosition).normalized;
                Arms.Patches.PunchP._Thing = Velocity;
                LastPosition = transform.position;
            }
        }
    }
}

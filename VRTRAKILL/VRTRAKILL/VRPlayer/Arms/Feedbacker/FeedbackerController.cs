using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Feedbacker
{
    internal class FeedbackerController : MonoSingleton<FeedbackerController>
    {
        public Transform PunchZoneT; public Armature Arm;

        private void UpdateTransform()
        {
            Arm.Hand.position = Vars.LeftController.transform.position;
            Arm.Hand.rotation = Vars.LeftController.transform.rotation;
        }

        void Start()
        {

        }
        void Update()
        {
            UpdateTransform();
        }
        // Override Animator shenanigans
        void LateUpdate()
        {
            Arm.UpperArm.localScale /= 100; Arm.Hand.localScale *= 100;
            UpdateTransform();
        }
    }
}

using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Feedbacker
{
    internal class FeedbackerController : MonoSingleton<FeedbackerController>
    {
        public Transform PunchZoneT; public Armature Arm;

        public void Start()
        {

        }
        public void Update()
        {
            Arm.Hand.position = Vars.LeftController.transform.position;
            Arm.Hand.rotation = Vars.LeftController.transform.rotation;
        }
    }
}

using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Feedbacker
{
    internal class FeedbackerController : MonoSingleton<FeedbackerController>
    {
        public Armature Arm;
        public Vector3 OffsetPosition = new Vector3(0, 0, 0), TotalPos = Vector3.zero;
        public Quaternion OffsetRotation = Quaternion.Euler(0, 180, 0);

        public void Start()
        {
            Arm.Hand.position = OffsetPosition;
            Arm.Hand.rotation = OffsetRotation;
        }

        public void LateUpdate()
        {
            TotalPos = Vars.LeftController.transform.position + OffsetPosition;

            Arm.Root.position = TotalPos;
            //Arm.Root.rotation = Vars.LeftController.transform.rotation;

            //Arm.Hand.position = TotalPos;
            Arm.Hand.rotation = Vars.LeftController.transform.rotation * OffsetRotation;
        }
    }
}

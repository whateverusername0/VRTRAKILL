using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Feedbacker
{
    internal class FeedbackerController : MonoSingleton<FeedbackerController>
    {
        private Vector3 ArmScale = new Vector3(0.5f, 0.5f, 0.5f);

        public Transform PunchZoneT = null;

        static Vector3 PreviousPosition; static Vector3 CurrentVelocity;
        public float Speed;

        IEnumerator CalculateVelocity()
        {
            PreviousPosition = Instance.transform.position;
            yield return new WaitForEndOfFrame();
            CurrentVelocity = (PreviousPosition - Instance.transform.position) / Time.deltaTime;
            Speed = CurrentVelocity.magnitude;
        }

        void Start()
        {
            while (Armature.Root == null) {}

            transform.parent = Armature.Root;
            transform.parent.localScale = ArmScale;

            // for now there's no full arm tracking planned so i did this thing
            // basically it moves the arm all the way to the shadow realm while maintaining original hand postion
            // so we get floating hands
            //Armature.UpperArm.transform.localPosition = new Vector3(0, -1000, 0);
            //Armature.Hand.transform.localPosition = new Vector3(0, 1000, 0);
        }

        void Update()
        {
            if (Armature.Root == null) return;

            Armature.UpperArm.transform.localRotation = new Quaternion(0, 0, 0, 0);

            Armature.Hand.position = Controllers.LeftArmController.Instance.Position;
            Armature.Hand.rotation = Controllers.LeftArmController.Instance.Rotation;

            if (PunchZoneT != null) PunchZoneT.position = Controllers.LeftArmController.Instance.Position;

            StartCoroutine(CalculateVelocity());
        }
    }
}

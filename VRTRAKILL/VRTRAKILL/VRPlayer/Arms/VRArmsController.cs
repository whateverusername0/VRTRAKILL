using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        public Armature Arm;
        public Vector3 OffsetPosition, TotalPos = Vector3.zero;
        public Quaternion OffsetRotation;

        public void Start()
        {
            switch (Arm.Type)
            {
                case ArmType.Feedbacker:
                    OffsetPosition = new Vector3(0, 0, 0);
                    OffsetRotation = Quaternion.Euler(-45, 180, 0);
                    break;
                case ArmType.Knuckleblaster:
                    OffsetPosition = new Vector3(0, 0, 0);
                    OffsetRotation = Quaternion.Euler(-45, 180, 0);
                    break;
                case ArmType.Whiplash:
                    OffsetPosition = new Vector3(0, 0, 0);
                    OffsetRotation = Quaternion.Euler(-45, 180, 0);
                    break;

                case ArmType.Spear:
                default:
                    Destroy(GetComponent<VRArmsController>()); break;
            }
        }

        public void Update() { LateUpdate(); }
        public void LateUpdate()
        {
            Arm.GameObjectT.position = Vars.LeftController.transform.position;
            Arm.Hand.position += OffsetPosition;
            Arm.Hand.rotation = Vars.LeftController.transform.rotation * OffsetRotation;
        }
    }
}

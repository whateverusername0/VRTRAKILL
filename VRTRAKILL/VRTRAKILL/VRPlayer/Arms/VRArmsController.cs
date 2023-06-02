using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        public Armature Arm;
        public Vector3 OffsetPosition;
        public Quaternion OffsetRotation = Quaternion.Euler(-45, 180, 0);
        public Vector3? HookOffsetPosition = null;

        public void Start()
        {
            switch (Arm.Type)
            {
                case ArmType.Feedbacker:
                    OffsetPosition = new Vector3(0, -.5f, -.5f); break;
                case ArmType.Knuckleblaster:
                    OffsetPosition = new Vector3(0, -.0175f, -.0175f); break;
                case ArmType.Whiplash:
                    OffsetPosition = new Vector3(.145f, .08f, .05f);
                    HookOffsetPosition = new Vector3(0, 0, 0);
                    break;

                case ArmType.Spear:
                default: Destroy(GetComponent<VRArmsController>()); break;
            }
        }

        public void Update() { LateUpdate(); }
        public void LateUpdate()
        {
            Arm.GameObjectT.position = Vars.LeftController.transform.position;
            Arm.GameObjectT.rotation = Vars.LeftController.transform.rotation;
            Arm.Root.localPosition = OffsetPosition;
            Arm.Hand.rotation = Vars.LeftController.transform.rotation * OffsetRotation;

            if (HookOffsetPosition != null) {  }
        }
    }
}

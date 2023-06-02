using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        public Armature Arm;
        public Vector3 OffsetPosition = new Vector3(0, 0, 0), TotalPos = Vector3.zero;
        public Quaternion OffsetRotation = Quaternion.Euler(-45, 180, 0);

        public void Start()
        {

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

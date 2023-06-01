using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.GrapplingHook
{
    internal class WhiplashController : MonoBehaviour
    {
        public Armature Arm;
        public Vector3 OffsetPosition = new Vector3(0, 0, 0), TotalPos = Vector3.zero;
        public Quaternion OffsetRotation = Quaternion.Euler(0, 0, 0);

        public void Start()
        {

        }

        public void Update() { LateUpdate(); }
        public void LateUpdate()
        {

        }
    }
}

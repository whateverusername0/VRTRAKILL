using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Gestures
{
    internal class GesturesController : MonoBehaviour
    {
        public VRIK.Armature Arm;

        public bool
            Punch = false,
            ThumbsUp = false,
            Point = false,
            MiddleFinger = false;

        public void Start()
        {
            switch (Arm.Type)
            {
                case VRIK.ArmType.Feedbacker:
                    break;
                case VRIK.ArmType.Knuckleblaster:
                    break;

                case VRIK.ArmType.Whiplash:
                case VRIK.ArmType.Spear:
                default: Destroy(GetComponent<GesturesController>()); break;
            }
        }

        // Since I don't know how to use the animator and import custom assets, i'm gonna set everything manually
        public void LateUpdate()
        {

        }
    }
}
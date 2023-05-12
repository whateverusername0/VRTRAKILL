using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class ArmRemover : MonoBehaviour
    {
        Transform Armature; Feedbacker.Armature Arm;
        Vector3 ArmSize, HandSize;
        public void Start()
        {
            if (gameObject.HasComponent<Revolver>())
            {
                Armature = transform.GetChild(0);
                Arm = new Feedbacker.Armature(Armature);
                ArmSize = new Vector3(1, 1, 1); HandSize = new Vector3(100, 100, 100);
            }
            if (gameObject.HasComponent<Sandbox.Arm.SandboxArm>())
            {
                Armature = transform;
                Arm = new Feedbacker.Armature(Armature);
                ArmSize = new Vector3(.01f, .01f, .01f); HandSize = new Vector3(100, 100, 100);
            }
        }
        public void LateUpdate()
        {
            Arm.RArmature.localScale = ArmSize;
            Arm.Hand.localScale = HandSize;
        }
    }
}

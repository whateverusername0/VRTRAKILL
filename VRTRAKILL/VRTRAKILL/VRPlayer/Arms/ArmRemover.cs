using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class ArmRemover : MonoBehaviour
    {
        Transform Armature;
        Feedbacker.Armature FArm;
        Vector3 ArmSize, HandSize;
        public void Start()
        {
            if (gameObject.HasComponent<Revolver>())
            {
                Armature = transform.GetChild(0);
                FArm = new Feedbacker.Armature(Armature);
                ArmSize = new Vector3(1, 1, 1); HandSize = new Vector3(100, 100, 100);
            }
            if (gameObject.HasComponent<Sandbox.Arm.SandboxArm>())
            {
                Armature = transform;
                FArm = new Feedbacker.Armature(Armature);
                ArmSize = new Vector3(.01f, .01f, .01f); HandSize = new Vector3(10, 10, 10);
            }
        }
        public void LateUpdate()
        {
            FArm.RArmature.localScale = ArmSize;
            FArm.Hand.localScale = HandSize;
        }
    }
}

using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class ArmRemover : MonoBehaviour
    {
        public Armature Arm;
        Vector3 ArmSize, HandSize;
        public void Start()
        {
            if (gameObject.HasComponent<Revolver>())
            {
                Arm = new Armature(transform, ArmType.Feedbacker); // * special case * //
                ArmSize = new Vector3(1, 1, 1);
                HandSize = new Vector3(100, 100, 100);
            }
            else if (gameObject.HasComponent<Sandbox.Arm.SandboxArm>())
            {
                Arm = new Armature(transform, ArmType.Feedbacker, IsSandboxer: true); // * special case * //
                ArmSize = new Vector3(1, 1, 1);
                HandSize = new Vector3(100, 100, 100);
            }
            else if (gameObject.HasComponent<Punch>())
            {
                ArmSize = new Vector3(.01f, .01f, .01f);
                switch (GetComponent<Punch>().type)
                {
                    case FistType.Standard:
                        HandSize = new Vector3(35, 35, 35);
                        break;
                    case FistType.Heavy:
                        HandSize = new Vector3(35, 35, 35);
                        break;
                    case FistType.Spear: // unused in the game for now (i think??)
                        Destroy(GetComponent<ArmRemover>());
                        break;
                }
            }
            else if (gameObject.HasComponent<HookArm>())
            {
                ArmSize = new Vector3(.01f, .01f, .01f);
                HandSize = new Vector3(35, 35, 35);
            }
        }
        public void Update() { LateUpdate(); }
        public void LateUpdate()
        {
            if (Arm != null)
            {
                Arm.Root.localScale = ArmSize;
                Arm.Hand.localScale = HandSize;
            }
            if (gameObject.HasComponent<HookArm>())
                Arm.Wrist.GetChild(1).localScale = HandSize;
        }
    }
}

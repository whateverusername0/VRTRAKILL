using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class ArmRemover : MonoBehaviour
    {
        public VRIK.Armature Arm;
        Vector3 ArmSize, HandSize, GOSize;
        public void Start()
        {
            GOSize = transform.localScale;
            if (gameObject.HasComponent<Revolver>() || gameObject.HasComponent<FishingRodWeapon>())
            {
                Arm = VRIK.Armature.FeedbackerPreset(transform);
                ArmSize = new Vector3(1, 1, 1);
                HandSize = new Vector3(100, 100, 100);
            }
            else if (gameObject.HasComponent<Sandbox.Arm.SandboxArm>())
            {
                Arm = VRIK.Armature.SandboxerPreset(transform);
                ArmSize = new Vector3(1, 1, 1);
                HandSize = new Vector3(100, 100, 100);
            }
            else if (gameObject.HasComponent<Punch>())
            {
                ArmSize = new Vector3(.001f, .001f, .001f);
                switch (GetComponent<Punch>().type)
                {
                    case FistType.Standard:
                        HandSize = new Vector3(325, 325, 325);
                        break;
                    case FistType.Heavy:
                        HandSize = new Vector3(275, 275, 275);
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

            if (Vars.Config.Controllers.HandS.LeftHandMode)
                GOSize = new Vector3(GOSize.x * -1, GOSize.y, GOSize.z);
        }
        public void Update() { LateUpdate(); }
        public void LateUpdate()
        {
            if (Arm != null)
            {
                Arm.GameObjectT.localScale = GOSize;
                Arm.Root.localScale = ArmSize;
                Arm.Hand.localScale = HandSize;
            }
            if (gameObject.HasComponent<HookArm>())
                Arm.Wrist.GetChild(1).localScale = HandSize;
        }
    }
}

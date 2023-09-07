using UnityEngine;
using Plugin.Util;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class ArmTransformer : MonoBehaviour
    {
        public Arm Arm;
        Vector3 ArmSize, HandSize;

        public void Start()
        {
            if (gameObject.HasComponent<Revolver>() || gameObject.HasComponent<FishingRodWeapon>())
            {
                Arm = Arm ?? Arm.FeedbackerPreset(transform);
                ArmSize = new Vector3(1, 1, 1);
                HandSize = new Vector3(100, 100, 100);
            }
            else if (gameObject.HasComponent<Sandbox.Arm.SandboxArm>())
            {
                Arm = Arm ?? Arm.SandboxerPreset(transform);
                ArmSize = new Vector3(1, 1, 1);
                HandSize = new Vector3(100, 100, 100);
            }
            else if (gameObject.HasComponent<Punch>())
            {
                ArmSize = new Vector3(.001f, .001f, .001f);
                switch (GetComponent<Punch>().type)
                {
                    case FistType.Standard: HandSize = new Vector3(325, 325, 325); break;
                    case FistType.Heavy: HandSize = new Vector3(275, 275, 275); break;
                    case FistType.Spear:
                    default: Destroy(GetComponent<ArmTransformer>()); break;
                }
            }
            else if (gameObject.HasComponent<HookArm>())
            {
                ArmSize = new Vector3(.01f, .01f, .01f);
                HandSize = new Vector3(35, 35, 35);
                Transform HookOffset = new GameObject("Hook Offset").transform;
                HookOffset.SetParent(Arm.Hand.Root, false);
                HookOffset.localPosition = new Vector3(-.01f, -.035f, -.005f);
                HookOffset.localEulerAngles = new Vector3(0, 80, 0);
                Arm.Forearm.GetChild(1).SetParent(HookOffset, true);
            }

            if (Vars.Config.Controllers.LeftHanded)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        
        public void LateUpdate()
        {
            if (Arm != null)
            {
                Arm.GameObjecT.localScale = transform.localScale;
                Arm.Root.localScale = ArmSize;
                Arm.Hand.Root.localScale = HandSize;
            }
        }
    }
}

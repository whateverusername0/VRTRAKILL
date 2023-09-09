using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class ArmTransformer : MonoBehaviour
    {
        public Arm Arm;
        Vector3 ArmSize, HandSize;

        public void Start()
        {
            Component[] C = gameObject.GetComponents<Component>();
            foreach (Component _C in C)
                switch (_C)
                {
                    case Revolver _:
                    case FishingRodWeapon _:
                        {
                            Arm = Arm ?? Arm.FeedbackerPreset(transform);
                            ArmSize = new Vector3(1, 1, 1);
                            HandSize = new Vector3(100, 100, 100);
                            break;
                        }
                    case Sandbox.Arm.SandboxArm _:
                        {
                            Arm = Arm ?? Arm.SandboxerPreset(transform);
                            ArmSize = new Vector3(1, 1, 1);
                            HandSize = new Vector3(100, 100, 100);
                            break;
                        }
                    case Punch _:
                        {
                            ArmSize = new Vector3(.001f, .001f, .001f);
                            switch (GetComponent<Punch>().type)
                            {
                                case FistType.Standard:
                                    {
                                        Arm = Arm ?? Arm.FeedbackerPreset(transform);
                                        HandSize = new Vector3(325, 325, 325);
                                        break;
                                    }
                                case FistType.Heavy:
                                    {
                                        Arm = Arm ?? Arm.KnuckleblasterPreset(transform);
                                        HandSize = new Vector3(275, 275, 275);
                                        break;
                                    }
                                case FistType.Spear:
                                default: Destroy(GetComponent<ArmTransformer>()); break;
                            }
                            break;
                        }
                    case HookArm _:
                        {
                            Arm = Arm ?? Arm.WhiplashPreset(transform);
                            ArmSize = new Vector3(.01f, .01f, .01f);
                            HandSize = new Vector3(35, 35, 35);

                            // hook fix
                            Transform HookOffset = new GameObject("Hook Offset").transform;
                            HookOffset.SetParent(Arm.Hand.Root, false);
                            HookOffset.localPosition = new Vector3(.005f, -.025f, 0);
                            HookOffset.localEulerAngles = new Vector3(-5, 80, 0);
                            HookOffset.localScale = new Vector3(.95f, .95f, .95f);
                            Arm.Forearm.GetChild(1).SetParent(HookOffset, true);
                            break;
                        }
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

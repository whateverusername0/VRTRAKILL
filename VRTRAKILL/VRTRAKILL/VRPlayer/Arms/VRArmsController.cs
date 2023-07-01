using Plugin.Helpers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        // basic stuff
        public VRIK.Armature Arm;
        public Vector3 OffsetPosition = new Vector3(.145f, .09f, .04f); // pre set to fix whiplash
        public Quaternion OffsetRotation = Quaternion.Euler(-90, 180, 0);
        public bool IsSandboxer = false, IsRevolver = false;

        // used for punching
        public Vector3 LastPosition, Velocity;

        // vrik stuff
        public bool UseVRIK = false;
        public Vector3 VRIKShoulderPosition = Vector3.zero;
        public Vector3 VRIKArmScale = new Vector3(.325f, .325f, .325f);

        public void Start()
        {
            if (OffsetPosition == null || OffsetPosition == new Vector3(.145f, .09f, .04f))
            {
                switch (Arm.Type)
                {
                    case VRIK.ArmType.Feedbacker:
                        OffsetPosition = new Vector3(0, -.25f, -.5f); break;
                    case VRIK.ArmType.Knuckleblaster:
                        OffsetPosition = new Vector3(0, -.01f, -.035f); break;
                    case VRIK.ArmType.Whiplash:
                        OffsetPosition = new Vector3(.145f, .09f, .04f); break;

                    case VRIK.ArmType.Spear:
                    default: Destroy(GetComponent<VRArmsController>()); break;
                }
            }

            LastPosition = transform.position;

            if (Vars.Config.Game.VRB.EnableVRIK)
            {
                UseVRIK = true;

                VRIK.IKArm IKArm = Arm.Hand.gameObject.AddComponent<VRIK.IKArm>();
                IKArm.ChainLength = 3;

                if (IsRevolver) IKArm.Target = Vars.DominantHand.transform;
                else IKArm.Target = Vars.NonDominantHand.transform;

                GetComponent<ArmRemover>().enabled = false;
            }
        }
        public void Update()
        {
            
        }
        public void FixedUpdate()
        {
            if (LastPosition != transform.position)
            {
                Velocity = (transform.position - LastPosition).normalized;
                LastPosition = transform.position;
            }
        }
        public void LateUpdate()
        {
            if (UseVRIK)
            {
                Arm.Root.localScale = VRIKArmScale;
            }

            try
            {
                // Update positions & rotations of the main gameobject + hand rotation (because animator stuff)
                if (IsSandboxer) MoveSandboxer();
                else if (IsRevolver) HandleRevolver();
                else MoveHand();

                HandleWhiplash();

            } catch {} // do nothing because i know that it 100% works :) (it gives out too many errors which zipbomb your storage)
        }

        private void MoveSandboxer()
        {
            if (!UseVRIK)
            {
                Arm.GameObjectT.position = Vars.DominantHand.transform.position;
                Arm.GameObjectT.rotation = Vars.DominantHand.transform.rotation;
            }
            Arm.Hand.position = Vars.DominantHand.transform.position;
            Arm.Hand.rotation = Vars.DominantHand.transform.rotation * OffsetRotation;
            Arm.Root.localPosition = OffsetPosition;
        }
        private void HandleRevolver()
        {
            if (GetComponent<Revolver>().anim.GetBool("Spinning"))
                Arm.Hand.rotation = Vars.DominantHand.transform.rotation * OffsetRotation;
        }
        private void MoveHand()
        {
            if (!UseVRIK)
            {
                Arm.GameObjectT.position = Vars.NonDominantHand.transform.position;
                Arm.GameObjectT.rotation = Vars.NonDominantHand.transform.rotation;
            }
            Arm.Root.localPosition = OffsetPosition;
            Arm.Hand.position = Vars.NonDominantHand.transform.position;
            Arm.Hand.rotation = Vars.NonDominantHand.transform.rotation * OffsetRotation;
        }
        private void HandleWhiplash()
        {
            if (gameObject.HasComponent<HookArm>())
                Arm.Wrist.GetChild(1).rotation = Vars.NonDominantHand.transform.rotation * OffsetRotation;

            // Thingamajig to disable other arms while grapplehooking
            if (HookArm.Instance.model.activeSelf && !gameObject.HasComponent<HookArm>()
                && !IsSandboxer && !gameObject.HasComponent<Revolver>())
                Arm.GameObjectT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            else Arm.GameObjectT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
    }
}

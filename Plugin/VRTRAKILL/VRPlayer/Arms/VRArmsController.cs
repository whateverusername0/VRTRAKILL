using Plugin.Helpers;
using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        // basic stuff
        public Arm Arm;
        public Vector3 OffsetPosition = new Vector3(.145f, .09f, .04f); // pre set to fix whiplash
        public Quaternion OffsetRotation = Quaternion.Euler(-90, 180, 0);
        public bool IsSandboxer = false, IsRevolver = false;

        // used for punching
        public Vector3 LastPosition, Velocity;

        public void Start()
        {
            if (OffsetPosition == null || OffsetPosition == new Vector3(.145f, .09f, .04f))
            {
                switch (Arm.Type)
                {
                    case ArmType.Feedbacker:
                        OffsetPosition = new Vector3(0, -.25f, -.5f); break;
                    case ArmType.Knuckleblaster:
                        OffsetPosition = new Vector3(0, -.01f, -.025f); break;
                    case ArmType.Whiplash:
                        OffsetPosition = new Vector3(.145f, .09f, .04f); break;

                    case ArmType.Spear:
                    default: Destroy(GetComponent<VRArmsController>()); break;
                }
            }
            LastPosition = transform.position;
        }
        public void FixedUpdate()
        {
            // velocity based punching
            if (LastPosition != transform.position)
            {
                Velocity = (transform.position - LastPosition).normalized;
                LastPosition = transform.position;
            }
        }
        public void LateUpdate()
        {
            try
            {
                // override positions & rotations of the main gameobject + hand rotation
                if (IsSandboxer) MoveSandboxer();
                else if (IsRevolver) HandleRevolver();
                else MoveHand();

                HandleWhiplash();

            } catch {} // it gives out too many errors which zipbomb your storage
        }

        private void MoveSandboxer()
        {
            Arm.GameObjecT.position = Vars.DominantHand.transform.position;
            Arm.GameObjecT.rotation = Vars.DominantHand.transform.rotation;

            Arm.Root.localPosition = OffsetPosition;
            Arm.Hand.Root.rotation = Vars.DominantHand.transform.rotation * OffsetRotation;
        }
        private void HandleRevolver()
        {
            if (GetComponent<Revolver>().anim.GetBool("Spinning"))
                Arm.Hand.Root.rotation = Vars.DominantHand.transform.rotation * OffsetRotation;
        }
        private void MoveHand()
        {
            Arm.GameObjecT.position = Vars.NonDominantHand.transform.position;
            Arm.GameObjecT.rotation = Vars.NonDominantHand.transform.rotation;

            Arm.Root.localPosition = OffsetPosition;
            Arm.Hand.Root.rotation = Vars.NonDominantHand.transform.rotation * OffsetRotation;
        }
        private void HandleWhiplash()
        {
            if (gameObject.HasComponent<HookArm>())
                Arm.Forearm.GetChild(1).rotation = Vars.NonDominantHand.transform.rotation * OffsetRotation;

            // Thingamajig to disable other arms while grapplehooking
            if (HookArm.Instance.model.activeSelf && !gameObject.HasComponent<HookArm>()
                && !IsSandboxer && !gameObject.HasComponent<Revolver>())
                Arm.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            else Arm.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
    }
}

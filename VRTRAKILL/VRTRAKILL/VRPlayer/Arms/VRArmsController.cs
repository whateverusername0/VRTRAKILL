using Plugin.Helpers;
using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRIK.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        // basic stuff
        public Armature Arm;
        public Vector3 OffsetPosition = new Vector3(.145f, .09f, .04f); // pre set to fix whiplash
        public Quaternion OffsetRotation = Quaternion.Euler(-90, 180, 0);
        public bool IsSandboxer = false, IsRevolver = false;

        // used for punching
        public Vector3 LastPosition, Velocity;

        // vrik stuff
        public bool UseVRIK = false;
        public Vector3 VRIKShoulderPosition = Vector3.zero;
        public Quaternion VRIKShoulderRotation = Quaternion.Euler(Vector3.zero);
        public Vector3 VRIKArmScale = new Vector3(.325f, .325f, .325f);

        public void Start()
        {
            if (OffsetPosition == null || OffsetPosition == new Vector3(.145f, .09f, .04f))
            {
                switch (Arm.Type)
                {
                    case ArmType.Feedbacker:
                        OffsetPosition = new Vector3(0, -.25f, -.5f);
                        VRIKArmScale = new Vector3(.325f, .325f, .325f);
                        break;
                    case ArmType.Knuckleblaster:
                        OffsetPosition = new Vector3(0, -.01f, -.035f);
                        VRIKArmScale = new Vector3(.325f, .325f, .325f);
                        break;
                    case ArmType.Whiplash:
                        OffsetPosition = new Vector3(.145f, .09f, .04f);
                        VRIKArmScale = new Vector3(.325f, .325f, .325f);
                        break;

                    case ArmType.Spear:
                    default: Destroy(GetComponent<VRArmsController>()); break;
                }
            }

            LastPosition = transform.position;

            if (Vars.Config.Game.VRB.EnableVRIK && VRIK.VRigController.Instance.Rig != null && !IsRevolver)
            {
                VRIK.IKArm IKArm = Arm.Wrist_End.gameObject.AddComponent<VRIK.IKArm>();
                IKArm.ChainLength = 3;
                IKArm.Target = Arm.Hand;

                GetComponent<ArmRemover>().enabled = false;
            }
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
                if (UseVRIK)
                {
                    if (IsSandboxer || IsRevolver) Arm.GameObjecT.position = VRIK.VRigController.Instance.Rig.RightArm.Clavicle.position;
                    else Arm.GameObjecT.position = VRIK.VRigController.Instance.Rig.LeftArm.Clavicle.position;
                }

                // override positions & rotations of the main gameobject + hand rotation
                if (IsSandboxer) MoveSandboxer();
                else if (IsRevolver) HandleRevolver();
                else MoveHand();

                HandleWhiplash();

            } catch {} // it gives out too many errors which zipbomb your storage
        }

        private void MoveSandboxer()
        {
            if (UseVRIK) Arm.Clavicle.position = VRIK.VRigController.Instance.Rig.RightArm.GameObjecT.position;
            else Arm.Clavicle.localPosition = OffsetPosition;
            Arm.Hand.position = Vars.DominantHand.transform.position;
            Arm.Hand.rotation = Vars.DominantHand.transform.rotation * OffsetRotation;
        }
        private void HandleRevolver()
        {
            if (GetComponent<Revolver>().anim.GetBool("Spinning"))
                Arm.Hand.rotation = Vars.DominantHand.transform.rotation * OffsetRotation;
        }
        private void MoveHand()
        {
            if (UseVRIK) Arm.Clavicle.position = VRIK.VRigController.Instance.Rig.LeftArm.GameObjecT.position;
            else Arm.Clavicle.localPosition = OffsetPosition;
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
                Arm.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            else Arm.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
    }
}

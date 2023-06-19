using Plugin.Helpers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoSingleton<VRArmsController>
    {
        public Armature Arm; public bool IsSandboxer = false;
        public Vector3 OffsetPosition = new Vector3(.145f, .09f, .04f); // hack to fix whiplash
        public Quaternion OffsetRotation = Quaternion.Euler(-90, 180, 0);
        public Vector3? HookOffsetPosition = null;

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
                        OffsetPosition = new Vector3(0, -.01f, -.035f); break;
                    case ArmType.Whiplash:
                        OffsetPosition = new Vector3(.145f, .09f, .04f);
                        HookOffsetPosition = new Vector3(0, 0, 0); break;

                    case ArmType.Spear:
                    default: Destroy(GetComponent<VRArmsController>()); break;
                }
            }

            LastPosition = transform.position;
        }

        public void Update()
        {
            if (LastPosition != transform.position)
            {
                Velocity = ((transform.position - LastPosition) / Time.deltaTime).normalized;
                LastPosition = transform.position;
            }
        }
        public void LateUpdate()
        {
            try
            {
                // Update positions & rotations of the main gameobject + hand rotation (because animator stuff)
                if (IsSandboxer || gameObject.HasComponent<Revolver>())
                {
                    Arm.GameObjectT.position = Vars.DominantHand.transform.position;
                    Arm.GameObjectT.rotation = Vars.DominantHand.transform.rotation;
                    Arm.Root.localPosition = OffsetPosition;
                    Arm.Hand.rotation = Vars.DominantHand.transform.rotation * OffsetRotation;
                }
                else
                {
                    Arm.GameObjectT.position = Vars.NonDominantHand.transform.position;
                    Arm.GameObjectT.rotation = Vars.NonDominantHand.transform.rotation;
                    Arm.Root.localPosition = OffsetPosition;
                    Arm.Hand.rotation = Vars.NonDominantHand.transform.rotation * OffsetRotation;
                }

                // Whiplash specific stuff
                if (gameObject.HasComponent<HookArm>())
                    Arm.Wrist.GetChild(1).rotation = Vars.NonDominantHand.transform.rotation * OffsetRotation;
                if (HookOffsetPosition != null) {  }

                // Thingamajig to disable other arms while grapplehooking
                if (HookArm.Instance.model.activeSelf && !gameObject.HasComponent<HookArm>()
                    && !IsSandboxer && !gameObject.HasComponent<Revolver>())
                    Arm.GameObjectT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                else Arm.GameObjectT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            } catch {} // do nothing because i know that it 100% works :) (it gives out too many errors which zipbomb your storage)
        }
    }
}

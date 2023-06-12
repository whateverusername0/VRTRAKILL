using Plugin.Helpers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        public Armature Arm; public bool IsSandboxer = false;
        public Vector3 OffsetPosition;
        public Quaternion OffsetRotation = Quaternion.Euler(-90, 180, 0);
        public Vector3? HookOffsetPosition = null;

        public void Start()
        {
            switch (Arm.Type)
            {
                case ArmType.Feedbacker:
                    OffsetPosition = new Vector3(0, -.25f, -.5f); break;
                case ArmType.Knuckleblaster:
                    OffsetPosition = new Vector3(0, 0, -.0175f); break;
                case ArmType.Whiplash:
                    OffsetPosition = new Vector3(.145f, .08f, .05f);
                    HookOffsetPosition = new Vector3(0, 0, 0); break;

                case ArmType.Spear:
                default: Destroy(GetComponent<VRArmsController>()); break;
            }
        }

        public void Update() { LateUpdate(); }
        public void LateUpdate()
        {
            try
            {
                // Update positions & rotations of the main gameobject + hand rotation (because animator stuff)
                if (IsSandboxer)
                {
                    Arm.GameObjectT.position = Vars.RightController.transform.position;
                    Arm.GameObjectT.rotation = Vars.RightController.transform.rotation;
                    Arm.Root.localPosition = OffsetPosition;
                    Arm.Hand.rotation = Vars.RightController.transform.rotation * OffsetRotation;
                }
                else
                {
                    Arm.GameObjectT.position = Vars.LeftController.transform.position;
                    Arm.GameObjectT.rotation = Vars.LeftController.transform.rotation;
                    Arm.Root.localPosition = OffsetPosition;
                    Arm.Hand.rotation = Vars.LeftController.transform.rotation * OffsetRotation;
                }

                // Whiplash specific stuff
                if (gameObject.HasComponent<HookArm>())
                    Arm.Wrist.GetChild(1).rotation = Vars.LeftController.transform.rotation * OffsetRotation;
                if (HookOffsetPosition != null) {  }

                // Thingamajig to disable other arms while grapplehooking
                if (HookArm.Instance.model.activeSelf && !gameObject.HasComponent<HookArm>())
                    Arm.GameObjectT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                else Arm.GameObjectT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            } catch {} // do nothing because i know that it 100% works :) (it gives out too many errors which zipbomb your storage)
        }
    }
}

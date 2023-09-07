using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.ArmController
{
    internal class ACBase : MonoBehaviour
    {
        public Arm Arm { get; private set; } public void SetArm(Arm A)
        {
            if (Arm != null)
            { if (A.Type == Arm.Type) Arm = A; }
            else Arm = A;
        }
        public Transform Target;
        public Vector3 OffsetPos = Vector3.zero, OffsetRot = new Vector3(-90, 180, 0);
        

        protected Vector3 ResolveOffsetPos()
        {
            switch (Arm.Type)
            {
                case ArmType.Feedbacker:     return new Vector3(0, -.25f, -.5f);
                case ArmType.Knuckleblaster: return new Vector3(0, -.01f, -.025f);
                case ArmType.Whiplash:       return new Vector3(.145f, .09f, .04f);

                case ArmType.Spear:
                default: return Vector3.zero;
            }
        }

        public virtual void Start()
        {
            if (Arm != null && OffsetPos == Vector3.zero) OffsetPos = ResolveOffsetPos();
        }
        public virtual void LateUpdate()
        {
            if (HookArm.Instance != null && HookArm.Instance.model != null && HookArm.Instance.model.activeSelf)
                Arm.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            else Arm.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
    }
}

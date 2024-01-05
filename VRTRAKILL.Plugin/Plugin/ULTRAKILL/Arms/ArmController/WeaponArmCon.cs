using VRTRAKILL.Utilities;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Arms.ArmController
{
    // For things that are "technically or not" weapons (revolver, fishing rod, etc.) and in need of a rotation locking
    internal class WeaponArmCon : ACBase
    {
        public override void Start()
        {
            base.Start();
            if (Vars.Config.Controllers.LeftHanded) Target = Vars.NonDominantHand.transform;
            else Target = Vars.DominantHand.transform;
        }
        public override void LateUpdate()
        {
            if (gameObject.HasComponent<Revolver>())
            {
                if (GetComponent<Revolver>().anim.GetBool("Spinning"))
                { Arm.Hand.Root.rotation = Target.rotation * Quaternion.Euler(OffsetRot); return; }
                return;
            }
            Arm.Hand.Root.rotation = Target.rotation * Quaternion.Euler(OffsetRot);
        }
    }
}

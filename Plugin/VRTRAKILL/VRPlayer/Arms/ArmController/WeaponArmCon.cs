using Plugin.Util;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.ArmController
{
    // For things that are "technically or not" weapons (revolver, fishing rod, etc.)
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
            //base.LateUpdate();
            if (gameObject.HasComponent<Revolver>() && GetComponent<Revolver>().anim.GetBool("Spinning"))
                Arm.Hand.Root.eulerAngles = (Vector3)(Target.eulerAngles + OffsetRot);
        }
    }
}

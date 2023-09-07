using Plugin.Util;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.ArmController
{
    internal class DefaultArmCon : ACBase
    {
        public override void Start()
        {
            base.Start();
            if (Vars.Config.Controllers.LeftHanded || gameObject.HasComponent<Sandbox.Arm.SandboxArm>())
                Target = Vars.DominantHand.transform;
            else Target = Vars.NonDominantHand.transform;
        }

        public override void LateUpdate()
        {
            if (!gameObject.HasComponent<HookArm>()) base.LateUpdate();
            Arm.GameObjecT.position = Target.position;
            Arm.GameObjecT.rotation = Target.rotation;

            Arm.Root.localPosition = (Vector3)OffsetPos;
            Arm.Hand.Root.eulerAngles = (Vector3)(Target.eulerAngles + OffsetRot);
        }
    }
}

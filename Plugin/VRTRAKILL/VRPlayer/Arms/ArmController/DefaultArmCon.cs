using Plugin.Util;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.ArmController
{
    internal class DefaultArmCon : ACBase
    {
        public Transform RawTarget { get; set; }
        public override void Start()
        {
            base.Start();
            if (Vars.Config.Controllers.LeftHanded || gameObject.HasComponent<Sandbox.Arm.SandboxArm>())
            { Target = Vars.DominantHand.transform; RawTarget = Vars.NDHC.transform; }
            else { Target = Vars.NonDominantHand.transform; RawTarget = Vars.DHC.transform; }
        }

        public override void LateUpdate()
        {
            if (Vars.IsMainMenu || Arm == null) { Vars.Log.LogWarning("No arm found!"); return; }

            if (!gameObject.HasComponent<HookArm>()) base.LateUpdate();
            Arm.GameObjecT.position = Target.position;
            Arm.GameObjecT.rotation = Target.rotation;

            Arm.Root.localPosition = OffsetPos;
            Arm.Hand.Root.rotation = Target.rotation * Quaternion.Euler(OffsetRot);
        }
    }
}

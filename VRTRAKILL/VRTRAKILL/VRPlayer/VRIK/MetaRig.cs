using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRIK.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    internal class MetaRig
    {
        public Transform GameObjectT { get; set; }
        public Transform Root { get; set; }
        public Transform Body { get; set; }
        public Transform Abdomen { get; set; }
        public Transform Chest { get; set; }
        public Transform Neck { get; set; }
        public Transform Head { get; set; }

        public Transform LShoulder { get; set; }
        public Arm LFeedbacker { get; set; }
        public Arm LKnuckleblaster { get; set; }
        public Arm LWhiplash { get; set; }

        public Transform RShoulder { get; set; }
        public Arm RFeedbacker { get; set; }
        public Arm RSandboxer { get; set; }

        public Leg LeftLeg { get; set; }
        public Leg RightLeg { get; set; }

        public static MetaRig V1CustomPreset(Transform T)
        {
            MetaRig MR = new MetaRig
            {
                GameObjectT = T, // V1
                Root = T.GetChild(1).GetChild(0) // Root
            };
            MR.Body = MR.Root.GetChild(2); // Spine
            MR.Abdomen = MR.Body.GetChild(0); // Abdomen
            MR.Chest = MR.Abdomen.GetChild(0); // Chest
            MR.Neck = MR.Chest.GetChild(1); // Neck
            MR.Head = MR.Neck.GetChild(0); // NeckHead

            MR.LShoulder = MR.Chest.GetChild(0); // LeftShoulder
            MR.LFeedbacker = Arm.MRFeedbackerPreset(MR.LShoulder.GetChild(0));
            MR.LKnuckleblaster = Arm.MRKnuckleblasterPreset(MR.LShoulder.GetChild(1));
            MR.LWhiplash = Arm.MRWhiplashPreset(MR.LShoulder.GetChild(2));

            MR.RShoulder = MR.Chest.GetChild(2); // RightShoulder
            MR.RFeedbacker = Arm.MRFeedbackerPreset(MR.RShoulder.GetChild(0));
            MR.RSandboxer = Arm.MRKnuckleblasterPreset(MR.RShoulder.GetChild(1));

            MR.LeftLeg = Leg.MRPreset(MR.Root.GetChild(0));
            MR.RightLeg = Leg.MRPreset(MR.Root.GetChild(1));
            return MR;
        }
        public static MetaRig CreateV1CustomPreset(GameObject Parent)
        {
            GameObject V1mdlGO = Object.Instantiate(Assets.Vars.V1Rig, Parent.transform, false);
            return V1CustomPreset(V1mdlGO.transform);
        }
    }
}

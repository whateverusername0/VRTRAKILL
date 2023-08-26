using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
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
        public Arm _LFeedbacker { get; set; }
        public Arm _LKnuckleblaster { get; set; }
        public Arm _LWhiplash { get; set; }
        public Arm _LSandboxer { get; set; }

        public Transform RShoulder { get; set; }
        public Arm _RFeedbacker { get; set; }
        public Arm _RKnuckleblaster { get; set; }
        public Arm _RWhiplash { get; set; }
        public Arm _RSandboxer { get; set; }

        public Arm FeedbackerA { get; set; } public Arm FeedbackerB { get; set; }
        public Arm Knuckleblaster { get; set; }
        public Arm Whiplash { get; set; }
        public Arm Sandboxer { get; set; }

        public Leg LeftLeg { get; set; }
        public Leg RightLeg { get; set; }

        public Transform IKPole_Left { get; set; }
        public Transform IKPole_Right { get; set; }

        // V stands for Model V (V1, V2), since they have identical armatures.
        public static MetaRig VCustomPreset(Transform T)
        {
            MetaRig MR = new MetaRig
            {
                GameObjectT = T, // V1
                Root = T.GetChild(1) // Armature
            };
            MR.Body = MR.Root.GetChild(0).GetChild(0).GetChild(0); // Body
            MR.Abdomen = MR.Body.GetChild(0); // Abdomen
            MR.Chest = MR.Abdomen.GetChild(0); // Chest
            MR.Neck = MR.Chest.GetChild(2); // Neck
            MR.Head = MR.Neck.GetChild(0); // NeckHead

            MR.LShoulder = MR.Chest.GetChild(0); // LeftShoulder
            MR._LFeedbacker = Arm.MRFeedbackerPreset(MR.LShoulder.GetChild(0));
            MR._LKnuckleblaster = Arm.MRKnuckleblasterPreset(MR.LShoulder.GetChild(1));
            MR._LWhiplash = Arm.MRWhiplashPreset(MR.LShoulder.GetChild(2));
            MR._LSandboxer = Arm.MRSandboxerPreset(MR.LShoulder.GetChild(3));

            MR.RShoulder = MR.Chest.GetChild(1); // RightShoulder
            MR._RFeedbacker = Arm.MRFeedbackerPreset(MR.RShoulder.GetChild(0));
            MR._RKnuckleblaster = Arm.MRKnuckleblasterPreset(MR.RShoulder.GetChild(1));
            MR._RWhiplash = Arm.MRWhiplashPreset(MR.RShoulder.GetChild(2));
            MR._RSandboxer = Arm.MRSandboxerPreset(MR.RShoulder.GetChild(3));

            if (Vars.Config.Controllers.LeftHanded)
            {
                MR.FeedbackerA = MR._RFeedbacker; MR.FeedbackerB = MR._LFeedbacker;
                MR.Knuckleblaster = MR._RKnuckleblaster;
                MR.Whiplash = MR._RWhiplash;
                MR.Sandboxer = MR._LSandboxer;
            }
            else
            {
                MR.FeedbackerA = MR._LFeedbacker; MR.FeedbackerB = MR._RFeedbacker;
                MR.Knuckleblaster = MR._LKnuckleblaster;
                MR.Whiplash = MR._LWhiplash;
                MR.Sandboxer = MR._RSandboxer;
            }

            MR.LeftLeg = Leg.MRPreset(MR.Root.GetChild(0).GetChild(0).GetChild(1));
            MR.RightLeg = Leg.MRPreset(MR.Root.GetChild(0).GetChild(0).GetChild(2));

            MR.IKPole_Left = MR.Root.GetChild(0).GetChild(1);
            MR.IKPole_Right = MR.Root.GetChild(0).GetChild(2);
            return MR;
        }
        public static MetaRig CreateVCustomPreset(GameObject Parent, string Name = null)
        {
            GameObject V1mdlGO = Object.Instantiate(Assets.Vars.VRig, Parent.transform, true);
            if (Name != null) V1mdlGO.name = Name;
            return VCustomPreset(V1mdlGO.transform);
        }
    }
}

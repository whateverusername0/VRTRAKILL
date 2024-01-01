using UnityEngine;
using VRBasePlugin.ULTRAKILL.VRAvatar.Armature;

#pragma warning disable IDE1006 // Naming Styles
namespace VRBasePlugin.ULTRAKILL.VRAvatar
{
    internal class MetaRig
    {
        public Transform GameObjectT { get; private set; }

        #region Core
        public Transform Root { get; private set; }
        public Transform Body { get; private set; }
        public Transform Abdomen { get; private set; }
        public Transform Chest { get; private set; }
        public Transform NeckEnd { get; private set; }
        public Transform Head { get; private set; }
        #endregion

        #region Arms & Shoulders
        public Transform LShoulder { get; private set; }
        public Arm _LFeedbacker { get; private set; }
        public Arm _LKnuckleblaster { get; private set; }
        public Arm _LWhiplash { get; private set; }
        public Arm _LSandboxer { get; private set; }

        public Transform RShoulder { get; private set; }
        public Arm _RFeedbacker { get; private set; }
        public Arm _RKnuckleblaster { get; private set; }
        public Arm _RWhiplash { get; private set; }
        public Arm _RSandboxer { get; private set; }

        public Arm FeedbackerA { get; private set; } public Arm FeedbackerB { get; private set; }
        public Arm Knuckleblaster { get; private set; }
        public Arm Whiplash { get; private set; }
        public Arm Sandboxer { get; private set; }

        #endregion

        #region Legs
        public Transform Pelvis { get; set; }
        public Leg LeftLeg { get; set; }
        public Leg LeftLegIK { get; private set; }
        public Leg RightLeg { get; set; }
        public Leg RightLegIK { get; private set; }
        #endregion

        #region IK Poles
        public Transform Arm_IKPole_Left { get; private set; }
        public Transform Arm_IKPole_Right { get; private set; }
        public Transform Leg_IKPole_Left { get; private set; }
        public Transform Leg_IKPole_Right { get; private set; }
        #endregion

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
            MR.NeckEnd = MR.Chest.GetChild(2).GetChild(0).GetChild(0); // Neck
            MR.Head = MR.Chest.GetChild(3); // V1_Head

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

            MR.Pelvis = MR.Root.GetChild(0).GetChild(0).GetChild(1);
            MR.LeftLeg = Leg.MRPreset(MR.Root.GetChild(0).GetChild(0).GetChild(1).GetChild(0));
            MR.RightLeg = Leg.MRPreset(MR.Root.GetChild(0).GetChild(0).GetChild(1).GetChild(2));
            MR.LeftLegIK = Leg.MRIKPreset(MR.Root.GetChild(0).GetChild(0).GetChild(1).GetChild(1));
            MR.RightLegIK = Leg.MRIKPreset(MR.Root.GetChild(0).GetChild(0).GetChild(1).GetChild(3));

            MR.Arm_IKPole_Left = MR.Root.GetChild(0).GetChild(1);
            MR.Arm_IKPole_Right = MR.Root.GetChild(0).GetChild(2);
            MR.Leg_IKPole_Left = MR.Root.GetChild(0).GetChild(0).GetChild(1).GetChild(4);
            MR.Leg_IKPole_Right = MR.Root.GetChild(0).GetChild(0).GetChild(1).GetChild(5);
            return MR;
        }
        public static MetaRig CreateVCustomPreset(GameObject Parent, string Name = null)
        {
            GameObject V1mdlGO = Object.Instantiate(Assets.VRig, Parent.transform, true);
            if (Name != null) V1mdlGO.name = Name;
            return VCustomPreset(V1mdlGO.transform);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
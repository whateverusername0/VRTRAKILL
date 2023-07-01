using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    public enum ArmType
    {
        Feedbacker,
        Knuckleblaster,
        Spear,
        Whiplash,
        Sandboxer
    }

    internal class Armature
    {
        public ArmType Type { get; set; }
        public Transform GameObjectT { get; set; }
        public Transform Root { get; set; }
        public Transform Clavicle { get; set; }
        public Transform UpperArm => Clavicle.GetChild(0); // * special case * //
        public Transform LowerArm => UpperArm.GetChild(0); // * special case * //
        public Transform Wrist { get; set; }

        public Transform Hand { get; set; }
        public Finger FIndex { get; set; }
        public Finger FPinkie { get; set; }
        public Finger FMiddle { get; set; }
        public Finger FRing { get; set; }
        public Finger FThumb { get; set; }

        public class Finger
        {
            public Transform Base { get; set; }
            public Transform Bridge => Base.GetChild(0);
            public Transform Tip { get; }
            public Transform TipEnd => Tip.GetChild(0);

            public Finger(Transform BaseD, bool IsThumb = false)
            {
                Base = BaseD;
                if (IsThumb) Tip = Base.GetChild(0);
                else Tip = Bridge.GetChild(0);
            }
        }

        public static Armature FeedbackerPreset(Transform T)
        {
            Armature A = new Armature();
            A.Type = ArmType.Feedbacker;
            A.GameObjectT = T;
            A.Root = T.GetChild(0).GetChild(0);
            A.Clavicle = A.Root;
            A.Wrist = A.UpperArm.GetChild(0);

            A.Hand = A.Wrist.GetChild(0);
            A.FIndex = new Finger(A.Hand.GetChild(0));
            A.FPinkie = new Finger(A.Hand.GetChild(1));
            A.FMiddle = new Finger(A.Hand.GetChild(2));
            A.FRing = new Finger(A.Hand.GetChild(3));
            A.FThumb = new Finger(A.Hand.GetChild(4), true);
            return A;
        }
        public static Armature MRFeedbackerPreset(Transform T)
        {
            Armature A = new Armature();
            A.Type = ArmType.Feedbacker;
            A.GameObjectT = T;
            A.Clavicle = A.GameObjectT;
            A.Hand = A.LowerArm.GetChild(0);
            A.FIndex = new Finger(A.Hand.GetChild(0));
            A.FThumb = new Finger(A.Hand.GetChild(1), IsThumb: true);
            return A;
        }
        public static Armature KnuckleblasterPreset(Transform T)
        {
            Armature A = new Armature();
            A.Type = ArmType.Knuckleblaster;
            A.GameObjectT = T;
            A.Root = T.GetChild(1);
            A.Clavicle = A.Root.GetChild(1);
            A.Wrist = A.Clavicle.GetChild(1);

            A.Hand = A.Wrist.GetChild(0);
            A.FIndex = new Finger(A.Hand.GetChild(4), true);
            A.FPinkie = new Finger(A.Hand.GetChild(5), true);
            A.FThumb = new Finger(A.Hand.GetChild(6), true);
            return A;
        }
        public static Armature SpearPreset(Transform T)
        {
            Armature A = new Armature();
            A.Type = ArmType.Spear;
            A.GameObjectT = T;
            return A;
        }
        public static Armature WhiplashPreset(Transform T)
        {
            Armature A = new Armature();
            A.Type = ArmType.Whiplash;
            A.GameObjectT = T;
            A.Root = T.GetChild(0).GetChild(1);
            A.Clavicle = A.Root.GetChild(1);
            A.Wrist = A.Clavicle.GetChild(1);

            A.Hand = A.Wrist.GetChild(0);
            A.FIndex = new Finger(A.Hand.GetChild(0));
            A.FPinkie = new Finger(A.Hand.GetChild(1));
            A.FMiddle = new Finger(A.Hand.GetChild(2));
            A.FRing = new Finger(A.Hand.GetChild(3));
            A.FThumb = new Finger(A.Hand.GetChild(4), true);
            return A;
        }
        public static Armature SandboxerPreset(Transform T)
        {
            Armature A = new Armature();
            A.Type = ArmType.Sandboxer;
            A.GameObjectT = T;
            A.Root = T.GetChild(0);
            A.Clavicle = A.Root;
            A.Wrist = A.UpperArm.GetChild(0);

            A.Hand = A.Wrist.GetChild(0);
            A.FIndex = new Finger(A.Hand.GetChild(0));
            A.FPinkie = new Finger(A.Hand.GetChild(1));
            A.FMiddle = new Finger(A.Hand.GetChild(2));
            A.FRing = new Finger(A.Hand.GetChild(3));
            A.FThumb = new Finger(A.Hand.GetChild(4), true);
            return A;
        }
    }
}

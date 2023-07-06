using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK.Armature
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
        public class tHand
        {
            public ArmType Type { get; set; }
            public Transform GameObjecT { get; set; }
            public Finger FIndex { get; set; }
            public Finger FPinkie { get; set; }
            public Finger FMiddle { get; set; }
            public Finger FRing { get; set; }
            public Finger FThumb { get; set; }

            public static tHand FeedbackerPreset(Transform Base)
            {
                tHand H = new tHand();
                H.Type = ArmType.Feedbacker;
                H.GameObjecT = Base;
                H.FIndex = new Finger(H.GameObjecT.GetChild(0));
                H.FPinkie = new Finger(H.GameObjecT.GetChild(1));
                H.FMiddle = new Finger(H.GameObjecT.GetChild(2));
                H.FRing = new Finger(H.GameObjecT.GetChild(3));
                H.FThumb = new Finger(H.GameObjecT.GetChild(4), true);
                return H;
            }
        }

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

        public ArmType Type { get; set; }
        public Transform GameObjecT { get; set; }
        public Transform Root { get; set; }
        public Transform Clavicle { get; set; }
        public Transform UpperArm => Clavicle.GetChild(0);
        public Transform Forearm => UpperArm.GetChild(0);
        public Transform Wrist { get; set; } public Transform Wrist_End { get; set; }

        public Transform Hand { get; set; }
        public Finger FIndex { get; set; }
        public Finger FPinkie { get; set; }
        public Finger FMiddle { get; set; }
        public Finger FRing { get; set; }
        public Finger FThumb { get; set; }

        public Transform Pole_RForearm { get; set; }
        public Transform Pole_LForearm { get; set; }

        public static Armature FeedbackerPreset(Transform GameObjectT)
        {
            try
            {
                Armature A = new Armature
                {
                    Type = ArmType.Feedbacker,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(0).GetChild(0)
                };
                A.Clavicle = A.Root;
                A.Wrist = A.UpperArm.GetChild(0);

                A.Hand = A.Wrist.GetChild(0);
                A.Wrist_End = CreateEndFromChild(A.Wrist, A.Hand);

                A.FIndex = new Finger(A.Hand.GetChild(0));
                A.FPinkie = new Finger(A.Hand.GetChild(1));
                A.FMiddle = new Finger(A.Hand.GetChild(2));
                A.FRing = new Finger(A.Hand.GetChild(3));
                A.FThumb = new Finger(A.Hand.GetChild(4), true);
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Armature KnuckleblasterPreset(Transform GameObjectT)
        {
            try
            {
                Armature A = new Armature
                {
                    Type = ArmType.Knuckleblaster,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(1)
                };
                A.Clavicle = A.Root.GetChild(1);
                A.Wrist = A.Clavicle.GetChild(1);

                A.Hand = A.Wrist.GetChild(0);
                A.Wrist_End = CreateEndFromChild(A.Wrist, A.Hand);

                A.FIndex = new Finger(A.Hand.GetChild(4), true);
                A.FPinkie = new Finger(A.Hand.GetChild(5), true);
                A.FThumb = new Finger(A.Hand.GetChild(6), true);
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Armature SpearPreset(Transform T)
        {
            try
            {
                Armature A = new Armature();
                A.Type = ArmType.Spear;
                A.GameObjecT = T;
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Armature WhiplashPreset(Transform GameObjectT)
        {
            try
            {
                Armature A = new Armature
                {
                    Type = ArmType.Whiplash,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(0).GetChild(1)
                };
                A.Clavicle = A.Root.GetChild(1);
                A.Wrist = A.Clavicle.GetChild(1);

                A.Hand = A.Wrist.GetChild(0);
                A.Wrist_End = CreateEndFromChild(A.Wrist, A.Hand);

                A.FIndex = new Finger(A.Hand.GetChild(0));
                A.FPinkie = new Finger(A.Hand.GetChild(1));
                A.FMiddle = new Finger(A.Hand.GetChild(2));
                A.FRing = new Finger(A.Hand.GetChild(3));
                A.FThumb = new Finger(A.Hand.GetChild(4), true);
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Armature SandboxerPreset(Transform GameObjectT)
        {
            try
            {
                Armature A = new Armature
                {
                    Type = ArmType.Sandboxer,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(0)
                };
                A.Clavicle = A.Root;
                A.Wrist = A.UpperArm.GetChild(0);

                A.Hand = A.Wrist.GetChild(0);
                A.Wrist_End = CreateEndFromChild(A.Wrist, A.Hand);

                A.FIndex = new Finger(A.Hand.GetChild(0));
                A.FPinkie = new Finger(A.Hand.GetChild(1));
                A.FMiddle = new Finger(A.Hand.GetChild(2));
                A.FRing = new Finger(A.Hand.GetChild(3));
                A.FThumb = new Finger(A.Hand.GetChild(4), true);
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }

        public static Armature MRFeedbackerPreset(Transform GameObjectT)
        {
            try
            {
                Armature A = new Armature
                {
                    Type = ArmType.Feedbacker,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT
                };
                A.Clavicle = A.GameObjecT;

                A.Hand = A.Forearm.GetChild(1);
                A.Wrist_End = A.Forearm.GetChild(0);

                A.FIndex = new Finger(A.Hand.GetChild(1));
                A.FThumb = new Finger(A.Hand.GetChild(0), IsThumb: true);
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Armature MRKnuckleblasterPreset(Transform GameObjectT)
        {
            Armature A = new Armature();
            A.Type = ArmType.Knuckleblaster;
            A.GameObjecT = GameObjectT;
            return null;
        }
        public static Armature MRWhiplashPreset(Transform GameObjectT)
        {
            Armature A = new Armature();
            A.Type = ArmType.Whiplash;
            A.GameObjecT = GameObjectT;
            return null;
        }
        public static Armature MRSpearPreset(Transform GameObjectT)
        {
            Armature A = new Armature();
            A.Type = ArmType.Spear;
            A.GameObjecT = GameObjectT;
            return null;
        }
        public static Armature MRSandboxerPreset(Transform GameObjectT)
        {
            Armature A = new Armature();
            A.Type = ArmType.Sandboxer;
            A.GameObjecT = GameObjectT;
            return null;
        }

        private static Transform CreateEndFromChild(Transform Parent, Transform CopyPositionFrom)
        {
            GameObject GO = new GameObject($"{Parent.gameObject.name}_End");
            GO.transform.parent = Parent;
            GO.transform.localPosition = CopyPositionFrom.localPosition;
            GO.transform.localRotation = CopyPositionFrom.localRotation;
            return GO.transform;
        }
    }
}

using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class Armature
    {
        // Simplifying things 101
        private enum ArmType
        {
            Feedbacker,
            Knuckleblaster,
            Whiplash
        }
        private ArmType? Type
        {
            get
            {
                if (GameObjectT.gameObject.GetComponent<Punch>().type == FistType.Standard)
                    return ArmType.Feedbacker;
                else if (GameObjectT.gameObject.GetComponent<Punch>().type == FistType.Heavy)
                    return ArmType.Knuckleblaster;
                else if (GameObjectT.gameObject.HasComponent<HookArm>())
                    return ArmType.Whiplash;
                else return null;
            }
        }

        public Transform GameObjectT { get; }
        public Transform Root
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                        return GameObjectT.GetChild(0).GetChild(0);
                    case ArmType.Knuckleblaster:
                        return GameObjectT.GetChild(1);
                    case ArmType.Whiplash:
                        return GameObjectT.GetChild(0).GetChild(1);
                    default: return null;
                }
            }
        }
        public Transform Clavicle
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                        return Root;
                    case ArmType.Knuckleblaster:
                    case ArmType.Whiplash:
                        return Root.GetChild(1);
                    default: return null;
                }
            }
        }
        public Transform UpperArm => Clavicle.GetChild(0);
        public Transform Wrist
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                        return UpperArm.GetChild(0);
                    case ArmType.Knuckleblaster:
                    case ArmType.Whiplash:
                        return Clavicle.GetChild(1);
                    default: return null;
                }
            }
        }

        public Transform Hand
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                        return Wrist.GetChild(0);
                    case ArmType.Knuckleblaster:
                        return Wrist.GetChild(0);
                    case ArmType.Whiplash:
                        return Wrist.GetChild(2);
                    default: return null;
                }
            }
        }
        public Finger FIndex
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                    case ArmType.Whiplash:
                        return new Finger(Hand.GetChild(0));
                    case ArmType.Knuckleblaster:
                        return new Finger(Hand.GetChild(4), true);
                    default: return null;
                }
            }
        }
        public Finger FPinkie
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                    case ArmType.Whiplash:
                        return new Finger(Hand.GetChild(1));
                    case ArmType.Knuckleblaster:
                        return new Finger(Hand.GetChild(5), true);
                    default: return null;
                }
            }
        }
        public Finger FMiddle
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                    case ArmType.Whiplash:
                        return new Finger(Hand.GetChild(2));
                    case ArmType.Knuckleblaster:
                    default: return null;
                }
            }
        }
        public Finger FRing
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                    case ArmType.Whiplash:
                        return new Finger(Hand.GetChild(3));
                    case ArmType.Knuckleblaster:
                    default: return null;
                }
            }
        }
        public Finger FThumb
        {
            get
            {
                switch (Type)
                {
                    case ArmType.Feedbacker:
                    case ArmType.Whiplash:
                        return new Finger(Hand.GetChild(4), true);
                    case ArmType.Knuckleblaster:
                        return new Finger(Hand.GetChild(6), true);
                    default: return null;
                }
            }
        }

        public class Finger : Transform
        {
            public Transform Base { get; set; }
            public Transform Bridge { get; }
            public Transform Tip { get; }
            public Transform TipEnd { get; }

            public Finger(Transform BaseD, bool IsThumb = false)
            {
                Base = BaseD;
                Bridge = Base.GetChild(0);
                if (IsThumb) Tip = Base.GetChild(0);
                else Tip = Bridge.GetChild(0);
                TipEnd = Tip.GetChild(0);
            }
        }
        public Armature(Transform T) { GameObjectT = T; }
    }
}

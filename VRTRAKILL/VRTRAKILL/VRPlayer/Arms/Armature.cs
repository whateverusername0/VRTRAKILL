using System;
using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    public enum ArmType
    {
        Feedbacker,
        Knuckleblaster,
        Spear,
        Whiplash
    }

    internal class Armature
    {
        // Simplifying things 101
        private ArmType? _Type; public ArmType? Type
        {
            get
            {
                if (_Type == null)
                {
                    if (GameObjectT.gameObject.GetComponent<Punch>().type == FistType.Standard)
                        return ArmType.Feedbacker;
                    else if (GameObjectT.gameObject.GetComponent<Punch>().type == FistType.Heavy)
                        return ArmType.Knuckleblaster;
                    else if (GameObjectT.gameObject.HasComponent<HookArm>())
                        return ArmType.Whiplash;
                    else return null;
                }
                else return _Type;
            }
            set { _Type = value; }
        }

        public Transform GameObjectT { get; }
        public Transform Root { get; }
        public Transform Clavicle { get; }
        public Transform UpperArm => Clavicle.GetChild(0); // * special case * //
        public Transform LowerArm => UpperArm.GetChild(0);
        public Transform Wrist { get; }

        public Transform Hand { get; }
        public Finger FIndex { get; }
        public Finger FPinkie { get; }
        public Finger FMiddle { get; }
        public Finger FRing { get; }
        public Finger FThumb { get; }

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
        public Armature(Transform T, ArmType? Type = null)
        {
            GameObjectT = T;

            switch (Type)
            {
                case ArmType.Feedbacker:
                    {
                        Root = GameObjectT.GetChild(0).GetChild(0);
                        Clavicle = Root;
                        // UpperArm
                        Wrist = UpperArm.GetChild(0);

                        Hand = Wrist.GetChild(0);
                        FIndex = new Finger(Hand.GetChild(0));
                        FPinkie = new Finger(Hand.GetChild(1));
                        FMiddle = new Finger(Hand.GetChild(2));
                        FRing = new Finger(Hand.GetChild(3));
                        FThumb = new Finger(Hand.GetChild(4), true);
                        break;
                    }
                case ArmType.Knuckleblaster:
                    {
                        Root = GameObjectT.GetChild(1);
                        Clavicle = Root.GetChild(1);
                        // UpperArm
                        Wrist = Clavicle.GetChild(1);

                        Hand = Wrist.GetChild(0);
                        FIndex = new Finger(Hand.GetChild(4), true);
                        FPinkie = new Finger(Hand.GetChild(5), true);
                        // FMiddle
                        // FRing
                        FThumb = new Finger(Hand.GetChild(6), true);
                        break;
                    }
                case ArmType.Whiplash:
                    {
                        Root = GameObjectT.GetChild(0).GetChild(1);
                        Clavicle = Root.GetChild(1);
                        // UpperArm
                        Wrist = Clavicle.GetChild(1);

                        Hand = Wrist.GetChild(2);
                        FIndex = new Finger(Hand.GetChild(0));
                        FPinkie = new Finger(Hand.GetChild(1));
                        FMiddle = new Finger(Hand.GetChild(2));
                        FRing = new Finger(Hand.GetChild(3));
                        FThumb = new Finger(Hand.GetChild(4), true);
                        break;
                    }
                case ArmType.Spear:
                default:
                    throw new NullReferenceException();
            }
        }
    }
}

using System;
using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
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
                    { _Type = ArmType.Feedbacker; return ArmType.Feedbacker; }
                    else if (GameObjectT.gameObject.GetComponent<Punch>().type == FistType.Heavy)
                    { _Type = ArmType.Knuckleblaster; return ArmType.Knuckleblaster; }
                    else if (GameObjectT.gameObject.HasComponent<HookArm>())
                    { _Type = ArmType.Whiplash; return ArmType.Whiplash; }
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
        public Transform LowerArm => UpperArm.GetChild(0); // * special case * //
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
        public Armature(Transform T, ArmType? Type = null, bool IsSandboxer = false)
        {
            GameObjectT = T; _Type = Type;
            switch (Type)
            {
                case ArmType.Feedbacker:
                    {
                        if (IsSandboxer) Root = GameObjectT.GetChild(0);
                        else Root = GameObjectT.GetChild(0).GetChild(0);
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

                        Hand = Wrist.GetChild(0);
                        FIndex = new Finger(Hand.GetChild(0));
                        FPinkie = new Finger(Hand.GetChild(1));
                        FMiddle = new Finger(Hand.GetChild(2));
                        FRing = new Finger(Hand.GetChild(3));
                        FThumb = new Finger(Hand.GetChild(4), true);
                        break;
                    }
                case ArmType.Spear:
                default: throw new NullReferenceException();
            }
        }
    }
}

using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Feedbacker
{
    internal class Armature
    {
        public static Transform Root => GameObject.Find("Arm2").transform;
        public static Transform RArmature => Root.GetChild(0);
        public static Transform UpperArm => RArmature.GetChild(0);
        public static Transform Forearm => UpperArm.GetChild(0);

        public static Transform Hand => Forearm.GetChild(0);
        public static Finger FIndex => new Finger(Hand.GetChild(0));
        public static Finger FLittle => new Finger(Hand.GetChild(1));
        public static Finger FMiddle => new Finger(Hand.GetChild(2));
        public static Finger FRing => new Finger(Hand.GetChild(3));
        public static Finger FThumb => new Finger(Hand.GetChild(4), true);

        public class Finger : Transform
        {
            public Transform Base { get; set; }
            public Transform Bridge { get; }
            public Transform Tip { get; }
            public Transform TipEnd { get; }

            public Finger(Transform BaseD, bool IsThumb = false)
            {
                Base = BaseD; // do you get it guys?? based!!! haha so funny!!
                Bridge = Base.GetChild(0);
                if (IsThumb) Tip = Base.GetChild(0);
                else Tip = Bridge.GetChild(0);
                TipEnd = Tip.GetChild(0);
            }
        }
    }
}

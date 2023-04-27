using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Feedbacker
{
    internal class Armature
    {
        public Transform Root { get; set; }
        public Transform RArmature => Root.GetChild(0);
        public Transform UpperArm  => RArmature.GetChild(0);
        public Transform Forearm   => UpperArm.GetChild(0);

        public Transform Hand => Forearm.GetChild(0);
        public Finger FIndex  => new Finger(Hand.GetChild(0));
        public Finger FLittle => new Finger(Hand.GetChild(1));
        public Finger FMiddle => new Finger(Hand.GetChild(2));
        public Finger FRing   => new Finger(Hand.GetChild(3));
        public Finger FThumb  => new Finger(Hand.GetChild(4), true);

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

        public Armature(Transform R)
        {
            Root = R;
        }
    }
}

using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK.Armature
{
    internal class Hand
    {
        public ArmType Type { get; set; }
        public Transform Root { get; set; }
        public Finger FIndex { get; set; }
        public Finger FPinkie { get; set; }
        public Finger FMiddle { get; set; }
        public Finger FRing { get; set; }
        public Finger FThumb { get; set; }

        public static Hand FeedbackerPreset(Transform Base)
        {
            Hand H = new Hand
            {
                Type = ArmType.Feedbacker,
                Root = Base
            };
            H.FIndex = new Finger(H.Root.GetChild(0));
            H.FPinkie = new Finger(H.Root.GetChild(1));
            H.FMiddle = new Finger(H.Root.GetChild(2));
            H.FRing = new Finger(H.Root.GetChild(3));
            H.FThumb = new Finger(H.Root.GetChild(4), true);
            return H;
        }
        public static Hand KnuckleblasterPreset(Transform Base)
        {
            Hand H = new Hand
            {
                Type = ArmType.Knuckleblaster,
                Root = Base
            };
            H.FIndex = new Finger(H.Root.GetChild(4), true);
            H.FPinkie = new Finger(H.Root.GetChild(5), true);
            H.FThumb = new Finger(H.Root.GetChild(6), true);
            return H;
        }
        public static Hand MRKnuckleblasterPreset(Transform Base)
        {
            Hand H = new Hand
            {
                Type = ArmType.Knuckleblaster,
                Root = Base
            };
            H.FIndex = new Finger(H.Root.GetChild(0), true);
            H.FPinkie = new Finger(H.Root.GetChild(1), true);
            H.FThumb = new Finger(H.Root.GetChild(2), true);
            return H;
        }
    }
}

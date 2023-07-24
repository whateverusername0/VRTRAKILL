using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK.Armature
{
    public class Finger
    {
        public Transform Root { get; set; }
        public Transform Bridge => Root.GetChild(0);
        public Transform Tip { get; }

        public Finger(Transform RooT, bool IsThumb = false)
        {
            Root = RooT;
            if (IsThumb) Tip = Root.GetChild(0);
            else Tip = Bridge.GetChild(0);
        }
    }
}

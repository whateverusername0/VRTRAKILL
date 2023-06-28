using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    internal class Leggature
    {
        public Transform GameObjectT { get; set; }
        public Transform Thigh { get; set; }
        public Transform Shin { get; set; }
        public Transform Foot { get; set; }
        public Transform Heel { get; set; }
        public Transform Toe { get; set; }

        public static Leggature V1Preset(Transform T)
        {
            Leggature L = new Leggature();
            L.GameObjectT = T;
            L.Thigh = L.GameObjectT;
            L.Shin = L.Thigh.GetChild(0);
            L.Foot = L.Shin.GetChild(0);
            L.Heel = L.Foot.GetChild(0);
            L.Toe = L.Foot.GetChild(1);
            return L;
        }
    }
}

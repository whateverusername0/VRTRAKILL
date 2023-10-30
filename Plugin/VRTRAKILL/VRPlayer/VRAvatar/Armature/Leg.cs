using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature
{
    internal class Leg
    {
        public Transform GameObjecT { get; set; }
        public Transform Thigh { get; set; }
        public Transform Calf { get; set; }
        public Transform Foot { get; set; }
        public Transform Heel { get; set; }
        public Transform Toe { get; set; }

        public static Leg MRPreset(Transform T)
        {
            Leg L = new Leg
            {
                GameObjecT = T,
                Thigh = T
            };
            L.Calf = L.Thigh.GetChild(0);
            L.Foot = L.Calf.GetChild(0);
            L.Heel = L.Foot.GetChild(0);
            L.Toe = L.Foot.GetChild(1);
            return L;
        }
        public static Leg MRIKPreset(Transform T)
        {
            Leg L = new Leg
            {
                GameObjecT = T,
                Thigh = T
            };
            L.Calf = L.Thigh.GetChild(0);
            L.Foot = L.Calf.GetChild(0);
            L.Heel = L.Foot.GetChild(0).GetChild(0);
            L.Toe = L.Foot.GetChild(0).GetChild(1);
            return L;
        }
    }
}

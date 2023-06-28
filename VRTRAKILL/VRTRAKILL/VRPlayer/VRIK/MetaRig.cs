using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    internal class MetaRig
    {
        public Transform GameObjectT { get; set; }
        public Transform Root { get; set; }
        public Transform Body { get; set; }
        public Transform Abdomen { get; set; }
        public Transform Chest { get; set; }
        public Transform Neck { get; set; }
        public Transform Head { get; set; }

        public Armature LeftArm { get; set; }
        public Armature RightArm { get; set; }

        public Leggature LeftLeg { get; set; }
        public Leggature RightLeg { get; set; }

        public static MetaRig V1Preset(Transform T)
        {
            MetaRig MR = new MetaRig();
            MR.GameObjectT = T; // v1_combined
            MR.Root = T.GetChild(0).GetChild(0); // spine
            MR.Body = MR.Root.GetChild(0); // spine.001
            MR.Abdomen = MR.Body.GetChild(0); // spine.002
            MR.Chest = MR.Abdomen.GetChild(0); // spine.003
            MR.Neck = MR.Chest.GetChild(2); // spine.004
            MR.Head = MR.Neck.GetChild(0); // spine.005

            MR.LeftArm = Armature.MRFeedbackerPreset(MR.Chest.GetChild(0)); // shoulder.L
            MR.RightArm = Armature.MRFeedbackerPreset(MR.Chest.GetChild(1)); // shoulder.R
            return MR;
        }
        public static MetaRig CreateV1Preset(GameObject Parent)
        {
            GameObject V1mdlGO = GameObject.Find("v1_combined");
            GameObject V1mdlGOC = GameObject.Instantiate(V1mdlGO, Parent.transform);
            return V1Preset(V1mdlGOC.transform);
        }
    }
}

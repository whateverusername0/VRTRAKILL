using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRIK.Armature;

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

        public Armature.Armature LeftArm { get; set; }
        public Armature.Armature RightArm { get; set; }

        public Leggature LeftLeg { get; set; }
        public Leggature RightLeg { get; set; }

        public static MetaRig V1CustomPreset(Transform T)
        {
            MetaRig MR = new MetaRig();
            MR.GameObjectT = T; // V1Rig
            MR.Root = T.GetChild(1).GetChild(0); // Root
            MR.Body = MR.Root.GetChild(2); // Spine
            MR.Abdomen = MR.Body.GetChild(0); // Abdomen
            MR.Chest = MR.Abdomen.GetChild(0); // Chest
            MR.Neck = MR.Chest.GetChild(1); // Neck
            MR.Head = MR.Neck.GetChild(0); // NeckHead

            MR.LeftArm = Armature.Armature.MRFeedbackerPreset(MR.Chest.GetChild(0)); // LeftShoulder
            MR.RightArm = Armature.Armature.MRFeedbackerPreset(MR.Chest.GetChild(2)); // RightShoulder

            MR.LeftLeg = Leggature.MRV1Preset(MR.Root.GetChild(0));
            MR.RightLeg = Leggature.MRV1Preset(MR.Root.GetChild(1));
            return MR;
        }
        public static MetaRig CreateV1CustomPreset(GameObject Parent)
        {
            GameObject V1mdlGO = Object.Instantiate(Assets.Vars.V1Rig, Parent.transform, true);
            return V1CustomPreset(V1mdlGO.transform);
        }
    }
}

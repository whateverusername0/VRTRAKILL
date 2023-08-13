using UnityEngine;

namespace Plugin.VRTRAKILL.Assets
{
    public static class Vars
    {
        public static GameObject VRig => AssetLoader.VRig;
        //public static GameObject VHead => AssetLoader.VHead;

        public static Material[] Skin_V1 => AssetLoader.V1Skin;
        public static Material[] Skin_V2 => AssetLoader.V2Skin;

        public static GameObject HandPose_Shotgun => AssetLoader.HandPose_Shotgun;
        public static GameObject HandPose_Nailgun => AssetLoader.HandPose_Nailgun;
        public static GameObject HandPose_Sawblade => AssetLoader.HandPose_Sawblade;
        public static GameObject HandPose_Railgun => AssetLoader.HandPose_Railgun;
    }
}

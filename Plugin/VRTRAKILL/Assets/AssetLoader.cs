using UnityEngine;

namespace Plugin.VRTRAKILL.Assets
{
    // check manifests in VRTRAKILL/AssetBundles to know which one is which
    internal class AssetLoader
    {
        // V1 Rig (also usable for V2)
        public static GameObject VRig { get; private set; }
        //public static GameObject VHead { get; private set; }

        // 0 - Body, 1 - Wing, 2 - Hand
        public static Material[] V1Skin { get; private set; }
        public static Material[] V2Skin { get; private set; }

        // Hand Poses
        public static GameObject HandPose_Shotgun { get; private set; }
        public static GameObject HandPose_Nailgun{ get; private set; }
        public static GameObject HandPose_Sawblade{ get; private set; }
        public static GameObject HandPose_Railgun { get; private set; }

        public static void LoadAllCustomAssets()
        {
            AssetBundle Assets = LoadBundle("vrtrakillassetbundle");
            VRig = Object.Instantiate(LoadAsset<GameObject>(Assets, "V1/V1.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            //VHead = Object.Instantiate(LoadAsset<GameObject>(Assets, "V1/V1_Head.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);

            HandPose_Shotgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Shotgun.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            HandPose_Nailgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Nailgun.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            HandPose_Sawblade = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Sawblade.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            HandPose_Railgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Railgun.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);

            //if (V1Skin == null)
            //    V1Skin = new Material[]
            //    {
            //        Object.Instantiate(LoadAsset<Material>(Assets, "V1/Mat_Body.mat")),
            //        Object.Instantiate(LoadAsset<Material>(Assets, "V1/Mat_Wing.mat")),
            //        Object.Instantiate(LoadAsset<Material>(Assets, "V1/Mat_Arms.mat"))
            //    };
            //if (V2Skin == null)
            //    V2Skin = new Material[]
            //    {
            //        Object.Instantiate(LoadAsset<Material>(Assets, "V2/Mat_Body.mat")),
            //        Object.Instantiate(LoadAsset<Material>(Assets, "V2/Mat_Wing.mat")),
            //        Object.Instantiate(LoadAsset<Material>(Assets, "V2/Mat_Arms.mat"))
            //    };

            Assets.Unload(false);
        }

        public static T LoadAsset<T>(AssetBundle Bundle, string PrefabName, string PathToPrefab = "Assets/AssetsBundles") where T : Object
        {
            T Asset = Bundle.LoadAsset<T>($"{PathToPrefab}/{PrefabName}");
            if (Asset != null) return Asset;
            else { Plugin.PLog.LogError($"Failed to load {PrefabName}."); return null; }
        }
        public static AssetBundle LoadBundle(string BundleName, string Path = null)
        {
            if (Path == null) Path = $"{Plugin.PluginPath}\\";
            return AssetBundle.LoadFromFile($"{Path}\\{BundleName}");
        }
    }
}
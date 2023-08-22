using UnityEngine;

namespace Plugin.VRTRAKILL.Assets
{
    // check manifests in VRTRAKILL/AssetBundles to know which one is which
    internal class AssetLoader
    {
        // V1 Rig (also usable for V2)
        public static GameObject VRig { get; private set; }
        public static GameObject VHead { get; private set; }

        // Hand Poses
        public static GameObject HandPose_Shotgun { get; private set; }
        public static GameObject HandPose_Nailgun{ get; private set; }
        public static GameObject HandPose_Sawblade{ get; private set; }
        public static GameObject HandPose_Railgun { get; private set; }

        public static GameObject UI_GTFOTW { get; private set; }

        private static Vector3 DefaultPos = new Vector3(2048, 2048, 2048);
        private static Quaternion DefaultRot = Quaternion.identity;

        public static void LoadAllCustomAssets()
        {
            AssetBundle Assets = LoadBundle("vrtrakillassetbundle");
            VRig = Object.Instantiate(LoadAsset<GameObject>(Assets, "V1/V1.prefab"), DefaultPos, DefaultRot);
            VHead = Object.Instantiate(LoadAsset<GameObject>(Assets, "V1/V1_Head.prefab"), DefaultPos, DefaultRot);

            HandPose_Shotgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Shotgun.prefab"), DefaultPos, DefaultRot);
            HandPose_Nailgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Nailgun.prefab"), DefaultPos, DefaultRot);
            HandPose_Sawblade = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Sawblade.prefab"), DefaultPos, DefaultRot);
            HandPose_Railgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Railgun.prefab"), DefaultPos, DefaultRot);

            UI_GTFOTW = Object.Instantiate(LoadAsset<GameObject>(Assets, "UI/GetOutOfTheWall.prefab"), DefaultPos, DefaultRot);

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
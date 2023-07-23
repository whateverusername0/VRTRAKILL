using UnityEngine;

namespace Plugin.VRTRAKILL.Assets
{
    // check manifests in VRTRAKILL/AssetBundles to know which one is which
    internal class AssetLoader
    {
        public static GameObject V1Rig, V2Rig;
        public static GameObject HandPose_Shotgun,
                                 HandPose_Nailgun,
                                 HandPose_Sawblade,
                                 HandPose_Railgun;

        public static void LoadAllCustomAssets()
        {
            AssetBundle Assets = LoadBundle("vrtrakillassetbundle");
            V1Rig = Object.Instantiate(LoadAsset<GameObject>(Assets, "V1/V1.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            V2Rig = Object.Instantiate(LoadAsset<GameObject>(Assets, "V2/V2.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);

            HandPose_Shotgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Shotgun.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            HandPose_Nailgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Nailgun.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            HandPose_Sawblade = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Sawblade.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            HandPose_Railgun = Object.Instantiate(LoadAsset<GameObject>(Assets, "Arms/Feedbacker/Hand_Railgun.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);

            Assets.Unload(false);
        }

        public static T LoadAsset<T>(AssetBundle Bundle, string PrefabName, string PathToPrefab = "Assets/AssetsBundles") where T : Object
        {
            var Asset = Bundle.LoadAsset<T>($"{PathToPrefab}/{PrefabName}");
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
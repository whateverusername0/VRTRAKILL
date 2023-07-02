using UnityEngine;

namespace Plugin.VRTRAKILL.Assets
{
    // check manifests in VRTRAKILL/AssetBundles to know which one is which

    internal class AssetLoader
    {
        public static GameObject V1Rig;

        public static void LoadCustomAssets()
        {
            AssetBundle Assets = LoadBundle("Rigs\\mdl_v1rigged");
            V1Rig = Object.Instantiate(LoadAsset<GameObject>(Assets, "v1/V1Rig.prefab"), new Vector3(2048, 2048, 2048), Quaternion.identity);
            
            Assets.Unload(false);
        }

        public static T LoadAsset<T>(AssetBundle Bundle, string PrefabName, string PathToPrefab = "Assets/ASSets/things") where T : Object
        {
            var Asset = Bundle.LoadAsset<T>($"{PathToPrefab}/{PrefabName}");
            if (Asset != null) return Asset;
            else { Plugin.PLog.LogError($"Failed to load {PrefabName}."); return null; }
        }
        public static AssetBundle LoadBundle(string BundleName, string Path = null)
        {
            if (Path == null) Path = $"{Plugin.AssetsPath}\\";
            return AssetBundle.LoadFromFile($"{Path}\\{BundleName}");
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugin.VRTRAKILL
{
    // does what needs to be done before anything else
    internal class SceneWorker
    {
        public static void Init()
        => SceneManager.activeSceneChanged += (x, y) => SceneChanged(y);

        private static void SceneChanged(Scene S)
        {
            GloballyEnableOffscreenRendering();

            Config.ConfigJSON.Instance = null; // reload config
            UI.UIConverter.ConvertAllCanvases();
            Assets.AssetLoader.LoadAllCustomAssets();
        }

        private static void GloballyEnableOffscreenRendering()
        {
            foreach (SkinnedMeshRenderer R in Object.FindObjectsOfType<SkinnedMeshRenderer>())
                R.updateWhenOffscreen = true;
        }
    }
}
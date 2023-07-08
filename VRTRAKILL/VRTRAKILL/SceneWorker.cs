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
            UI.UIConverter.ConvertAllCanvases();
            Assets.AssetLoader.LoadAllCustomAssets();
        }
    }
}
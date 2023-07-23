using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL
{
    // does what needs to be done before anything else
    internal class SceneWorker
    {
        public static void Init()
        => SceneManager.activeSceneChanged += (x, y) => SceneChanged(y);

        // cool message suppersion :)
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private static void SceneChanged(Scene S)
        {
            GloballyEnableOffscreenRendering();

            Config.ConfigJSON.Instance = null; // reload config
            UI.UIConverter.ConvertAllCanvases();
            Assets.AssetLoader.LoadAllCustomAssets();

            RelayerAssetsStuff();
        }

        private static void GloballyEnableOffscreenRendering()
        {
            foreach (SkinnedMeshRenderer R in Object.FindObjectsOfType<SkinnedMeshRenderer>())
                R.updateWhenOffscreen = true;
        }
        private static void RelayerAssetsStuff()
        {
            // thanks to unity being so fucking weird instead of actual gameobjects it stores references to them,
            // so this works.
            foreach (PropertyInfo I in typeof(Assets.Vars).GetProperties())
            {
                if (I.GetValue(typeof(Assets.Vars), null) != null)
                {
                    GameObject GO = (GameObject)I.GetValue(typeof(Assets.Vars), null);
                    GO.RecursiveChangeLayer((int)Vars.Layers.AlwaysOnTop);
                }
                else continue;
            }
        }
    }
}
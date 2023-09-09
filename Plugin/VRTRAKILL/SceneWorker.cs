using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using Plugin.Util;

namespace Plugin.VRTRAKILL
{
    // does what needs to be done before anything else
    internal static class SceneWorker
    {
        public static void Init()
        => SceneManager.activeSceneChanged += (x, y) => SceneChanged(y);

        // cool message suppersion :)
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        private static void SceneChanged(Scene S)
        {
            Misc.EnableOffscreenRendering();

            Config.ConfigJSON.Instance = null; // reload config

            Assets.AssetLoader.LoadAllCustomAssets();
            RelayerAssetsStuff();

            UI.UIConverter.ConvertAllCanvases();
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
                    GO.RecursiveChangeLayer((int)Layers.AlwaysOnTop);
                }
                else continue;
            }
        }
    }
}
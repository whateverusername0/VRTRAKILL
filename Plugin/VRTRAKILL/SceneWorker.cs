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
        private static void SceneChanged(Scene S)
        {
            Util.Unity.EnableOffscreenRendering();

            // reload config
            Config.ConfigJSON.Instance = null;
            Config.ConfigMaster.Init();

            Assets.LoadAllCustomAssets();
            RelayerAssetsStuff();

            UI.UIConverter.ConvertAllCanvases();
        }
        private static void RelayerAssetsStuff()
        {
            // thanks to unity being so fucking weird instead of actual gameobjects it stores references to them,
            // so this works.
            foreach (PropertyInfo I in typeof(Assets).GetProperties())
            {
                if (I.GetValue(typeof(Assets), null) != null)
                {
                    GameObject GO = (GameObject)I.GetValue(typeof(Assets), null);
                    GO.RecursiveChangeLayer((int)Layers.AlwaysOnTop);
                }
                else continue;
            }
        }
    }
}
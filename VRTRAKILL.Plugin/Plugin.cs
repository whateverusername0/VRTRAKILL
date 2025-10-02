using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using Valve.VR;
using Plugin.Systems;
using VRTRAKILL.Utilities;
using Plugin.Data;

namespace Plugin
{
    // note: i will NEVER use transpilers IN THIS LIFETIME!! OVER MY DEAD BODY!!

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public sealed partial class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; }

        public void Awake()
        {
            Log = Logger;
            Debug.unityLogger.filterLogType = LogType.Warning;

            Prefs.ConfigMaster.Init();
            PatchStuff();
            SceneWorker.Init();

            InitializeSteamVR();
        }

        private void PatchStuff()
        {
            new Patcher(new HarmonyLib.Harmony(PluginInfo.PLUGIN_GUID))
            {
                Log = Vars.Log,
            }.PatchAll();
        }

        private void InitializeSteamVR()
        {
            SteamVR_Actions.PreInitialize();
            SteamVR.Initialize();
            Systems.Input.SVRActionsManager.Init();
        }
    }
}
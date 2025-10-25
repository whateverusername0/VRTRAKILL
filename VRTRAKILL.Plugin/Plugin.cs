using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using Valve.VR;
using VRTRAKILL.Systems;
using VRTRAKILL.Utilities;
using VRTRAKILL.Data;

namespace VRTRAKILL;

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
        // just patch everything at this point. nobody is looking at the settings anyway.
        new Patcher(new HarmonyLib.Harmony(PluginInfo.PLUGIN_GUID))
        {
            Log = Vars.Log,
        }.PatchAll();
    }

    private void InitializeSteamVR()
    {
        SteamVR_Actions.PreInitialize();
        SteamVR.Initialize(true);
        Log.LogMessage($"SteamVR Active: {SteamVR.active}, Connected: {SteamVR.initializedState}");

        Systems.Input.SVRActionsManager.Init();
    }
}
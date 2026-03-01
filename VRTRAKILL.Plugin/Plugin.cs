using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using Valve.VR;
using VRTRAKILL.Data;
using VRTRAKILL.Patches.Misc;
using VRTRAKILL.Patches.ULTRAKILL;
using VRTRAKILL.Utilities;

namespace VRTRAKILL;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public sealed partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; }

    public void Awake()
    {
        Log = Logger;
        Debug.unityLogger.filterLogType = LogType.Log;

        PatchStuff();
        //SceneWorker.Init();

        InitializeSteamVR();
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null)
        {
            Log.LogError("XR General Settings are null!");
            return;
        }

        var loader = XRGeneralSettings.Instance.Manager.activeLoader;
        if (loader == null)
            XRGeneralSettings.Instance.Manager.InitializeLoaderSync();

        UnityExtensions.EnableOffscreenRendering();
    }

    private void PatchStuff()
    {
        //new Patcher(new HarmonyLib.Harmony(PluginInfo.PLUGIN_GUID))
        //{
        //    Log = Vars.Log,
        //}.PatchAll();

        new Patcher(new HarmonyLib.Harmony(PluginInfo.PLUGIN_GUID))
        {
            Log = Vars.Log,
        }.Patch(new Type[]
        {
            typeof(PatchSteamVR),
            typeof(PatchCameraController),
            typeof(PatchInitGame)
        });
    }

    private void InitializeSteamVR()
    {
        Log.LogMessage("Initiaizing XR Loader");
        var generalSettings = ScriptableObject.CreateInstance<XRGeneralSettings>();
        var managerSettings = ScriptableObject.CreateInstance<XRManagerSettings>();
        var xrLoader = ScriptableObject.CreateInstance<OpenVRLoader>();

        var settings = OpenVRSettings.GetSettings();
        settings.StereoRenderingMode = OpenVRSettings.StereoRenderingModes.MultiPass;

        generalSettings.Manager = managerSettings;
        ((List<XRLoader>)managerSettings.activeLoaders).Clear();
        ((List<XRLoader>)managerSettings.activeLoaders).Add(xrLoader);
        managerSettings.InitializeLoaderSync();

        var loader = XRGeneralSettings.Instance.Manager.activeLoader;
        if (loader == null) Log.LogFatal("Unable to load XR Display Subsystem!");

        var display = managerSettings.activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
        display.Start();

        // for future reference
        if (display.TryGetDisplayRefreshRate(out var refreshRate))
            Vars.RefreshRate = refreshRate;

        Log.LogMessage("Active loader: " + managerSettings.activeLoader);

        Log.LogMessage("Initializing SteamVR");

        SteamVR_Actions.PreInitialize();
        SteamVR.Initialize(true);

        Log.LogMessage($"SteamVR Active: {SteamVR.active}, Connected: {SteamVR.initializedState}");

        //Systems.Input.SVRActionsManager.Init();
    }
}
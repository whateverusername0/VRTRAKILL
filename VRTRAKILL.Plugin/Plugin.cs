using BepInEx;
using BepInEx.Logging;
using System;
using System.IO;
using System.Linq;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using Valve.VR;
using VRTRAKILL.Plugin.Input;
using VRTRAKILL.Plugin.Preferences;
using VRTRAKILL.Plugin.Utilities;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Plugin;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public sealed partial class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; private set; }
    internal ManualLogSource Log { get; private set; }

    public XRGeneralSettings XRGeneralSettings { get; private set; }
    public XRManagerSettings XRManagerSettings { get; private set; }
    public OpenVRLoader OpenVRLoader { get; private set; }

    public GameObject ControllerGameObject { get; private set; }

    public void Awake()
    {
        var timestamp = Time.time;

        Instance = this;

        Log = Logger;
        Debug.unityLogger.filterLogType = LogType.Log;

        if (!ArgumentExists("--vr"))
        {
            Log.LogWarning(
                "Argument of --vr is missing.\n" +
                "Add it in the command line arguments for the mod to run.\n" +
                "The game will now start normally!");
            return;
        }

        if (!IsGameVersionValid())
        {
            var unintended =
                "This version of VRTRAKILL was NOT DESIGNED for this version of ULTRAKILL!\n" +
                "To run it regardless you have to add --forceVersion in the command line arguments.\n";

            if (!ArgumentExists("--forceVersion"))
            {
                unintended += "Which you did not! The game will now start normally!";
                Log.LogFatal(unintended);
                return;
            }
            else unintended += "Which you did! Do keep in mind that this voids all warranty!";

            Log.LogWarning(unintended);
        }

        Log.LogInfo("Initializing OpenXR.");
        if (!TryInitializeXR())
        {
            Log.LogFatal("Unable to initialize OpenXR. Aborting and starting in normal mode. Sorry!");

            Destroy(XRGeneralSettings);
            Destroy(XRManagerSettings);
            Destroy(OpenVRLoader);
            return;
        }

        Log.LogInfo("Applying patches.");
        new Patcher(new HarmonyLib.Harmony(PluginInfo.PLUGIN_GUID))
        {
            LogSource = Log,
        }.PatchAll();

        Log.LogInfo("Adding singletons.");
        ControllerGameObject = new GameObject("VRTRAKILL Managed");
        DontDestroyOnLoad(ControllerGameObject);

        ControllerGameObject.EnsureComponent<PluginSceneManager>();
        ControllerGameObject.EnsureComponent<PluginTimingManager>();
        ControllerGameObject.EnsureComponent<PluginPreferencesManager>();

        Log.LogInfo($"Initialized in {Time.time - timestamp}s.");
    }

    public bool TryInitializeXR()
    {
        Log.LogInfo("Initializing OpenXR with OpenVR as the main loader.");

        var gs = ScriptableObject.CreateInstance<XRGeneralSettings>();
        var xrm = ScriptableObject.CreateInstance<XRManagerSettings>();
        var vrl = ScriptableObject.CreateInstance<OpenVRLoader>();

        XRGeneralSettings = gs;
        XRManagerSettings = xrm;
        OpenVRLoader = vrl;

        gs.Manager = xrm;
        var ovrs = OpenVRSettings.GetSettings();
        ovrs.StereoRenderingMode = OpenVRSettings.StereoRenderingModes.MultiPass;

        if (!xrm.TryAddLoader(vrl))
        {
            Log.LogError("Unable to add OpenVR loader!");
            return false;
        }
        xrm.InitializeLoaderSync();
        xrm.StartSubsystems();

        var loader = XRGeneralSettings.Instance.Manager.activeLoader;
        if (loader == null)
        {
            Log.LogError("Unable to load XR display subsystem!");
            return false;
        }

        Log.LogMessage("Active loader: " + loader);

        // unsure if this is neccessary
        var displaySubsystem = loader.GetLoadedSubsystem<XRDisplaySubsystem>();
        if (displaySubsystem != null && !displaySubsystem.running)
            displaySubsystem.Start();

        return true;
    }

    public bool TryInitializeSteamVR()
    {
        Log.LogMessage("Initializing SteamVR.");

        try
        {
            SteamVR_Actions.PreInitialize();
            SteamVR.Initialize(true);
        }
        catch (Exception e)
        {
            Log.LogError($"Failed to initialize SteamVR! Exception:\n{e}");
            return false;
        }

        Log.LogMessage($"SteamVR Active: {SteamVR.active}, Connected: {SteamVR.initializedState}");

        SteamVRInputBridge.Initialize();

        return true;
    }

    public bool IsGameVersionValid()
    {
        var path = Path.Combine(PluginInfo.GamePath, "ULTRAKILL_Data", "boot.config");
        if (!File.Exists(path)) return true;

        var bootConfig = File.ReadAllLines(path);
        var build = bootConfig.FirstOrDefault(q => q.Contains("build-guid"));
        if (build == default) return true;

        var version = build.Split('=').LastOrDefault();
        if (version == default) return true;

        return version == PluginInfo.INTENDED_BUILD_GUID;
    }

    public bool ArgumentExists(string arg)
        => Environment.GetCommandLineArgs().Contains(arg);
}
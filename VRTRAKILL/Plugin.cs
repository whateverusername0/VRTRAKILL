using System.IO; using System.Diagnostics;
using BepInEx; using BepInEx.Logging;
using Valve.VR;

namespace Plugin
{
    /* If you're reading this and don't plan on stopping, then
     * welcome to the codebase where all hopes and dreams go die.
     * This is a dumpster fire of spaghetti code, inconsistent
     * naming and questionable life choices. Amen. */
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)] public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.whateverusername0.vrtrakill",
                            PLUGIN_NAME = "VRTRAKILL",
                            PLUGIN_VERSION = "0.9";

        public static string GameExePath = Process.GetCurrentProcess().MainModule.FileName,
                             GamePath = Path.GetDirectoryName(GameExePath),
                             HMDModel = string.Empty;

        internal static ManualLogSource PLogger { get; private set; }

        public void Awake()
        {
            PLogger = Logger;

            new HarmonyLib.Harmony(PLUGIN_GUID).PatchAll();

            VRTRAKILL.Config.ConfigMaster.Init();

            VRTRAKILL.UI.UIConverter.Init();

            InitializeSteamVR();
        }

        private void InitializeSteamVR()
        {
            SteamVR_Actions.PreInitialize();
            SteamVR.Initialize();

            VRTRAKILL.Input.VRActionsManager.Init();
        }
    }
}
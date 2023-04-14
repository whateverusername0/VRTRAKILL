using System.IO;
using System.Diagnostics;
using BepInEx;
using BepInEx.Logging;
using Valve.VR;

namespace Plugin
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)] public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.popikman.vrtrakill",
                            PLUGIN_NAME = "VRTRAKILL",
                            PLUGIN_VERSION = "0.5";

        public static string GameExePath = Process.GetCurrentProcess().MainModule.FileName,
                             GamePath = Path.GetDirectoryName(GameExePath),
                             HMDModel = string.Empty;

        internal static ManualLogSource PLogger { get; private set; }

        private void Awake()
        {
            PLogger = Logger;

            new HarmonyLib.Harmony(PLUGIN_GUID).PatchAll();

            VRTRAKILL.UI.VRUIController.Init();

            InitializeSteamVR();
        }

        private void InitializeSteamVR()
        {
            SteamVR_Actions.PreInitialize();
            SteamVR.Initialize();

            VRTRAKILL.VRInputManager.Init();
        }
    }
}
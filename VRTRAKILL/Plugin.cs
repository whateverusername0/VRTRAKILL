using System.IO; using System.Diagnostics;
using BepInEx; using BepInEx.Logging;
using Valve.VR;

using Plugin.Helpers; using Plugin.VRTRAKILL;

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

        public static Patcher MainPatcher, HapticsPatcher, GunsPatcher, ArmsPatcher, IKPatcher;

        internal static ManualLogSource PLogger { get; private set; }

        public void Awake()
        {
            PLogger = Logger;

            MainPatcher =
                new Patcher(new HarmonyLib.Harmony($"{PLUGIN_GUID}.base"), 
                            _Namespaces: new string[]
                            {
                                typeof(Helpers.Patches.A).Namespace,

                                typeof(VRTRAKILL.VRPlayer.VRCamera.Patches.A).Namespace,
                                typeof(VRTRAKILL.UI.Patches.A).Namespace,
                                typeof(VRTRAKILL.VRPlayer.Movement.Patches.A).Namespace
                            });
            MainPatcher.PatchAll();

            if (Vars.Config.Input.InputSettings.EnableControllerHaptics)
            {
                HapticsPatcher = 
                    new Patcher(new HarmonyLib.Harmony($"{PLUGIN_GUID}.haptics"),
                                typeof(VRTRAKILL.VRPlayer.Controllers.Patches.ControllerHaptics));
                HapticsPatcher.PatchAll();
            }
            if (Vars.Config.Input.InputSettings.EnableControllerShooting)
            {
                GunsPatcher = 
                    new Patcher(new HarmonyLib.Harmony($"{PLUGIN_GUID}.guns"),
                                typeof(VRTRAKILL.VRPlayer.Guns.Patches.A).Namespace);
                GunsPatcher.PatchAll();
            }
            if (Vars.Config.Input.InputSettings.EnableMovementPunching)
            {
                ArmsPatcher =
                    new Patcher(new HarmonyLib.Harmony($"{PLUGIN_GUID}.arms"),
                                typeof(VRTRAKILL.VRPlayer.Arms.Patches.A).Namespace);
                ArmsPatcher.PatchAll();
            }
            if (Vars.Config.Controllers.HandS.EnableVRIK)
            {
                IKPatcher =
                    new Patcher(new HarmonyLib.Harmony($"{PLUGIN_GUID}.vrik"),
                                typeof(VRTRAKILL.VRPlayer.Arms.VRIKPatches.A).Namespace);
                IKPatcher.PatchAll();
            }

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
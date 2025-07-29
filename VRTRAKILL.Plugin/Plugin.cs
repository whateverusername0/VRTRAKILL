using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using Valve.VR;
using Plugin.Systems;
using VRTRAKILL.Utilities;

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
            System.Collections.Generic.List<string> Namespaces = new System.Collections.Generic.List<string>
            {
                typeof(Patches.A).Namespace,
                typeof(Systems.Patches.A).Namespace,

                typeof(Systems.VRCamera.Patches.A).Namespace,
                typeof(Systems.UI.Patches.A).Namespace,
                typeof(Systems.Movement.Patches.A).Namespace,
            };
            System.Collections.Generic.List<System.Type> Types = new System.Collections.Generic.List<System.Type>
            {
                typeof(Systems.Controllers.Patches.ControllerAdder),
                typeof(Systems.Input.ControlMessages.Patches),
            };
            if (Vars.Config.Controllers.EnableHaptics) Types.Add(typeof(Systems.Controllers.Patches.ControllerHaptics));
            if (Vars.Config.EnableCBS)                 Namespaces.Add(typeof(Systems.Guns.Patches.A).Namespace);
            if (Vars.Config.EnableMBP)                 Namespaces.Add(typeof(Systems.Arms.Patches.A).Namespace);
            if (!Vars.Config.MBP.CameraWhiplash)       Namespaces.Add(typeof(Systems.Arms.Patches.Whiplash.A).Namespace);
            if (Vars.Config.EnableVRBody)              Namespaces.Add(typeof(Systems.VRAvatar.Patches.A).Namespace);

            new Patcher(new HarmonyLib.Harmony(PluginInfo.PLUGIN_GUID))
            {
                Namespaces = Namespaces.ToArray(),
                Types = Types.ToArray(),
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
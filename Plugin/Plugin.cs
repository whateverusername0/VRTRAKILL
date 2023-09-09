﻿using System.IO;
using System.Reflection;
using System.Diagnostics;

using BepInEx;
using BepInEx.Logging;

using Valve.VR;

using Plugin.Util;
using Plugin.VRTRAKILL;

namespace Plugin
{
    /* If you're reading this and don't plan on stopping, then
     * welcome to the codebase where all hopes and dreams go die.
     * This is a dumpster fire of spaghetti, inconsistent
     * naming, questionable life choices and shitty performance. Amen. */
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)] public sealed class Plugin : BaseUnityPlugin
    {
        public const string
            PLUGIN_GUID = "com.whateverusername0.vrtrakill",
            PLUGIN_NAME = "VRTRAKILL",
            PLUGIN_VERSION = "0.14";

        public static string
            PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            FullGamePath = Process.GetCurrentProcess().MainModule.FileName,
            GamePath = Path.GetDirectoryName(FullGamePath);

        internal static ManualLogSource PLog { get; private set; }

        public void Awake()
        {
            PLog = Logger;

            VRTRAKILL.Config.ConfigMaster.Init();
            PatchStuff();
            new Patcher(new HarmonyLib.Harmony($"{PLUGIN_GUID}.testing")) { Type = typeof(VRTRAKILL.Input.ControlMessages.Patches) }.PatchAll();
            SceneWorker.Init();

            InitializeSteamVR();
        }
        private void PatchStuff()
        {
            System.Collections.Generic.List<string> Namespaces = new System.Collections.Generic.List<string>
            {
                typeof(Util.Patches.A).Namespace,
                typeof(VRTRAKILL.Patches.A).Namespace,

                typeof(VRTRAKILL.VRPlayer.VRCamera.Patches.A).Namespace,
                typeof(VRTRAKILL.UI.Patches.A).Namespace,
                typeof(VRTRAKILL.VRPlayer.Patches.A).Namespace,
                typeof(VRTRAKILL.VRPlayer.Movement.Patches.A).Namespace,
            };
            System.Collections.Generic.List<System.Type> Types = new System.Collections.Generic.List<System.Type>
            {
                typeof(VRTRAKILL.VRPlayer.Controllers.Patches.ControllerAdder),
                typeof(VRTRAKILL.Input.ControlMessages.Patches)
            };
            if (Vars.Config.Controllers.EnableHaptics)
                Types.Add(typeof(VRTRAKILL.VRPlayer.Controllers.Patches.ControllerHaptics));
            if (Vars.Config.EnableCBS)
                Namespaces.Add(typeof(VRTRAKILL.VRPlayer.Guns.Patches.A).Namespace);
            if (Vars.Config.EnableMBP)
                Namespaces.Add(typeof(VRTRAKILL.VRPlayer.Arms.Patches.A).Namespace);
            if (!Vars.Config.MBP.CameraWhiplash)
                Namespaces.Add(typeof(VRTRAKILL.VRPlayer.Arms.Patches.Whiplash.A).Namespace);
            if (Vars.Config.EnableVRBody)
                Namespaces.Add(typeof(VRTRAKILL.VRPlayer.VRAvatar.Patches.A).Namespace);

            new Patcher(new HarmonyLib.Harmony($"{PLUGIN_GUID}.base"))
            {
                Namespaces = Namespaces.ToArray(),
                Types = Types.ToArray()
            }.PatchAll();
        }
        private void InitializeSteamVR()
        {
            SteamVR_Actions.PreInitialize();
            SteamVR.Initialize();

            VRTRAKILL.Input.SVRActionsManager.Init();
        }
    }
}
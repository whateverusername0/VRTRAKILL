using BepInEx;
using BepInEx.Logging;
using Valve.VR;
using VRBasePlugin.ULTRAKILL;
using VRTRAKILL.Utilities;

namespace VRBasePlugin
{
    /* If you're reading this and don't plan on stopping, then
     * welcome to the codebase where all hopes and dreams go die.
     * This is a dumpster fire of spaghetti, inconsistent
     * naming, questionable life choices and shitty performance. Amen. */

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public sealed partial class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource PLog { get; private set; }

        public void Awake()
        {
            PLog = Logger;

            Versioning.CheckForUpdates();

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
                typeof(ULTRAKILL.Patches.A).Namespace,

                typeof(ULTRAKILL.VRCamera.Patches.A).Namespace,
                typeof(ULTRAKILL.UI.Patches.A).Namespace,
                typeof(ULTRAKILL.Movement.Patches.A).Namespace,
            };
            System.Collections.Generic.List<System.Type> Types = new System.Collections.Generic.List<System.Type>
            {
                typeof(ULTRAKILL.Controllers.Patches.ControllerAdder),
                typeof(ULTRAKILL.Input.ControlMessages.Patches),
            };
            if (Vars.Config.Controllers.EnableHaptics) Types.Add(typeof(ULTRAKILL.Controllers.Patches.ControllerHaptics));
            if (Vars.Config.EnableCBS)                 Namespaces.Add(typeof(ULTRAKILL.Guns.Patches.A).Namespace);
            if (Vars.Config.EnableMBP)                 Namespaces.Add(typeof(ULTRAKILL.Arms.Patches.A).Namespace);
            if (!Vars.Config.MBP.CameraWhiplash)       Namespaces.Add(typeof(ULTRAKILL.Arms.Patches.Whiplash.A).Namespace);
            if (Vars.Config.EnableVRBody)              Namespaces.Add(typeof(ULTRAKILL.VRAvatar.Patches.A).Namespace);

            new Patcher(new HarmonyLib.Harmony($"{PluginInfo.PLUGIN_GUID}.base"))
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
            ULTRAKILL.Input.SVRActionsManager.Init();
        }
    }
}
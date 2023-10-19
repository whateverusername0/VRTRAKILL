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

    // Dependencies are for compatibility with other mods.
    [BepInDependency("com.eternalUnion.pluginConfigurator", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("xzxADIxzx.Jaket", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public sealed partial class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource PLog { get; private set; }

        public void Awake()
        {
            PLog = Logger;

            Versioning.CheckForUpdates();

            VRTRAKILL.Config.ConfigMaster.Init();
            PatchStuff();
            SceneWorker.Init();

            InitializeSteamVR();

            VRTRAKILL.Compatibility.Initializer.Start();
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
            if (Vars.Config.Controllers.EnableHaptics) Types.Add(typeof(VRTRAKILL.VRPlayer.Controllers.Patches.ControllerHaptics));
            if (Vars.Config.EnableCBS)                 Namespaces.Add(typeof(VRTRAKILL.VRPlayer.Guns.Patches.A).Namespace);
            if (Vars.Config.EnableMBP)                 Namespaces.Add(typeof(VRTRAKILL.VRPlayer.Arms.Patches.A).Namespace);
            if (!Vars.Config.MBP.CameraWhiplash)       Namespaces.Add(typeof(VRTRAKILL.VRPlayer.Arms.Patches.Whiplash.A).Namespace);
            if (Vars.Config.EnableVRBody)              Namespaces.Add(typeof(VRTRAKILL.VRPlayer.VRAvatar.Patches.A).Namespace);

            new Patcher(new HarmonyLib.Harmony($"{PluginInfo.PLUGIN_GUID}.base"))
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
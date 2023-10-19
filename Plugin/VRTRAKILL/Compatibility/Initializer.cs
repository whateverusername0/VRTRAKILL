using System;
using System.Collections.Generic;
using BepInEx.Bootstrap;

namespace Plugin.VRTRAKILL.Compatibility
{
    /// <summary>
    /// Contains initializers for every compatible mod
    /// and has a method <c>Start()</c> that searches for every mod and calls it's equivalent function.
    /// </summary>
    // Those are to be maintained separately from VRTRAKILL and NOT mixed IN ANY WAY POSSIBLE.
    // Keep it simple and modular.
    internal static class Initializer
    {
        private static readonly Dictionary<string, Action<object>> Mods = new Dictionary<string, Action<object>>
        {
            { "com.eternalUnion.pluginConfigurator", (_) => PluginConfigurator() },
            { "xzxADIxzx.Jaket", (_) => JAKET() },
        };

        /// <summary> Searches for every compatible mod and calls it's equivalent function </summary>
        public static void Start()
        {
            Vars.Log.LogInfo("Searching for compatible mods...");
            foreach (var Plugin in Chainloader.PluginInfos)
            {
                Mods.TryGetValue(Plugin.Value.Metadata.GUID, out Action<object> A);
                A?.Invoke(null);
            }
            Vars.Log.LogInfo("Finished searching for compatible mods.");
        }

        private static void PluginConfigurator()
        {
            Vars.Log.LogInfo("Detected PluginConfigurator! Building settings UI...");
            Compatibility.PluginConfigurator.Init();
        }
        private static void JAKET()
        {
            Vars.Log.LogInfo("Detected JAKET! No funationality added. WIP!");
        }
    }
}

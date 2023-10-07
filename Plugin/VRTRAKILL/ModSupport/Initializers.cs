using System;
using System.Collections.Generic;
using PluginConfig.API;

namespace Plugin.VRTRAKILL.ModSupport
{
    // Contains initializers for every mod
    internal static class Initializers
    {
        public static Dictionary<string, Action<object>> Mods = new Dictionary<string, Action<object>>
        {
            { "com.eternalUnion.pluginConfigurator", (_) => PluginConfigurator() },
            { "xzxADIxzx.Jaket", (_) => JAKET() },
        };

        public static void PluginConfigurator()
        {
            PluginConfigurator PC = PluginConfig.API.PluginConfigurator.Create(Plugin.PLUGIN_NAME, Plugin.PLUGIN_GUID);
            new ConfigPanel(PC.rootPanel, "WORK IN PROGRESS!", "WIP");
        }
        public static void JAKET()
        {

        }
    }
}

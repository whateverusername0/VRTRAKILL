using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace VRTRAKILL.Plugin;

public static class PluginInfo
{
    public const string
        PLUGIN_GUID = "whateverusername0.VRTRAKILL", // bepinex thing DO NOT CHANGE EVER
        PLUGIN_NAME = "VRTRAKILL", // bepinex thing
        PLUGIN_VERSION = "1", // what build version is the plugin?
        INTENDED_BUILD_GUID = "cd2a9e08be6e44379c465e9b5d8b4fc2"; // see Data\boot.config : build-guid

    public static readonly string
        // "~/BepInEx/plugins/VRTRAKILL"
        PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),

        // "~/ULTRAKILL/ULTRAKILL.exe"
        FullGamePath = Process.GetCurrentProcess().MainModule.FileName,

        // "~/ULTRAKILL"
        GamePath = Path.GetDirectoryName(FullGamePath);
}

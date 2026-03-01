using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace VRTRAKILL
{
    public static class PluginInfo
    {
        public const string
            PLUGIN_GUID = "whateverusername0.VRTRAKILL",
            PLUGIN_NAME = "VRTRAKILL",
            PLUGIN_VERSION = "1.0.0";

        public static readonly string
            PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), // "~/BepInEx/plugins/VRTRAKILL"
            FullGamePath = Process.GetCurrentProcess().MainModule.FileName, // "~/ULTRAKILL/ULTRAKILL.exe"
            GamePath = Path.GetDirectoryName(FullGamePath); // "~/ULTRAKILL"

        public const string
            GithubRepoLink = "https://api.github.com/repos/whateverusername0/VRTRAKILL",
            FriendlyGithubRepoLink = "https://github.com/whateverusername0/VRTRAKILL";
    }
}

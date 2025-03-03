using BepInEx;
using BepInEx.Logging;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using Versioning;

namespace ULTRAVR
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public sealed class Plugin : BaseUnityPlugin
    {
        #region Plugin Info

        public const string
            PluginGUID = "VRTRAKILL", // do not change this string EVER
            PluginName = "VRTRAKILL",
            PluginVersion = "1.0.0"; // never use spaces

        public static readonly string
            PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), // "~/BepInEx/plugins/VRTRAKILL"
            FullGamePath = Process.GetCurrentProcess().MainModule.FileName, // "~/ULTRAKILL/ULTRAKILL.exe"
            GamePath = Path.GetDirectoryName(FullGamePath); // "~/ULTRAKILL"

        public const string
            GithubRepoLink_API = "https://api.github.com/repos/whateverusername0/VRTRAKILL",
            GithubRepoLink_Friendly = "https://github.com/whateverusername0/VRTRAKILL";

        #endregion

        internal static ManualLogSource Log { get; private set; }

        public void Awake()
        {
            Log = base.Logger;
            UnityEngine.Debug.unityLogger.filterLogType = LogType.Error;

            CheckForUpdates();
        }

        public void CheckForUpdates()
        {
            using (var v = new Versioning.Versioning(Log))
            {
                Version? latest = null;
                try
                {
                    latest = v.GetLatestVersion(GithubRepoLink_API);
                    var current = PluginVersion.ToVersion();

                    if (current == latest) // up to date
                    {
                        Log.LogInfo($"{PluginName} is up to date!");
                        return;
                    }
                    if (current < latest) // outdated
                    {
                        Log.LogWarning(
                            $"{PluginName} is out of date!" +
                            $"\nDownload the latest version at \"{GithubRepoLink_Friendly}/releases/latest\".");
                        return;
                    }
                    if (current > latest) // bleeding edge case...
                    {
                        Log.LogWarning(
                            $"{PluginName} somehow has a higher version than the latest github release." +
                            $"\nMight as well wish you good luck in patching all those classes!");
                        return;
                    }
                }
                catch
                {
                    Log.LogError(
                        $"Failed to check for updates!" +
                        $"\nThis might be an internal issue, but you can check if you're connected to the internet.");
                    return;
                }
            }
        }
    }
}

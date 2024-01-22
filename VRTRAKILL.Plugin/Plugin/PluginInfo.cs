﻿using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace VRBasePlugin
{
    public static class PluginInfo
    {
        public const string
            PLUGIN_GUID = "VRTRAKILL", // do not change this string EVER
            PLUGIN_NAME = "VRTRAKILL",
            PLUGIN_VERSION = "0.19.0"; // never use spaces

        public static readonly string
            PluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), // ".../BepInEx/plugins/VRTRAKILL"
            FullGamePath = Process.GetCurrentProcess().MainModule.FileName, // ".../ULTRAKILL/ULTRAKILL.exe"
            GamePath = Path.GetDirectoryName(FullGamePath); // ".../ULTRAKILL"

        public const string
            GithubRepoLink = "https://api.github.com/repos/whateverusername0/VRTRAKILL",
            FriendlyGithubRepoLink = "https://github.com/whateverusername0/VRTRAKILL";
    }
}
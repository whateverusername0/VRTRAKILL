using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.VRTRAKILL;

namespace Plugin.Util
{
    internal class Versioning
    {
        [JsonProperty("tag_name")] public string Version { get; set; }

        public static void CheckForUpdates()
        {
            Vars.Log.LogInfo("Checking for updates...");
            try
            {
                using (HttpClient HC = new HttpClient())
                {
                    var Stream = HC.GetStreamAsync($"{PluginInfo.GithubRepoLink}/releases/latest"); Stream.Wait();
                    Versioning LatestVersion = JsonConvert.DeserializeObject<Versioning>(Stream.ToString());
                    if (PluginInfo.PLUGIN_VERSION != LatestVersion.Version.ToVersion().ToString())
                    {
                        Vars.Log.LogWarning(
                            $"This version of VRTRAKILL is outdated!" +
                            $"\nIt is highly recommended that you download a newer version by visiting" +
                            $"{PluginInfo.FriendlyGithubRepoLink}/releases/latest");
                        return;
                    }
                }
            } catch { Vars.Log.LogError("Unable to check for updates!"); return; }
            Vars.Log.LogInfo("Your VRTRAKILL is fully up-to-date! :)");
        }
    }

    /// <summary>
    /// Reprenents a version of the mod.
    /// Structure: <c>"Major.Minor.Patch-revision"</c>
    /// </summary>
    public readonly struct Version
    {
        private readonly int? _Major, _Minor, _Patch;
        public int Major => _Major ?? 0;
        public int Minor => _Minor ?? 0;
        public int Patch => _Patch ?? 0;

        public Version(int Major) : this()
        => _Major = Major;
        public Version(int Major, int Minor) : this(Major)
        => _Minor = Minor;
        public Version(int Major, int Minor, int Patch) : this(Major, Minor)
        => _Patch = Patch;

        public static bool operator ==(Version V1, Version V2)
        {
            if (V1.Major == V2.Major && V1.Minor == V2.Minor && V1.Patch == V2.Patch) return true;
            else return false;
        }
        public static bool operator !=(Version V1, Version V2) => !(V1 == V2);

        public static bool operator >(Version V1, Version V2)
        {
            if ((V1.Major > V2.Major)
            || (V1.Major == V2.Major && V1.Minor > V2.Minor)
            || (V1.Major == V2.Major && V1.Minor == V2.Minor && V1.Patch > V2.Patch)) return true;
            else return false;
        }
        public static bool operator <(Version V1, Version V2)
        {
            if ((V1.Major < V2.Major)
            || (V1.Major == V2.Major && V1.Minor < V2.Minor)
            || (V1.Major == V2.Major && V1.Minor == V2.Minor && V1.Patch < V2.Patch)) return true;
            else return false;
        }

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode()
        {
            string S = ToString();
            return S.GetHashCode();
        }

        public override string ToString() => $"{Major}.{Minor}.{Patch}";
    }
}

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
                    var HRQM = new HttpRequestMessage(HttpMethod.Get, $"{PluginInfo.GithubRepoLink}/releases/latest");
                    HRQM.Headers.Add("User-Agent", "User-Agent");
                    var Response = HC.SendAsync(HRQM, HttpCompletionOption.ResponseContentRead); Response.Wait();

                    var Stream = Response.Result.Content.ReadAsStringAsync(); Stream.Wait();

                    Versioning LatestVersion = JsonConvert.DeserializeObject<Versioning>(Stream.Result);
                    if (PluginInfo.PLUGIN_VERSION.ToVersion() > LatestVersion.Version.ToVersion())
                    {
                        Vars.Log.LogWarning(
                            $"This version of VRTRAKILL is higher than the one on github!" +
                            $"\nAre you a developer? Or just fucking around with versions? Or is it my deadass who forgot to update a single number?" +
                            $"\nFind the latest prebuilt binary here: {PluginInfo.FriendlyGithubRepoLink}/releases/latest");
                        return;
                    }
                    else if (PluginInfo.PLUGIN_VERSION.ToVersion() == LatestVersion.Version.ToVersion())
                    { Vars.Log.LogInfo($"You are up to date! :)"); return; }
                    else if (PluginInfo.PLUGIN_VERSION.ToVersion() < LatestVersion.Version.ToVersion())
                    {
                        Vars.Log.LogWarning(
                            $"This version of VRTRAKILL is outdated!" +
                            $"\nIt is highly recommended that you download a newer version by visiting " +
                            $"{PluginInfo.FriendlyGithubRepoLink}/releases/latest");
                        return;
                    }
                }
            } catch(System.Exception E) { Vars.Log.LogError("Unable to check for updates!"); Vars.Log.LogError(E.Message + E.InnerException); return; }
            Vars.Log.LogInfo("Your VRTRAKILL is fully up-to-date! :)");
        }
    }

    /// <summary>
    /// Reprenents a version of the mod.
    /// Structure: <c>"Major.Minor.Patch"</c>
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

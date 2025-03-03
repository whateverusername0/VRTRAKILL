using BepInEx.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Versioning
{
    public class VersionJSON
    {
        [JsonProperty("tag_name")] public string Version { get; set; }
    }

    public class Versioning : IDisposable
    {
        public ManualLogSource Logger { get; private set; }

        private HttpClient _client;

        public Versioning(ManualLogSource logger)
        {
            Logger = logger;
            _client = new HttpClient();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public string GetLatestVersionRaw(string link)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{link}/releases/latest");

            // github doesn't allow other apps to get intel without a header
            // so i'm forging a new one
            request.Headers.Add("User-Agent", "SkibidiToilet");

            // waiting for it because it's async only and we're running on sync
            var response = _client.SendAsync(request); response.Wait();
            var stream = response.Result.Content.ReadAsStringAsync(); stream.Wait();

            var version = JsonConvert.DeserializeObject<VersionJSON>(stream.Result);
            return version.Version;
        }
        public Version GetLatestVersion(string link)
            => GetLatestVersionRaw(link).ToVersion();
    }
}
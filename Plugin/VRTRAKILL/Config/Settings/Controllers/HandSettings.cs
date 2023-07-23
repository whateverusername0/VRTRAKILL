using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.Controllers
{
    internal class HandSettings
    {
        [JsonProperty("Swap hands (left-handed mode)")] public bool LeftHandMode { get; set; } = false;
        [JsonProperty("Enable arms (VRIK) (unused)")] public bool EnableVRIK { get; set; } = false;
    }
}

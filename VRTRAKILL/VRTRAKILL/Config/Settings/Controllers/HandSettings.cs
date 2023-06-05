using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.Controllers
{
    public class HandSettings
    {
        [JsonProperty("Swap Hands (Left-Handed Mode) (unused)")] public bool LeftHandMode { get; set; } = false;
        [JsonProperty("Enable arms (VRIK) (unused)")] public bool FloatingHands { get; set; } = false;
    }
}

using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings
{
    public class Game
    {
        [JsonProperty("Do not override movement values")] public bool DoNotOverrideMoveValues { get; set; } = false;
        [JsonProperty("Enable first-person 4-S camera")] public bool EnableFP4SCam { get; set; } = true;
    }
}

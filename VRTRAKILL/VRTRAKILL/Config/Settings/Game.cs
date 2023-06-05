using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings
{
    public class Game
    {
        [JsonProperty("Do not override movement values")] public bool DoNotOverrideMoveValues { get; set; } = false;
    }
}

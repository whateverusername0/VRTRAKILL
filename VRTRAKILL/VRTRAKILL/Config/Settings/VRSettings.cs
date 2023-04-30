using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings
{
    internal class VRSettings
    {
        [JsonProperty("Enable default crosshair")] public bool EnableDefaultCrosshair { get; set; } = true;
        [JsonProperty("Draw controller lines")] public bool DrawControllerLines { get; set; } = false;
        [JsonProperty("Do not override jump strength")] public bool DoNotOverrideJumpPower { get; set; } = false;
    }
}

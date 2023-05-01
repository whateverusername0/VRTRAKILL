using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings
{
    internal class VRSettings
    {
        [JsonProperty("Enable default crosshair")] public bool EnableDefaultCrosshair { get; set; } = true;
        [JsonProperty("Draw controller lines")] public bool DrawControllerLines { get; set; } = false;
        [JsonProperty("Do not override movement values")] public bool DoNotOverrideMoveValues { get; set; } = false;
        [JsonProperty("HUD Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;
        [JsonProperty("Create Desktop camera (EXPIREMENTAL)")] public bool CreateDesktopCam { get; set; } = true;
    }
}

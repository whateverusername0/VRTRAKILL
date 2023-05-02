using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings
{
    internal class VRSettings
    {
        [JsonProperty("Enable default crosshair")] public bool EnableDefaultCrosshair { get; set; } = true;
        [JsonProperty("HUD Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;

        [JsonProperty("Do not override movement values")] public bool DoNotOverrideMoveValues { get; set; } = false;

        [JsonProperty("Draw controller lines (WIP, Unused)")] public bool DrawControllerLines { get; set; } = false;
        [JsonProperty("Controller line initial transparency (from 0 to 1)")] public float CLInitTransparency { get; set; } = 0.4f;
        [JsonProperty("Controller line end transparency (from 0 to 1)")] public float CLEndTransparency { get; set; } = 0.1f;

        [JsonProperty("Create Desktop camera (EXPIREMENTAL)")] public bool CreateDesktopCam { get; set; } = false;
    }
}

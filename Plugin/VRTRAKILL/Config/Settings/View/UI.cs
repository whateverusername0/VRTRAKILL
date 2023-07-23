using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.View
{
    internal class UI
    {
        [JsonProperty("Enable standard HUD (replaces classic hud)")] public bool EnableStandardHUD { get; set; } = false;
        [JsonProperty("HUD Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;
        [JsonProperty("Crosshair distance")] public float CrosshairDistance { get; set; } = 8;
    }
}

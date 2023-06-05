using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.Input
{
    public class InputSettings
    {
        [JsonProperty("Deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
        [JsonProperty("Snap turning")] public bool SnapTurning { get; set; } = false;
        [JsonProperty("Snap turning angles")] public float SnapTurningAngles { get; set; } = 45;
        [JsonProperty("Smooth turning speed")] public float SmoothTurningSpeed { get; set; } = 300;
        [JsonProperty("Enable Controller Haptics (Vibration)")] public bool EnableCH { get; set; } = true;
        [JsonProperty("Controller-based shooting (unused)")] public bool ControllerShooty { get; set; } = true;
        [JsonProperty("Movement-based punching (unused)")] public bool PunchIrl { get; set; } = true;
    }
}

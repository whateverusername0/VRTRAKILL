using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Input
{
    internal class VRInputSettings
    {
        [JsonProperty("Joystick deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
        [JsonProperty("Snap turning (unused)")] public bool SnapTurning { get; set; } = false;
        [JsonProperty("Snap turning angles (unused)")] public float SnapTurningAngles { get; set; } = 45;
        [JsonProperty("Smooth turning speed")] public float SmoothTurningSpeed { get; set; } = 300;
    }
}

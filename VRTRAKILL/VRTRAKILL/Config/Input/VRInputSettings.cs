using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Input
{
    internal class VRInputSettings
    {
        [JsonProperty("Joystick deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
        [JsonProperty("Snap turning")] public bool SnapTurning { get; set; } = false;
        [JsonProperty("Snap turning angles")] public float SnapTurningAngles { get; set; } = 45;
        [JsonProperty("Smooth turning speed")] public float SmoothTurningSpeed { get; set; } = 300;

        [JsonProperty("Enable Controller Haptics (unused)")] public bool EnableCH { get; set; } = false;
        [JsonProperty("Swap Hands (Left-Handed Mode) (unused)")] public bool LeftHandMode { get; set; } = false;
    }
}

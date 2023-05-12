using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Input
{
    internal class VRInputSettings
    {
        [JsonProperty("Joystick deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
        [JsonProperty("Snap turning")] public bool SnapTurning { get; set; } = false;
        [JsonProperty("Snap turning angles")] public float SnapTurningAngles { get; set; } = 45;
        [JsonProperty("Smooth turning speed")] public float SmoothTurningSpeed { get; set; } = 300;

        [JsonProperty("VR Hands")] public HandsSettings Hands { get; set; } public class HandsSettings
        {
            [JsonProperty("Enable Controller Haptics")] public bool EnableCH { get; set; } = true;
            [JsonProperty("Swap Hands (Left-Handed Mode) (unused)")] public bool LeftHandMode { get; set; } = false;
            [JsonProperty("Enable arms (unused)")] public bool FloatingHands { get; set; } = false;
            [JsonProperty("Controller-based shooting (unused)")] public bool ControllerShooty { get; set; } = true;
            [JsonProperty("Movement-based punching (unused)")] public bool PunchIrl { get; set; } = true;
        }

        public VRInputSettings()
        {
            Hands = new HandsSettings();
        }
    }
}

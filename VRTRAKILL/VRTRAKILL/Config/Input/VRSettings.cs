using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Input
{
    internal class VRSettings
    {
        // placeholder
        [JsonProperty] public static float Deadzone => 0.4f;
        [JsonProperty] public static bool SnapTurning => false; // placeholder
        [JsonProperty] public static int SnapTurningAngles => 45; // placeholder
        [JsonProperty] public static int SmoothTurningSpeed => 300;
    }
}

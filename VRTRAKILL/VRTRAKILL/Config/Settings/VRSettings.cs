using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings
{
    internal class VRSettings
    {
        [JsonProperty("VR UI Settings")] public VRUISettings VRUI { get; set; } public class VRUISettings
        {
            [JsonProperty("Enable default crosshair")] public bool EnableDefaultCrosshair { get; set; } = false;
            [JsonProperty("HUD Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;
            [JsonProperty("Enabke standard HUD")] public bool EnableStandardHUD { get; set; } = false;
        }

        [JsonProperty("Do not override movement values")] public bool DoNotOverrideMoveValues { get; set; } = false;

        [JsonProperty("Controller Lines (unused)")] public ControllerLines CL { get; set; } public class ControllerLines
        {
            [JsonProperty("Draw controller lines")] public bool DrawControllerLines { get; set; } = false;
            [JsonProperty("Controller line initial transparency (from 0 to 1)")] public float LInitTransparency { get; set; } = 0.4f;
            [JsonProperty("Controller line end transparency (from 0 to 1)")] public float LEndTransparency { get; set; } = 0.1f;
        }

        [JsonProperty("Desktop View (for recording, etc.)")] public DesktopView DV { get; set; } public class DesktopView
        {
            [JsonProperty("Enable Desktop View")] public bool EnableDV { get; set; } = true;
            [JsonProperty("World View FOV")] public float WorldCamFOV { get; set; } = 90;
            [JsonProperty("UI View FOV")] public float UICamFOV { get; set; } = 45;
        }

        public VRSettings()
        {
            VRUI = new VRUISettings();
            CL = new ControllerLines();
            DV = new DesktopView();
        }
    }
}

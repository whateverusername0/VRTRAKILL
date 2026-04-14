using Newtonsoft.Json;

namespace VRTRAKILL.Prefs;

public class VrtrakillConfigJSON
{
    [JsonIgnore] public UKBindings.ModifiedActions UKBinds { get; set; }

    [JsonProperty("Frame update multiplier")] public float UpdateMultiplier { get; set; } = 0.575f;
    [JsonProperty("UI Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;

    [JsonProperty("Controller Settings")] public ControllerSettings Controllers { get; set; }

    [JsonProperty("MBP Settings")] public PunchSettings MBP { get; set; }

    [JsonProperty("Avatar Settings")] public AvatarSettings Avatar { get; set; }

    [JsonProperty("DesktopView Settings")] public DesktopViewSettings DesktopView { get; set; }

    public class ControllerSettings
    {
        [JsonProperty("Deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
        [JsonProperty("Smooth turning speed")] public float SmoothSpeed { get; set; } = 300;
        [JsonProperty("Snap turning")] public bool SnapTurn { get; set; } = false;
        [JsonProperty("Snap turn frequency")] public float SnapTurnSpeed { get; set; } = .2f;
        [JsonProperty("Snap turning angles")] public float SnapAngles { get; set; } = 45;
        [JsonProperty("Draw controller models")] public bool DrawControllers { get; set; } = true;
        [JsonProperty("Enable Crosshair")] public bool EnableCrosshair { get; set; } = true;
        [JsonProperty("Crosshair distance")] public float CrosshairDistance { get; set; } = 8;
    }

    public class PunchSettings
    {
        [JsonProperty("Required punching velocity")] public float PunchingSpeed { get; set; } = 7.5f;
        [JsonProperty("WHIPLASH: camera-based aiming")] public bool CameraWhiplash { get; set; } = false;
    }

    public class AvatarSettings
    {
        [JsonProperty("Draw arms")] public bool DrawArms { get; set; } = true;
        [JsonProperty("Draw legs")] public bool DrawLegs { get; set; } = false;
    }

    public class DesktopViewSettings
    {
        [JsonProperty("Enabled")] public bool Enabled { get; set; } = true;
        [JsonProperty("World view FOV")] public float WorldCamFOV { get; set; } = 90;
        [JsonProperty("UI view FOV")] public float UICamFOV { get; set; } = 90;
    }

    public VrtrakillConfigJSON()
    {
        UKBinds = UKBindings.GetBinds();
        Controllers = new ControllerSettings();
        MBP = new PunchSettings();
        Avatar = new AvatarSettings();
        DesktopView = new DesktopViewSettings();
    }
}
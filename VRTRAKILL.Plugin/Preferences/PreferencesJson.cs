using Newtonsoft.Json;

namespace VRTRAKILL.Plugin.Preferences;

public class PreferencesJson
{
    public float InterfaceScale { get; set; } = .0625f;

    public float ControllerDeadzone { get; set; } = .4f;

    public bool SnapTurning { get; set; } = false;

    public float SmoothTurnSpeed { get; set; } = 300f;

    public float SnapTurnSpeed { get; set; } = .2f;

    public float SnapAnglesPerTurn { get; set; } = 45f;

    public float CrosshairDistance { get; set; } = 8f;

    public float RequiredPunchSpeed { get; set; } = 7.5f;

    public bool CameraBasedWhiplash { get; set; } = false;
}
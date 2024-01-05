using Newtonsoft.Json;
using System.IO;
using VRBasePlugin.ULTRAKILL;

#pragma warning disable IDE1006 // Naming Styles
namespace VRBasePlugin.Prefs
{
    public class NewConfig
    {
        [JsonIgnore] public UKBindings.ModifiedActions UKBinds { get; set; }
        [JsonProperty("VRTRAKILL Keybinds")] public _VRBinds VRBinds { get; set; } public class _VRBinds
        {
            [JsonProperty("Toggle Desktop View")] public string ToggleDV { get; set; } = "T";
            [JsonProperty("Toggle Size Adjustment")] public string ToggleAvatarSizeAdj { get; set; } = "J";
        }

        [JsonProperty("Movement multiplier")] public float MovementMultiplier { get; set; } = 0.575f;

        [JsonProperty("Controller Settings")] public _Controllers Controllers { get; set; } public class _Controllers
        {
            [JsonProperty("Deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
            [JsonProperty("Smooth turning speed")] public float SmoothSpeed { get; set; } = 300;
            [JsonProperty("Snap turning")] public bool SnapTurn { get; set; } = false;
            [JsonProperty("Snap turning angles")] public float SnapAngles { get; set; } = 45;

            [JsonProperty("Draw controller models")] public bool DrawControllers { get; set; } = true;
            [JsonProperty("Enable Controller Rumble")] public bool EnableHaptics { get; set; } = true;
            [JsonProperty("Left handed (BROKEN)")] public bool LeftHanded { get; set; } = false;
        }

        [JsonProperty("Enable controller-based aiming")] public bool EnableCBS { get; set; } = true;
        [JsonProperty("CBS Settings")] public _CBS CBS { get; set; } public class _CBS
        {
            [JsonProperty("Enable Crosshair")] public bool EnableCrosshair { get; set; } = true;
            [JsonProperty("Crosshair distance")] public float CrosshairDistance { get; set; } = 8;
        }

        [JsonProperty("Enable movement-based punching")] public bool EnableMBP { get; set; } = true;
        [JsonProperty("MBP Settings")] public _MBP MBP { get; set; } public class _MBP
        {
            [JsonProperty("Velocity-based punching direction")] public bool ToggleVelocity { get; set; } = false;
            [JsonProperty("Required speed to punch")] public float PunchingSpeed { get; set; } = 7.5f;
            [JsonProperty("WHIPLASH: camera-based aiming")] public bool CameraWhiplash { get; set; } = false;
        }

        [JsonProperty("Enable VRAvatar")] public bool EnableVRBody { get; set; } = true;
        [JsonProperty("VRAvatar Settings")] public _VRBody VRBody { get; set; } public class _VRBody
        {
            [JsonProperty("Enable arms IK")] public bool EnableArmsIK { get; set; } = true;
            [JsonProperty("Enable legs IK")] public bool EnableLegsIK { get; set; } = true;

            public _VRBody()
            {

            }
        }

        [JsonProperty("UI Interaction Settings")] public _UIInteraction UIInteraction { get; set; } public class _UIInteraction
        {
            [JsonProperty("UI Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;
            [JsonProperty("Controller-based")] public bool ControllerBased { get; set; } = true;

            [JsonProperty("Controller Lines")] public _ControllerLines ControllerLines { get; set; } public class _ControllerLines
            {
                [JsonProperty("Enabled")] public bool Enabled { get; set; } = true;
                [JsonProperty("Starting transparency (from 0 to 1)")] public float StartAlpha { get; set; } = 0.4f;
                [JsonProperty("End transparency (from 0 to 1)")] public float EndAlpha { get; set; } = 0.1f;
            }

            public _UIInteraction()
            {
                ControllerLines = new _ControllerLines();
            }
        }

        [JsonProperty("DesktopView Settings")] public _DesktopView DesktopView { get; set; } public class _DesktopView
        {
            [JsonProperty("Enabled by default")] public bool Enabled { get; set; } = true;
            [JsonProperty("World view FOV")] public float WorldCamFOV { get; set; } = 90;
            [JsonProperty("UI view FOV")] public float UICamFOV { get; set; } = 90;
        }

        [JsonProperty("Miscellaneous/Unsorted Settings")] public _Misc Misc { get; set; } public class _Misc
        {
            //[JsonProperty("Enable 4S FPS Camera (BROKEN)")] public bool Enable4SFPSCam { get; set; } = false;
        }

        public NewConfig()
        {
            UKBinds = UKBindings.GetBinds().Actions;
            VRBinds = new _VRBinds();
            Controllers = new _Controllers();
            CBS = new _CBS();
            MBP = new _MBP();
            VRBody = new _VRBody();
            UIInteraction = new _UIInteraction();
            DesktopView = new _DesktopView();
            Misc = new _Misc();
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
using Newtonsoft.Json;
using System.IO;

#pragma warning disable IDE1006 // Naming Styles
namespace Plugin.VRTRAKILL.Config
{
    public class NewConfig
    {
        [JsonProperty("ULTRAKILL Keybinds")] public _UKKeybinds UKKeybinds { get; set; } public class _UKKeybinds
        {
            [JsonProperty("Fire")] public string Shoot { get; set; } = "MouseLeft";
            [JsonProperty("Alternative Fire")] public string AltShoot { get; set; } = "MouseRight";
            [JsonProperty("Punch")] public string Punch { get; set; } = "F";

            [JsonProperty("Jump")] public string Jump { get; set; } = "Spacebar";
            [JsonProperty("Slide")] public string Slide { get; set; } = "LeftControl";
            [JsonProperty("Dash")] public string Dash { get; set; } = "LeftShift";

            [JsonProperty("Last weapon used")] public string LastWeaponUsed { get; set; } = "Q";
            [JsonProperty("Change weapon variation")] public string ChangeWeaponVariation { get; set; } = "E";
            [JsonProperty("Iterate weapon (weapon scroll)")] public string IterateWeapon { get; set; } = "MouseScroll";
            [JsonProperty("Swap hand")] public string SwapHand { get; set; } = "G";
            [JsonProperty("Whiplash")] public string Whiplash { get; set; } = "R";

            [JsonProperty("Escape (pause, etc.)")] public string Escape { get; set; } = "Escape";

            [JsonProperty("Slot 1")] public string Slot1 { get; set; } = "1";
            [JsonProperty("Slot 2")] public string Slot2 { get; set; } = "2";
            [JsonProperty("Slot 3")] public string Slot3 { get; set; } = "3";
            [JsonProperty("Slot 4")] public string Slot4 { get; set; } = "4";
            [JsonProperty("Slot 5")] public string Slot5 { get; set; } = "5";
            [JsonProperty("Slot 6")] public string Slot6 { get; set; } = "6";
            [JsonProperty("Slot 7")] public string Slot7 { get; set; } = "7";
            [JsonProperty("Slot 8")] public string Slot8 { get; set; } = "8";
            [JsonProperty("Slot 9")] public string Slot9 { get; set; } = "9";
            [JsonProperty("Slot 0")] public string Slot0 { get; set; } = "0";
        }
        [JsonProperty("VRTRAKILL Keybinds")] public _VRKeybinds VRKeybinds { get; set; } public class _VRKeybinds
        {
            [JsonProperty("Toggle Desktop View")] public string ToggleDV { get; set; } = "T";
            [JsonProperty("Toggle Spectator Camera")] public string ToggleSC { get; set; } = "Y";
            [JsonProperty("Switch Spectator Camera mode")] public string EnumSCMode { get; set; } = "H";
            [JsonProperty("Rotate/Move SpecCam Up")] public string SpecCamUp { get; set; } = "UpArrow";
            [JsonProperty("Rotate/Move SpecCam Down")] public string SpecCamDown { get; set; } = "DownArrow";
            [JsonProperty("Rotate/Move SpecCam Left")] public string SpecCamLeft { get; set; } = "LeftArrow";
            [JsonProperty("Rotate/Move SpecCam Right")] public string SpecCamRight { get; set; } = "RightArrow";
            [JsonProperty("Spectator Camera move mode (hold and use with the keys above)")] public string SpecCamMoveMode { get; set; } = "RightShift";
        }

        [JsonProperty("Movement multiplier")] public float MovementMultiplier { get; set; } = 0.575f;
        [JsonProperty("Controller Settings")] public _ControllerSettings Controllers { get; set; } public class _ControllerSettings
        {
            [JsonProperty("Deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
            [JsonProperty("Smooth turning speed")] public float SmoothSpeed { get; set; } = 300;
            [JsonProperty("Snap turning")] public bool SnapTurn { get; set; } = false;
            [JsonProperty("Snap turning angles")] public float SnapAngles { get; set; } = 45;

            [JsonProperty("Draw controllers in the main menu")] public bool DrawControllers { get; set; } = true;
            [JsonProperty("Enable Controller Rumble")] public bool EnableHaptics { get; set; } = true;
            [JsonProperty("Left handed? (BROKEN)")] public bool LeftHanded { get; set; } = false;
        }

        [JsonProperty("Enable controller-based shooting")] public bool EnableCBS { get; set; } = true;
        [JsonProperty("CBS Settings")] public _CBS CBS { get; set; } public class _CBS
        {
            [JsonProperty("Enable Crosshair")] public bool EnableCrosshair { get; set; } = true;
            [JsonProperty("Crosshair distance")] public float CrosshairDistance { get; set; } = 8;
        }

        [JsonProperty("Enable movement-based punching")] public bool EnableMBP { get; set; } = true;
        [JsonProperty("MBP Settings")] public _MBP MBP { get; set; } public class _MBP
        {
            [JsonProperty("Velocity-based punching direction?")] public bool ToggleVelocity { get; set; } = false;
            [JsonProperty("Required punching speed")] public float PunchingSpeed { get; set; } = 7.5f;
            [JsonProperty("Enable coin throwing from the non-dominant hand")] public bool EnableNDHCoinThrow { get; set; } = true;
            [JsonProperty("WHIPLASH: Enable camera-based aiming")] public bool CameraWhiplash { get; set; } = false;
        }

        [JsonProperty("Enable VRAvatar")] public bool EnableVRBody { get; set; } = true;
        [JsonProperty("VRAvatar Settings")] public _VRBody VRBody { get; set; } public class _VRBody
        {
            [JsonProperty("Enable skins")] public bool EnableSkins { get; set; } = false;
            [JsonProperty("Skins *(ONLY ONE MUST BE CHOSEN)*")] public _Skins Skins { get; set; } public class _Skins
            {
                [JsonProperty("V1 (Default)")] public bool V1 { get; set; } = true;
                [JsonProperty("V2 (PLACEHOLDER. NOT IMPLEMENTED YET)")] public bool V2 { get; set; } = false;
            }

            public _VRBody()
            {
                Skins = new _Skins();
            }
        }

        [JsonProperty("UI Interaction Settings")] public _UIInteraction UIInteraction { get; set; } public class _UIInteraction
        {
            [JsonProperty("UI Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;
            [JsonProperty("Controller-based?")] public bool ControllerBased { get; set; } = true;

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

            [JsonProperty("Spectator Camera")] public _SpecCam SpectatorCamera { get; set; } public class _SpecCam
            {
                [JsonProperty("Enabled by default (replaces dv pov)")] public bool Enabled { get; set; } = false;
                [JsonProperty("Mode (0: follow, 1: rotate, 2: fixed)")] public int Mode { get; set; } = 0;
            }

            public _DesktopView()
            {
                SpectatorCamera = new _SpecCam();
            }
        }

        [JsonProperty("Miscellaneous (or unsorted) Settings")] public _Misc Misc { get; set; } public class _Misc
        {
            //[JsonProperty("Enable 4S FPS Camera (BROKEN)")] public bool Enable4SFPSCam { get; set; } = false;
        }

        public NewConfig()
        {
            UKKeybinds = new _UKKeybinds();
            VRKeybinds = new _VRKeybinds();
            Controllers = new _ControllerSettings();
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
using Newtonsoft.Json;

#pragma warning disable IDE1006 // Naming Styles
namespace Plugin.VRTRAKILL.Config
{
    public class NewConfig
    {
        [JsonProperty("Keybinds *(HAVE TO MATCH WITH THE GAME)*")] public _Keybinds Keybinds { get; set; } public class _Keybinds
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

        [JsonProperty("Controller Settings")] public _ControllerSettings ControllerSettings { get; set; } public class _ControllerSettings
        {
            [JsonProperty("Deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
            [JsonProperty("Smooth turning speed")] public float SmoothTurningSpeed { get; set; } = 300;
            [JsonProperty("Snap turning")] public bool SnapTurning { get; set; } = false;
            [JsonProperty("Snap turning angles")] public float SnapTurningAngles { get; set; } = 45;
            [JsonProperty("Enable Controller Haptics (Vibration)")] public bool EnableControllerHaptics { get; set; } = true;
        }

        [JsonProperty("Enable controller-based shooting")] public bool EnableCBS { get; set; } = true;
        [JsonProperty("CBS Settings")] public _CBS CBS { get; set; } public class _CBS
        {

        }

        [JsonProperty("Enable movement-based punching")] public bool EnableMBP { get; set; } = true;
        [JsonProperty("MBP Settings")] public _MBP MBP { get; set; } public class _MBP
        {
            [JsonProperty("Required punching speed")] public float PunchingSpeed { get; set; } = 7.5f;
            [JsonProperty("WHIPLASH: Enable camera-based aiming")] public bool CameraWhiplash { get; set; } = false;
            [JsonProperty("Enable coin throwing from the non-dominant hand")] public bool EnableNDHandCoinThrow { get; set; } = true;
            [JsonProperty("Enable velocity-based direction")] public bool EnablePunchingVelocity { get; set; } = false;
        }

        [JsonProperty("Enable VRAvatar")] public bool EnableVRBody { get; set; } = true;
        [JsonProperty("VRAvatar Settings")] public _VRBody VRBody { get; set; } public class _VRBody
        {

        }

        [JsonProperty("UI")] public _UI UI { get; set; } public class _UI
        {
            [JsonProperty("Enable standard HUD (replaces classic hud)")] public bool EnableStandardHUD { get; set; } = true;
            [JsonProperty("HUD Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;
            [JsonProperty("Crosshair distance")] public float CrosshairDistance { get; set; } = 8;
        }

        [JsonProperty("UI Interaction")] public _UIInteraction UIInteraction { get; set; } public class _UIInteraction
        {
            [JsonProperty("Enable UI Pointer")] public bool EnableUIPointer { get; set; } = true;
            [JsonProperty("Controller-based? (BROKEN)")] public bool ControllerBased { get; set; } = false;

            [JsonProperty("Controller Lines")] public _ControllerLines ControllerLines { get; set; } public class _ControllerLines
            {
                [JsonProperty("Enabled")] public bool Enabled { get; set; } = false;
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
            [JsonProperty("Enabled")] public bool EnableDV { get; set; } = true;
            [JsonProperty("World view FOV")] public float WorldCamFOV { get; set; } = 90;
            [JsonProperty("UI view FOV")] public float UICamFOV { get; set; } = 90;
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
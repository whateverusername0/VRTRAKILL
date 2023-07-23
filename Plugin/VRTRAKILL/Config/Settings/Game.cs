using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings
{
    internal class Game
    {
        [JsonProperty("Player movement multiplier (speed, jump power, etc.)")] public float MovementMultiplier { get; set; } = .57f;
        [JsonProperty("Enable first-person 4-S camera (broken)")] public bool EnableFP4SCam { get; set; } = false;

        [JsonProperty("Controller-based shooting")] public ControllerBasedShooting CBS { get; set; } public class ControllerBasedShooting
        {
            [JsonProperty("Enable controller-based shooting")] public bool EnableControllerShooting { get; set; } = true;
        }

        [JsonProperty("Movement-based punching")] public MovementBasedPunching MBP { get; set; } public class MovementBasedPunching
        {
            [JsonProperty("Enable movement-based punching")] public bool EnableMovementPunching { get; set; } = true;
            [JsonProperty("Required speed to punch (default: 7.5f)")] public float PunchingSpeed { get; set; } = 7.5f;
            [JsonProperty("WHIPLASH: Disable controller aiming (enable camera aim)")] public bool DisableControllerAiming { get; set; } = false;
            [JsonProperty("Enable coin throwing from hand (not from the gun)")] public bool EnableHandCoinThrow { get; set; } = true;
            [JsonProperty("Enable punch direction detection by velocity (instead of what's infront of you)")] public bool EnablePunchingVelocity { get; set; } = false;
        }
        [JsonProperty("Hand Gestures (unused)")] public HandGestures HG { get; set; } public class HandGestures
        {
            [JsonProperty("Enable hand gestures")] public bool EnableHandGestures { get; set; } = true;
            [JsonProperty("Replace pointing gesture with a middle finger")] public bool EnableMiddleFinger { get; set; } = false;
        }
        [JsonProperty("VR Body")] public VRBody VRB { get; set; } public class VRBody
        {
            [JsonProperty("Enable VRIK")] public bool EnableVRIK { get; set; } = true;
        }

        public Game()
        {
            CBS = new ControllerBasedShooting();
            MBP = new MovementBasedPunching();
            HG = new HandGestures();
            VRB = new VRBody();

            if (!MBP.EnableMovementPunching) HG.EnableHandGestures = false;
        }
    }
}

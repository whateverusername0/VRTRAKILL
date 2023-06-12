using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings
{
    public class Game
    {
        [JsonProperty("Do not override movement values")] public bool DoNotOverrideMoveValues { get; set; } = false;
        [JsonProperty("Enable first-person 4-S camera")] public bool EnableFP4SCam { get; set; } = true;

        [JsonProperty("Controller-based shooting")] public ControllerBasedShooting CBS { get; set; } public class ControllerBasedShooting
        {
            [JsonProperty("Enable controller-based shooting")] public bool EnableControllerShooting { get; set; } = true;
        }

        [JsonProperty("Movement-based punching")] public MovementBasedPunching MBP { get; set; } public class MovementBasedPunching
        {
            [JsonProperty("Enable movement-based punching")] public bool EnableMovementPunching { get; set; } = true;
            [JsonProperty("WHIPLASH: Disable controller aiming (enable camera aim)")] public bool DisableControllerAiming { get; set; } = false;
        }
        [JsonProperty("Hand Gestures (unused)")] public HandGestures HG { get; set; } public class HandGestures
        {
            [JsonProperty("Enable hand gestures")] public bool EnableHandGestures { get; set; } = true;
            [JsonProperty("Replace pointing gesture with a middle finger")] public bool EnableMiddleFinger { get; set; } = false;
        }

        public Game()
        {
            CBS = new ControllerBasedShooting();
            MBP = new MovementBasedPunching();
            HG = new HandGestures();

            if (!MBP.EnableMovementPunching) HG.EnableHandGestures = false;
        }
    }
}

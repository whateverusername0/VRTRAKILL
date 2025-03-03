using Newtonsoft.Json;
using System.IO;

#pragma warning disable IDE1006 // Naming Styles
namespace ULTRAVR.Configuration
{
    public class JSONNewConfig
    {
        // since ultrakill is running on basic Update and headset refresh rates are different we gotta do sth about it.
        [JsonProperty("Movement multiplier")] public float MovementMultiplier { get; set; } = 0.575f;

        [JsonIgnore] public ActionMap.ModifiedActions UKBinds { get; set; }
        [JsonProperty("VRTRAKILL Keybinds")] public _VRBinds VRBinds { get; set; } public class _VRBinds
        {
            [JsonProperty("Toggle Desktop View")] public string ToggleDV { get; set; } = "T";
        }

        [JsonProperty("Controllers")] public _Controllers Controllers { get; set; } public class _Controllers
        {
            [JsonProperty("Deadzone (from 0 to 1)")] public float Deadzone { get; set; } = 0.4f;
            [JsonProperty("Smooth turning speed")] public float SmoothSpeed { get; set; } = 300;
            [JsonProperty("Snap turning")] public bool SnapTurn { get; set; } = false;
            [JsonProperty("Snap turning angles")] public float SnapAngles { get; set; } = 45;

            [JsonProperty("Draw controller models")] public bool DrawControllers { get; set; } = true;
            [JsonProperty("Enable Haptics")] public bool EnableHaptics { get; set; } = true;
        }

        [JsonProperty("Gameplay")] public _Gameplay CBS { get; set; } public class _Gameplay
        {
            [JsonProperty("Crosshair distance")] public float CrosshairDistance { get; set; } = 8;
            [JsonProperty("Required speed to punch")] public float PunchingSpeed { get; set; } = 7.5f;
        }

        [JsonProperty("Accessibility")] public _Accessibility Acc { get; set; } public class _Accessibility
        {
            [JsonProperty("WHIPLASH: camera-based aiming")] public bool CameraWhiplash { get; set; } = false;
        }

        [JsonProperty("In-game body")] public _Avatar Avatar { get; set; } public class _Avatar
        {
            [JsonProperty("Enabled")] public bool Enabled { get; set; } = false;
            [JsonProperty("IK Arms")] public bool Arms { get; set; } = true;
            [JsonProperty("IK Legs")] public bool Legs { get; set; } = true;
        }

        [JsonProperty("UI")] public _UIInteraction UIInteraction { get; set; } public class _UIInteraction
        {
            [JsonProperty("UI Size (from 0 to 0.1)")] public float UISize { get; set; } = 0.0625f;

            [JsonProperty("Controller Lines")] public _ControllerLines ControllerLines { get; set; } public class _ControllerLines
            {
                [JsonProperty("Enabled")] public bool Enabled { get; set; } = true;
                [JsonProperty("Start alpha (from 0 to 1)")] public float StartAlpha { get; set; } = 0.4f;
                [JsonProperty("End alpha (from 0 to 1)")] public float EndAlpha { get; set; } = 0.1f;
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

        public JSONNewConfig()
        {
            UKBinds = ActionMap.GetBinds();
            VRBinds = new _VRBinds();
        }

        public static JSONNewConfig Deserialize(string filepath)
        {
            try
            {
                var json = File.ReadAllText(filepath);
                var config = JsonConvert.DeserializeObject<JSONNewConfig>(json);
                return config;
            }
            catch (FileNotFoundException)
            {
                Plugin.Log.LogError(
                    "Unable to find VRTRAKILL_Config.json." +
                    "Generating a new one. Please quit the game and fill it out." +
                    "Starting up using default settings.");

                Serialize(new JSONNewConfig(), filepath);
                return new JSONNewConfig();
            }
            catch (JsonException)
            {
                Plugin.Log.LogError(
                    "Something went wrong during parsing VRTRAKILL_Config.json" +
                    "\nFix any typos, formatting errors, etc." +
                    "\nOr delete the config and let it generate once more." +
                    "\nStarting up using default settings.");

                return new JSONNewConfig();
            }
        }
        public static void Serialize(JSONNewConfig config, string filepath)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(filepath, json);
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
using Newtonsoft.Json;
using System.IO;

namespace Plugin.VRTRAKILL.Config
{
    public class ConfigJSON
    {
        [JsonProperty("Gameplay Settings")] public Settings.Game Game { get; set; }
        [JsonProperty("Input Settings")] public Settings.Input.InputS Input { get; set; }
        [JsonProperty("Controller Settings")] public Settings.Controllers.ControllerS Controllers { get; set; }
        [JsonProperty("View Settings")] public Settings.View.ViewS View { get; set; }

        public static ConfigJSON Instance { get; private set; }

        public ConfigJSON()
        {
            Game = new Settings.Game();
            Input = new Settings.Input.InputS();
            Controllers = new Settings.Controllers.ControllerS();
            View = new Settings.View.ViewS();
        }

        public static ConfigJSON GetConfig()
        {
            if (Instance != null) return Instance;
            else { Instance = Deserialize(); return Instance; }
        }

        public static ConfigJSON Deserialize()
        {
            try
            {
                string Temp = File.ReadAllText(ConfigMaster.ConfigPath);
                ConfigJSON Config = JsonConvert.DeserializeObject<ConfigJSON>(Temp);
                return Config;
            }
            catch (FileNotFoundException)
            {
                Plugin.PLog.LogError("Unable to find VRTRAKILL_Config.json.\n" +
                                        "Generating a new one. Please quit the game and fill it out.\n" +
                                        "Starting up using default settings.");
                Serialize(new ConfigJSON()); return new ConfigJSON();
            }
            catch (JsonException)
            {
                Plugin.PLog.LogError("Something went wrong when trying to read VRTRAKILL_Config.json\n" +
                                        "Please fix any typos, formatting errors, etc. Perhaps regenerate config.\n" +
                                        "Starting up using default settings.");
                return new ConfigJSON();
            }
        }
        public static void Serialize(ConfigJSON Config)
        {
            string JSON = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(ConfigMaster.ConfigPath, JSON);
        }
    }
}

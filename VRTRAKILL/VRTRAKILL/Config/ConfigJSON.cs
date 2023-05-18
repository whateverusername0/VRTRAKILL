﻿using Newtonsoft.Json;
using System.IO;

namespace Plugin.VRTRAKILL.Config
{
    public class ConfigJSON
    {
        [JsonProperty("VR Settings")] public Settings.VRSettings VRSettings { get; set; }
        [JsonProperty("VR Input Settings")] public Input.VRInputSettings VRInputSettings { get; set; }
        [JsonProperty("Keybinds")] public Input.Keybinds Inputs { get; set; }

        public ConfigJSON()
        {
            VRSettings = new Settings.VRSettings();
            VRInputSettings = new Input.VRInputSettings();
            Inputs = new Input.Keybinds();
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
                Plugin.PLogger.LogError("Unable to find VRTRAKILL_Config.json.\n" +
                                        "Generating a new one. Please quit the game and fill it out.\n" +
                                        "Starting up using default settings.");
                Serialize(new ConfigJSON()); return new ConfigJSON();
            }
            catch (JsonException)
            {
                Plugin.PLogger.LogError("Something went wrong when trying to read VRTRAKILL_Config.json\n" +
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

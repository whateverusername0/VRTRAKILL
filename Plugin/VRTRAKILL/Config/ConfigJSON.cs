using Newtonsoft.Json;
using System.IO;

namespace Plugin.VRTRAKILL.Config
{
    public class ConfigJSON
    {
        [JsonProperty("VRTRAKILL Settings")] public NewConfig Config { get; set; }

        public static ConfigJSON Instance { get; set; }

        public ConfigJSON()
        {
            Config = new NewConfig();
        }

        public static ConfigJSON GetConfig()
        {
            if (Instance == null) Instance = Deserialize();
            return Instance;
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
                Vars.Log.LogError("Unable to find VRTRAKILL_Config.json.\n" +
                                     "Generating a new one. Please quit the game and fill it out.\n" +
                                     "Starting up using default settings.");
                Serialize(new ConfigJSON()); return new ConfigJSON();
            }
            catch (JsonException)
            {
                Vars.Log.LogError("Something went wrong when trying to read VRTRAKILL_Config.json\n" +
                                     "Please fix any typos, formatting errors, etc.\n" +
                                     "Or delete the config and let it generate once more.\n" +
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

using Newtonsoft.Json;
using System.IO;

namespace Plugin.VRTRAKILL.Config
{
    internal class ConfigJSON
    {
        public Input.VRSettings VRSettings { get; set; }
        public Input.LegacyInput LegacyInputs { get; set; }

        public ConfigJSON Deserialize()
        {
            try
            {
                string Temp = File.ReadAllText(ConfigMaster.ConfigPath);
                ConfigJSON Config = JsonConvert.DeserializeObject<ConfigJSON>(Temp);
                return Config;
            }
            catch (FileNotFoundException)
            {
                Plugin.PLogger.LogError("Unable to find VRTRAKILL_Config.json, without it you literally cannot use the mod.\n" +
                                        "Generating a new one. Please quit the game and fill it out, ty.");
                Serialize(new ConfigJSON());
            }
            return null;
        }
        public static void Serialize(ConfigJSON Config)
        {
            string JSON = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(ConfigMaster.ConfigPath, JSON);
        }
    }
}

using System.Collections.Generic;
using WindowsInput.Native;
using Newtonsoft.Json;
using System.IO;

namespace Plugin.VRTRAKILL.Config
{
    public class ConfigJSON
    {
        // this is straight forward boilerplate, it's not even needed most of the part, lol
        public string JumpKey { get; set; }
        public string SlideKey { get; set; }
        public string DashKey { get; set; }

        public string OpenWeaponWheelieKey { get; set; }
        public string IterateWeaponKey { get; set; }
        public string ChangeWeaponVariationKey { get; set; }
        public string SwapHandKey { get; set; }

        public string PauseKey { get; set; }



        public static ConfigJSON Deserialize()
        {
            try
            {
                string Temp = File.ReadAllText(ConfigMaster.ConfigPath);
                ConfigJSON Config = JsonConvert.DeserializeObject<ConfigJSON>(Temp);
                return Config;
            } catch (FileNotFoundException)
            {
                Plugin.PLogger.LogError("Unable to find VRTRAKILL_Config.json, without it you cannot fully ULTRAKILL, because there'll be no input.\n" +
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

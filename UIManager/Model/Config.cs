using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace UIManager.Model
{
    public class Config
    {
        [JsonIgnore] public static readonly string ConfigPath = $"{App.APPLICATIONPATH}\\Config.json";
        [JsonProperty] public string ULTRAKILLPath { get; set; } = "C:\\Games\\ULTRAKILL";

        private static Config Deserialize(string FullPath)
        {
            try { return JsonConvert.DeserializeObject<Config>(File.ReadAllText(FullPath)); }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Unable to find the config file for VRTRAKILLMan. Generating a new one.",
                                "VRTRAKILL Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                Serialize(new Config()); return new Config();
            }
            catch (JsonException)
            {
                MessageBox.Show("Something went wrong when trying to deserialize VRTRAKILLMan's config file." +
                                "Before pressing OK, make a backup of your existing config" +
                                "in case you have made changes that you don't want to lose.",
                                "VRTRAKILL Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                Serialize(new Config()); return new Config();
            }
        }
        private static void Serialize(Config Config, string JSONPath = null)
        {
            if (string.IsNullOrWhiteSpace(JSONPath)) JSONPath = ConfigPath;
            File.WriteAllText(JSONPath, JsonConvert.SerializeObject(Config, Formatting.Indented));
        }

        [JsonIgnore] public static Config _Instance { get; set; }
        [JsonIgnore] public Config Instance => _Instance;
        public static Config GetInstance()
        {
            if (_Instance == null) _Instance = Deserialize(ConfigPath);
            return _Instance;
        }
    }
}

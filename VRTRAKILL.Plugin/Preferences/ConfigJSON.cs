using Newtonsoft.Json;
using System.IO;
using VRTRAKILL.Data;

namespace VRTRAKILL.Prefs;

public class ConfigJSON
{
    public static string ConfigPath = $"{PluginInfo.PluginPath}\\VRTRAKILL_Config.json";
    [JsonProperty("VRTRAKILL Settings")] public VrtrakillConfigJSON Config { get; set; }

    private static ConfigJSON _instance { get; set; }
    public static ConfigJSON Instance
    {
        get
        {
            if (_instance == null) _instance = Deserialize();
            return _instance;
        }
    }

    public ConfigJSON()
    {
        Config = new VrtrakillConfigJSON();
    }

    public static ConfigJSON Deserialize()
    {
        try
        {
            string Temp = File.ReadAllText(ConfigPath);
            ConfigJSON Config = JsonConvert.DeserializeObject<ConfigJSON>(Temp);
            return Config;
        }
        catch (FileNotFoundException)
        {
            GlobalVars.Log.LogError("Unable to find VRTRAKILL_Config.json.\n" +
                              "Generating a new one. Please quit the game and fill it out.\n" +
                              "Starting up using default settings.");
            Serialize(new ConfigJSON()); return new ConfigJSON();
        }
        catch (JsonException)
        {
            GlobalVars.Log.LogError("Something went wrong when trying to read VRTRAKILL_Config.json\n" +
                              "Please fix any typos, formatting errors, etc.\n" +
                              "Or delete the config and let it generate once more.\n" +
                              "Starting up using default settings.");
            return new ConfigJSON();
        }
    }

    public static void Serialize(ConfigJSON Config)
    {
        string JSON = JsonConvert.SerializeObject(Config, Formatting.Indented);
        File.WriteAllText(ConfigPath, JSON);
    }
}

using Newtonsoft.Json;
using System.IO;

namespace VRTRAKILL.Plugin.Preferences;

public class PluginPreferencesManager : MonoSingleton<PluginPreferencesManager>
{
    private static Plugin _plugin => Plugin.Instance;

    [JsonIgnore] public static string ConfigPath = $"{PluginInfo.PluginPath}\\VRTRAKILL_Config.json";

    public PreferencesJson Config { get; private set; }

    public static void Serialize(PluginPreferencesManager config)
    {
        var json = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(ConfigPath, json);
    }

    protected void Awake()
    {
        Config = new();

        try
        {
            var json = File.ReadAllText(ConfigPath);
            var config = JsonConvert.DeserializeObject<PluginPreferencesManager>(json);
            Config = config.Config;
        }
        catch (FileNotFoundException)
        {
            _plugin.Log.LogError(
                "Unable to find VRTRAKILL_Config.json\n" +
                "Generating a new one. Please quit the game and fill it out.\n" +
                "Starting up using default settings.");

            Serialize(new PluginPreferencesManager());
            return;
        }
        catch (JsonException)
        {
            _plugin.Log.LogError(
                "Something went wrong when trying to read VRTRAKILL_Config.json\n" +
                "Please fix any typos, formatting errors, etc.\n" +
                "Or delete the config and let it generate once more.\n" +
                "Starting up using default settings.");

            return;
        }
    }
}

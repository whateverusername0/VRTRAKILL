using Newtonsoft.Json;

namespace ULTRAVR.Input.Actions
{
    /// <summary>
    ///     Represents the Binds.json file located in ULTRAKILL\\Preferences\\Binds.json
    /// </summary>
    public class UltrakillBindings
    {
        [JsonProperty("controlScheme")] public string ControlScheme { get; set; } = "Keyboard & Mouse";
        [JsonProperty("modifiedActions")] public ModifiedActionMap Actions { get; set; }
    }
}

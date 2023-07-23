using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.Input
{
    internal class InputS
    {
        [JsonProperty("Input")] public InputSettings InputSettings { get; set; }
        [JsonProperty("Keybinds")] public Keybinds Keybinds { get; set; }

        public InputS()
        {
            InputSettings = new InputSettings();
            Keybinds = new Keybinds();
        }
    }
}

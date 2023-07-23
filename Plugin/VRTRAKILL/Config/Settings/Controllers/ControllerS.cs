using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.Controllers
{
    internal class ControllerS
    {
        [JsonProperty("Use controller-based UI interaction (broken)")] public bool UseControllerUIInteraction { get; set; }
        [JsonProperty("Controller Lines")] public ControllerLines CLines { get; set; }
        [JsonProperty("Hands")] public HandSettings HandS { get; set; }

        public ControllerS()
        {
            CLines = new ControllerLines();
            HandS = new HandSettings();
        }
    }
}

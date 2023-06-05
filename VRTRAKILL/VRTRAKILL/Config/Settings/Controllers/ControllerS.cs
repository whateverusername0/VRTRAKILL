using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.Controllers
{
    public class ControllerS
    {
        [JsonProperty("Controller Lines")] public ControllerLines CLines { get; set; }
        [JsonProperty("Hands")] public HandSettings HandS { get; set; }

        public ControllerS()
        {
            CLines = new ControllerLines();
            HandS = new HandSettings();
        }
    }
}

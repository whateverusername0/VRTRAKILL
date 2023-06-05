using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.Controllers
{
    public class ControllerLines
    {
        [JsonProperty("Draw controller lines")] public bool DrawControllerLines { get; set; } = false;
        [JsonProperty("Initial transparency (from 0 to 1)")] public float LInitTransparency { get; set; } = 0.4f;
        [JsonProperty("End transparency (from 0 to 1)")] public float LEndTransparency { get; set; } = 0.1f;
    }
}

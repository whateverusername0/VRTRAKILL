using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Settings.View
{
    public class ViewS
    {
        [JsonProperty("VR UI")] public UI VRUI { get; set; }
        [JsonProperty("Desktop View (for recording, etc.)")] public DesktopView DV { get; set; }

        public ViewS()
        {
            VRUI = new UI();
            DV = new DesktopView();
        }
    }
}

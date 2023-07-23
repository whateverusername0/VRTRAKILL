using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.VRTRAKILL.Config.Settings.View
{
    public class DesktopView
    {
        [JsonProperty("Enable Desktop View")] public bool EnableDV { get; set; } = true;
        [JsonProperty("World View FOV")] public float WorldCamFOV { get; set; } = 90;
        [JsonProperty("UI View FOV")] public float UICamFOV { get; set; } = 45;
    }
}

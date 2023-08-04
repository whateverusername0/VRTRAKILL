using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config
{
    public class NewConfig
    {



        [JsonProperty("Enable controller-based shooting")] public bool EnableCBS { get; set; } = true;

        [JsonProperty("Emable movement-based punching")] public bool EnableMBP { get; set; } = true;
        [JsonProperty("")] public _MBP MBP { get; set; } public class _MBP
        {

        }
        // velocity-based

        [JsonProperty("")] public bool EnableVRBody { get; set; } = true;
        [JsonProperty("")] public _VRBody VRBody { get; set; } public class _VRBody
        {

        }

        [JsonProperty("UI Interaction")] public _UIInteraction UIInteraction { get; set; } public class _UIInteraction
        {
            [JsonProperty("Enable UI Pointer")] public bool EnableUIPointer { get; set; } = true;

            [JsonProperty("Controller-based?")] public bool ControllerBased { get; set; } = false;
            [JsonProperty("Controller Lines")] public _ControllerLines ControllerLines { get; set; } public class _ControllerLines
            {
                [JsonProperty("Enabled")] public bool DrawControllerLines { get; set; } = false;
                [JsonProperty("Starting transparency (from 0 to 1)")] public float StartAlpha { get; set; } = 0.4f;
                [JsonProperty("End transparency (from 0 to 1)")] public float EndAlpha { get; set; } = 0.1f;
            }

            public _UIInteraction()
            {
                ControllerLines = new _ControllerLines();
            }
        }
    }
}

using Newtonsoft.Json;

namespace Plugin.VRTRAKILL.Config.Input
{
    public class Keybinds
    {
        [JsonProperty("Jump")] public string JumpKey { get; set; } = "Spacebar";
        [JsonProperty("Slide")] public string SlideKey { get; set; } = "LeftControl";
        [JsonProperty("Dash")] public string DashKey { get; set; } = "LeftShift";

        [JsonProperty("Last weapon used")] public string LastWeaponUsedKey { get; set; } = "Q";
        [JsonProperty("Change weapon variation")] public string ChangeWeaponVariationKey { get; set; } = "E";
        [JsonProperty("Iterate weapon (weapon scroll)")] public string IterateWeaponKey { get; set; } = "MouseScroll";
        [JsonProperty("Swap hand")] public string SwapHandKey { get; set; } = "G";
        [JsonProperty("Whiplash")] public string WhiplashKey { get; set; } = "R";

        [JsonProperty("Escape (pause, etc.)")] public string EscapeKey { get; set; } = "Escape";

        [JsonProperty("Slot 1")] public string Slot1Key { get; set; } = "1";
        [JsonProperty("Slot 2")] public string Slot2Key { get; set; } = "2";
        [JsonProperty("Slot 3")] public string Slot3Key { get; set; } = "3";
        [JsonProperty("Slot 4")] public string Slot4Key { get; set; } = "4";
        [JsonProperty("Slot 5")] public string Slot5Key { get; set; } = "5";
        [JsonProperty("Slot 6")] public string Slot6Key { get; set; } = "6";
        [JsonProperty("Slot 7")] public string Slot7Key { get; set; } = "7";
        [JsonProperty("Slot 8")] public string Slot8Key { get; set; } = "8";
        [JsonProperty("Slot 9")] public string Slot9Key { get; set; } = "9";
        [JsonProperty("Slot 0")] public string Slot0Key { get; set; } = "0";
    }
}

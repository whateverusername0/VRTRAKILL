using Newtonsoft.Json;
using System.IO;

namespace Plugin.VRTRAKILL.Config.Input
{
    public class LegacyInput
    {
        [JsonProperty] public static bool Enabled = true; // placeholder

        public string WalkForwardKey => "W";
        public string WalkBackwardKey => "S";
        public string WalkLeftKey => "A";
        public string WalkRightKey => "D";

        public string JumpKey => "Spacebar";
        public string SlideKey => "LeftControl";
        public string DashKey => "LeftShift";

        public string LastWeaponUsedKey => "Q";
        public string ChangeWeaponVariationKey => "";
        public string IterateWeaponKey => "MouseScroll";
        public string SwapHandKey => "G";
        public string WhiplashKey => "R";

        public string EscapeKey => "Escape"; // used for pause and other things

        public string Slot1Key => "1";
        public string Slot2Key => "2";
        public string Slot3Key => "3";
        public string Slot4Key => "4";
        public string Slot5Key => "5";
        public string Slot6Key => "6";
        public string Slot7Key => "7";
        public string Slot8Key => "8";
        public string Slot9Key => "9";
        public string Slot0Key => "0";

        public static LegacyInput Deserialize()
        {
            try
            {
                string Temp = File.ReadAllText(ConfigMaster.ConfigPath);
                LegacyInput Config = JsonConvert.DeserializeObject<LegacyInput>(Temp);
                return Config;
            } catch (FileNotFoundException)
            {
                Plugin.PLogger.LogError("Unable to find VRTRAKILL_Config.json, without it you cannot fully ULTRAKILL, because there'll be no input.\n" +
                                        "Generating a new one. Please quit the game and fill it out, ty.");
                Serialize(new LegacyInput());
            }
            return null;
        }
        public static void Serialize(LegacyInput Config)
        {
            string JSON = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(ConfigMaster.ConfigPath, JSON);
        }
    }
}

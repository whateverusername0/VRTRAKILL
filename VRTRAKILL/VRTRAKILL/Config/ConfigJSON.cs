using System.Collections.Generic;
using Newtonsoft.Json;
using WindowsInput.Native;

namespace Plugin.VRTRAKILL.Config
{
    public class ConfigJSON
    {
        // this is straight forward boilerplate, it's not even needed most of the part, lol
        public string JumpKey { get; set; }
        public string SlideKey { get; set; }
        public string DashKey { get; set; }

        public string OpenWeaponWheelieKey { get; set; }
        public string IterateWeaponKey { get; set; }
        public string ChangeWeaponVariationKey { get; set; }
        public string SwapHand { get; set; }

        public string Pause { get; set; }



        public static ConfigJSON Deserialize()
        {

            return null;
        }
    }
}

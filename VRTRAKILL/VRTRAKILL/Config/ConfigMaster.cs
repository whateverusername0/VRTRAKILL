using System;
using WindowsInput.Native;

namespace Plugin.VRTRAKILL.Config
{
    internal class ConfigMaster
    {
        public static string ConfigPath = $"{Plugin.GamePath}\\BepInEx\\plugins\\VRTRAKILL_Config.json";

        public static VirtualKeyCode
            Jump,
            Slide,
            Dash,

            OpenWeaponWheelie,
            IterateWeapon,
            ChangeWeaponVariation,

            SwapHand,

            Pause;

        public static void Init()
        {
            ConfigJSON Config = ConfigJSON.Deserialize();
            ConvertJSONToKeys(Config);
        }

        private static void ConvertJSONToKeys(ConfigJSON Config)
        {
            try
            {
                InputMap.Keys.TryGetValue(Config.JumpKey, out Jump);
                InputMap.Keys.TryGetValue(Config.SlideKey, out Slide);
                InputMap.Keys.TryGetValue(Config.DashKey, out Dash);

                InputMap.Keys.TryGetValue(Config.OpenWeaponWheelieKey, out OpenWeaponWheelie);
                InputMap.Keys.TryGetValue(Config.IterateWeaponKey, out IterateWeapon);
                InputMap.Keys.TryGetValue(Config.ChangeWeaponVariationKey, out ChangeWeaponVariation);

                InputMap.Keys.TryGetValue(Config.SwapHandKey, out SwapHand);

                InputMap.Keys.TryGetValue(Config.PauseKey, out Pause);
            }
            catch (Exception)
            { Plugin.PLogger.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps null keys?" +
                                      "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it."); }
        }
    }
}

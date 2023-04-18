using System;
using WindowsInput.Native;

namespace Plugin.VRTRAKILL.Config
{
    internal class ConfigMaster
    {
        public static string ConfigPath = $"{Plugin.GamePath}\\BepInEx\\plugins\\VRTRAKILL_Config.json";

        public static VirtualKeyCode
            WalkForward, WalkBackward,
            WalkLeft, WalkRight,

            Jump, Slide, Dash,

            LastWeaponUsed,
            ChangeWeaponVariation,
            IterateWeapon,

            SwapHand,
            Whiplash,

            Escape,
            
            Slot0, Slot1, Slot2,
            Slot3, Slot4, Slot5,
            Slot6, Slot7, Slot8, Slot9;

        public static void Init()
        {
           /*if (Input.LegacyInput.Enabled)
            {*/
            ConfigJSON Config = ConfigJSON.Deserialize();
            Input.LegacyInput LegacyInput = Config.LegacyInputs; ConvertJSONToKeys(LegacyInput);
          /*}*/
        }

        private static void ConvertJSONToKeys(Input.LegacyInput Config)
        {
            try
            {
                LegacyInputMap.Keys.TryGetValue(Config.WalkForwardKey, out WalkForward);
                LegacyInputMap.Keys.TryGetValue(Config.WalkBackwardKey, out WalkBackward);
                LegacyInputMap.Keys.TryGetValue(Config.WalkLeftKey, out WalkLeft);
                LegacyInputMap.Keys.TryGetValue(Config.WalkRightKey, out WalkRight);

                LegacyInputMap.Keys.TryGetValue(Config.JumpKey, out Jump);
                LegacyInputMap.Keys.TryGetValue(Config.SlideKey, out Slide);
                LegacyInputMap.Keys.TryGetValue(Config.DashKey, out Dash);

                LegacyInputMap.Keys.TryGetValue(Config.LastWeaponUsedKey, out LastWeaponUsed);
                LegacyInputMap.Keys.TryGetValue(Config.ChangeWeaponVariationKey, out ChangeWeaponVariation);
                LegacyInputMap.Keys.TryGetValue(Config.IterateWeaponKey, out IterateWeapon);

                LegacyInputMap.Keys.TryGetValue(Config.SwapHandKey, out SwapHand);
                LegacyInputMap.Keys.TryGetValue(Config.WhiplashKey, out Whiplash);

                LegacyInputMap.Keys.TryGetValue(Config.EscapeKey, out Escape);

                LegacyInputMap.Keys.TryGetValue(Config.Slot0Key, out Slot0);
                LegacyInputMap.Keys.TryGetValue(Config.Slot1Key, out Slot1);
                LegacyInputMap.Keys.TryGetValue(Config.Slot2Key, out Slot2);
                LegacyInputMap.Keys.TryGetValue(Config.Slot3Key, out Slot3);
                LegacyInputMap.Keys.TryGetValue(Config.Slot4Key, out Slot4);
                LegacyInputMap.Keys.TryGetValue(Config.Slot5Key, out Slot5);
                LegacyInputMap.Keys.TryGetValue(Config.Slot6Key, out Slot6);
                LegacyInputMap.Keys.TryGetValue(Config.Slot7Key, out Slot7);
                LegacyInputMap.Keys.TryGetValue(Config.Slot8Key, out Slot8);
                LegacyInputMap.Keys.TryGetValue(Config.Slot9Key, out Slot9);
            }
            catch (Exception)
            { Plugin.PLogger.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps keys are literally null?" +
                                      "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it."); }
        }
    }
}
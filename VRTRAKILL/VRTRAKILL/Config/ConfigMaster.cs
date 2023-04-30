using System;
using WindowsInput.Native;

namespace Plugin.VRTRAKILL.Config
{
    internal class ConfigMaster
    {
        public static string ConfigPath = $"{Plugin.GamePath}\\BepInEx\\plugins\\VRTRAKILL_Config.json";

        public static VirtualKeyCode
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
            Input.Keybinds Keybinds = Vars.Config.Inputs; ConvertJSONToKeys(Keybinds);
        }

        private static void ConvertJSONToKeys(Input.Keybinds Config)
        {
            try
            {
                InputMap.Keys.TryGetValue(Config.JumpKey, out Jump);
                InputMap.Keys.TryGetValue(Config.SlideKey, out Slide);
                InputMap.Keys.TryGetValue(Config.DashKey, out Dash);

                InputMap.Keys.TryGetValue(Config.LastWeaponUsedKey, out LastWeaponUsed);
                InputMap.Keys.TryGetValue(Config.ChangeWeaponVariationKey, out ChangeWeaponVariation);
                InputMap.Keys.TryGetValue(Config.IterateWeaponKey, out IterateWeapon);

                InputMap.Keys.TryGetValue(Config.SwapHandKey, out SwapHand);
                InputMap.Keys.TryGetValue(Config.WhiplashKey, out Whiplash);

                InputMap.Keys.TryGetValue(Config.EscapeKey, out Escape);

                InputMap.Keys.TryGetValue(Config.Slot0Key, out Slot0);
                InputMap.Keys.TryGetValue(Config.Slot1Key, out Slot1);
                InputMap.Keys.TryGetValue(Config.Slot2Key, out Slot2);
                InputMap.Keys.TryGetValue(Config.Slot3Key, out Slot3);
                InputMap.Keys.TryGetValue(Config.Slot4Key, out Slot4);
                InputMap.Keys.TryGetValue(Config.Slot5Key, out Slot5);
                InputMap.Keys.TryGetValue(Config.Slot6Key, out Slot6);
                InputMap.Keys.TryGetValue(Config.Slot7Key, out Slot7);
                InputMap.Keys.TryGetValue(Config.Slot8Key, out Slot8);
                InputMap.Keys.TryGetValue(Config.Slot9Key, out Slot9);
            }
            catch (Exception)
            {
                Plugin.PLogger.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps keys are literally null?" +
                                        "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it.");
            }
        }
    }
}
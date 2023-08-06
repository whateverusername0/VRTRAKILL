using System;
using WindowsInput;
using WindowsInput.Native;
using Plugin.VRTRAKILL.Input;

namespace Plugin.VRTRAKILL.Config
{
    internal class ConfigMaster
    {
        public static string ConfigPath = $"{Plugin.PluginPath}\\VRTRAKILL_Config.json";

        public static VirtualKeyCode?
            KShoot = null, KAltShoot = null, KPunch = null ,

            KJump = null, KSlide = null, KDash = null,

            KLastWeaponUsed = null,
            KChangeVariation = null,
            KIterateWeapon = null,

            KSwapHand = null,
            KWhiplash = null,

            KEscape = null,
            
            KSlot0 = null, KSlot1 = null, KSlot2 = null,
            KSlot3 = null, KSlot4 = null, KSlot5 = null,
            KSlot6 = null, KSlot7 = null, KSlot8 = null, KSlot9 = null;
        public static MouseButton?
            MShoot = null, MAltShoot = null, MPunch = null,

            MJump = null, MSlide = null, MDash = null,

            MLastWeaponUsed = null,
            MChangeVariation = null,
            MIterateWeapon = null,

            MSwapHand = null,
            MWhiplash = null,

            MEscape = null,

            MSlot0 = null, MSlot1 = null, MSlot2 = null,
            MSlot3 = null, MSlot4 = null, MSlot5 = null,
            MSlot6 = null, MSlot7 = null, MSlot8 = null, MSlot9 = null;

        public static void Init()
        {
            NewConfig._Keybinds Keybinds = Vars.Config.Keybinds;
            ConvertJSONToKeys(Keybinds);
        }

        private static void ConvertJSONToKeys(NewConfig._Keybinds Config)
        {
            try
            {
                // Keyboard values
                InputMap.Keys.TryGetValue(Config.Shoot, out KShoot);
                InputMap.Keys.TryGetValue(Config.AltShoot, out KAltShoot);
                InputMap.Keys.TryGetValue(Config.Punch, out KPunch);

                InputMap.Keys.TryGetValue(Config.Jump, out KJump);
                InputMap.Keys.TryGetValue(Config.Slide, out KSlide);
                InputMap.Keys.TryGetValue(Config.Dash, out KDash);

                InputMap.Keys.TryGetValue(Config.LastWeaponUsed, out KLastWeaponUsed);
                InputMap.Keys.TryGetValue(Config.ChangeWeaponVariation, out KChangeVariation);
                InputMap.Keys.TryGetValue(Config.IterateWeapon, out KIterateWeapon);

                InputMap.Keys.TryGetValue(Config.SwapHand, out KSwapHand);
                InputMap.Keys.TryGetValue(Config.Whiplash, out KWhiplash);

                InputMap.Keys.TryGetValue(Config.Escape, out KEscape);

                InputMap.Keys.TryGetValue(Config.Slot0, out KSlot0);
                InputMap.Keys.TryGetValue(Config.Slot1, out KSlot1);
                InputMap.Keys.TryGetValue(Config.Slot2, out KSlot2);
                InputMap.Keys.TryGetValue(Config.Slot3, out KSlot3);
                InputMap.Keys.TryGetValue(Config.Slot4, out KSlot4);
                InputMap.Keys.TryGetValue(Config.Slot5, out KSlot5);
                InputMap.Keys.TryGetValue(Config.Slot6, out KSlot6);
                InputMap.Keys.TryGetValue(Config.Slot7, out KSlot7);
                InputMap.Keys.TryGetValue(Config.Slot8, out KSlot8);
                InputMap.Keys.TryGetValue(Config.Slot9, out KSlot9);

                // Mouse values
                InputMap.MouseKeys.TryGetValue(Config.Shoot, out MShoot);
                InputMap.MouseKeys.TryGetValue(Config.AltShoot, out MAltShoot);
                InputMap.MouseKeys.TryGetValue(Config.Punch, out MPunch);

                InputMap.MouseKeys.TryGetValue(Config.Jump, out MJump);
                InputMap.MouseKeys.TryGetValue(Config.Slide, out MSlide);
                InputMap.MouseKeys.TryGetValue(Config.Dash, out MDash);

                InputMap.MouseKeys.TryGetValue(Config.LastWeaponUsed, out MLastWeaponUsed);
                InputMap.MouseKeys.TryGetValue(Config.ChangeWeaponVariation, out MChangeVariation);
                InputMap.MouseKeys.TryGetValue(Config.IterateWeapon, out MIterateWeapon);

                InputMap.MouseKeys.TryGetValue(Config.SwapHand, out MSwapHand);
                InputMap.MouseKeys.TryGetValue(Config.Whiplash, out MWhiplash);

                InputMap.MouseKeys.TryGetValue(Config.Escape, out MEscape);

                InputMap.MouseKeys.TryGetValue(Config.Slot0, out MSlot0);
                InputMap.MouseKeys.TryGetValue(Config.Slot1, out MSlot1);
                InputMap.MouseKeys.TryGetValue(Config.Slot2, out MSlot2);
                InputMap.MouseKeys.TryGetValue(Config.Slot3, out MSlot3);
                InputMap.MouseKeys.TryGetValue(Config.Slot4, out MSlot4);
                InputMap.MouseKeys.TryGetValue(Config.Slot5, out MSlot5);
                InputMap.MouseKeys.TryGetValue(Config.Slot6, out MSlot6);
                InputMap.MouseKeys.TryGetValue(Config.Slot7, out MSlot7);
                InputMap.MouseKeys.TryGetValue(Config.Slot8, out MSlot8);
                InputMap.MouseKeys.TryGetValue(Config.Slot9, out MSlot9);
            }
            catch (Exception)
            {
                Plugin.PLog.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps keys are literally null?" +
                                        "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it.");
            }
        }
    }
}
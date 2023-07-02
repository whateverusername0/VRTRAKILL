using System;
using WindowsInput;
using WindowsInput.Native;

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
            Settings.Input.Keybinds Keybinds = Vars.Config.Input.Keybinds; ConvertJSONToKeys(Keybinds);
        }

        private static void ConvertJSONToKeys(Settings.Input.Keybinds Config)
        {
            try
            {
                // Keyboard values
                InputMap.Keys.TryGetValue(Config.ShootKey, out KShoot);
                InputMap.Keys.TryGetValue(Config.AltShootKey, out KAltShoot);
                InputMap.Keys.TryGetValue(Config.PunchKey, out KPunch);

                InputMap.Keys.TryGetValue(Config.JumpKey, out KJump);
                InputMap.Keys.TryGetValue(Config.SlideKey, out KSlide);
                InputMap.Keys.TryGetValue(Config.DashKey, out KDash);

                InputMap.Keys.TryGetValue(Config.LastWeaponUsedKey, out KLastWeaponUsed);
                InputMap.Keys.TryGetValue(Config.ChangeWeaponVariationKey, out KChangeVariation);
                InputMap.Keys.TryGetValue(Config.IterateWeaponKey, out KIterateWeapon);

                InputMap.Keys.TryGetValue(Config.SwapHandKey, out KSwapHand);
                InputMap.Keys.TryGetValue(Config.WhiplashKey, out KWhiplash);

                InputMap.Keys.TryGetValue(Config.EscapeKey, out KEscape);

                InputMap.Keys.TryGetValue(Config.Slot0Key, out KSlot0);
                InputMap.Keys.TryGetValue(Config.Slot1Key, out KSlot1);
                InputMap.Keys.TryGetValue(Config.Slot2Key, out KSlot2);
                InputMap.Keys.TryGetValue(Config.Slot3Key, out KSlot3);
                InputMap.Keys.TryGetValue(Config.Slot4Key, out KSlot4);
                InputMap.Keys.TryGetValue(Config.Slot5Key, out KSlot5);
                InputMap.Keys.TryGetValue(Config.Slot6Key, out KSlot6);
                InputMap.Keys.TryGetValue(Config.Slot7Key, out KSlot7);
                InputMap.Keys.TryGetValue(Config.Slot8Key, out KSlot8);
                InputMap.Keys.TryGetValue(Config.Slot9Key, out KSlot9);

                // Mouse values
                InputMap.MouseKeys.TryGetValue(Config.ShootKey, out MShoot);
                InputMap.MouseKeys.TryGetValue(Config.AltShootKey, out MAltShoot);
                InputMap.MouseKeys.TryGetValue(Config.PunchKey, out MPunch);

                InputMap.MouseKeys.TryGetValue(Config.JumpKey, out MJump);
                InputMap.MouseKeys.TryGetValue(Config.SlideKey, out MSlide);
                InputMap.MouseKeys.TryGetValue(Config.DashKey, out MDash);

                InputMap.MouseKeys.TryGetValue(Config.LastWeaponUsedKey, out MLastWeaponUsed);
                InputMap.MouseKeys.TryGetValue(Config.ChangeWeaponVariationKey, out MChangeVariation);
                InputMap.MouseKeys.TryGetValue(Config.IterateWeaponKey, out MIterateWeapon);

                InputMap.MouseKeys.TryGetValue(Config.SwapHandKey, out MSwapHand);
                InputMap.MouseKeys.TryGetValue(Config.WhiplashKey, out MWhiplash);

                InputMap.MouseKeys.TryGetValue(Config.EscapeKey, out MEscape);

                InputMap.MouseKeys.TryGetValue(Config.Slot0Key, out MSlot0);
                InputMap.MouseKeys.TryGetValue(Config.Slot1Key, out MSlot1);
                InputMap.MouseKeys.TryGetValue(Config.Slot2Key, out MSlot2);
                InputMap.MouseKeys.TryGetValue(Config.Slot3Key, out MSlot3);
                InputMap.MouseKeys.TryGetValue(Config.Slot4Key, out MSlot4);
                InputMap.MouseKeys.TryGetValue(Config.Slot5Key, out MSlot5);
                InputMap.MouseKeys.TryGetValue(Config.Slot6Key, out MSlot6);
                InputMap.MouseKeys.TryGetValue(Config.Slot7Key, out MSlot7);
                InputMap.MouseKeys.TryGetValue(Config.Slot8Key, out MSlot8);
                InputMap.MouseKeys.TryGetValue(Config.Slot9Key, out MSlot9);
            }
            catch (Exception)
            {
                Plugin.PLog.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps keys are literally null?" +
                                        "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it.");
            }
        }
    }
}
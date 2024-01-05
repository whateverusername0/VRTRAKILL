using System;
using WindowsInput;
using WindowsInput.Native;
using VRBasePlugin.ULTRAKILL.Input;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using VRBasePlugin.ULTRAKILL;

namespace VRBasePlugin.Prefs
{
    public sealed class ConfigMaster
    {
        public static string ConfigPath = $"{PluginInfo.PluginPath}\\VRTRAKILL_Config.json";

        public static VirtualKeyCode?
            KShoot = null, KAltShoot = null, KPunch = null ,

            KJump = null, KSlide = null, KDash = null,

            KLastWeapon = null, KPrevWeapon = null, KNextWeapon = null,
            KChangeVariation = null,

            KSwapHand = null,
            KWhiplash = null,
            
            KSlot0 = null, KSlot1 = null, KSlot2 = null,
            KSlot3 = null, KSlot4 = null, KSlot5 = null,
            KSlot6 = null, KSlot7 = null, KSlot8 = null, KSlot9 = null;
        public static MouseButton?
            MShoot = null, MAltShoot = null, MPunch = null,

            MJump = null, MSlide = null, MDash = null,

            MLastWeapon = null, MPrevWeapon = null, MNextWeapon = null,
            MChangeVariation = null,

            MSwapHand = null,
            MWhiplash = null,

            MSlot0 = null, MSlot1 = null, MSlot2 = null,
            MSlot3 = null, MSlot4 = null, MSlot5 = null,
            MSlot6 = null, MSlot7 = null, MSlot8 = null, MSlot9 = null;

        // joystickwhateverbutton is there because unity is angry when you pass it as null
        public static UnityEngine.KeyCode?
            ToggleDesktopView = UnityEngine.KeyCode.Joystick8Button9,

            TPCamUp = UnityEngine.KeyCode.Joystick8Button9,
            TPCamDown = UnityEngine.KeyCode.Joystick8Button9,
            TPCamLeft = UnityEngine.KeyCode.Joystick8Button9,
            TPCamRight = UnityEngine.KeyCode.Joystick8Button9,
            TPCamHoldMoveMode = UnityEngine.KeyCode.Joystick8Button9,

            ToggleAvatarSizeAdj = UnityEngine.KeyCode.Joystick8Button9;

        public static void Init()
        {
            ConvertJSONToKeys(Vars.Config.UKBinds);
            ConvertJSONToKeys(Vars.Config.VRBinds);
        }

        private static void ConvertJSONToKeys(UKBindings.ModifiedActions Binds)
        {
            try
            {
                #region Keys
                InputMap.Keys.TryGetValue(Binds.PrimaryFire[0].Path, out KShoot);
                InputMap.Keys.TryGetValue(Binds.SecondaryFire[0].Path, out KAltShoot);
                InputMap.Keys.TryGetValue(Binds.Punch[0].Path, out KPunch);

                InputMap.Keys.TryGetValue(Binds.Jump[0].Path, out KJump);
                InputMap.Keys.TryGetValue(Binds.Slide[0].Path, out KSlide);
                InputMap.Keys.TryGetValue(Binds.Dodge[0].Path, out KDash);

                InputMap.Keys.TryGetValue(Binds.LastWeapon[0].Path, out KLastWeapon);
                InputMap.Keys.TryGetValue(Binds.ChangeVariation[0].Path, out KChangeVariation);

                InputMap.Keys.TryGetValue(Binds.ChangeFist[0].Path, out KSwapHand);
                InputMap.Keys.TryGetValue(Binds.Hook[0].Path, out KWhiplash);

                InputMap.Keys.TryGetValue(Binds.Slot0[0].Path, out KSlot0);
                InputMap.Keys.TryGetValue(Binds.Slot1[0].Path, out KSlot1);
                InputMap.Keys.TryGetValue(Binds.Slot2[0].Path, out KSlot2);
                InputMap.Keys.TryGetValue(Binds.Slot3[0].Path, out KSlot3);
                InputMap.Keys.TryGetValue(Binds.Slot4[0].Path, out KSlot4);
                InputMap.Keys.TryGetValue(Binds.Slot5[0].Path, out KSlot5);
                InputMap.Keys.TryGetValue(Binds.Slot6[0].Path, out KSlot6);
                InputMap.Keys.TryGetValue(Binds.Slot7[0].Path, out KSlot7);
                InputMap.Keys.TryGetValue(Binds.Slot8[0].Path, out KSlot8);
                InputMap.Keys.TryGetValue(Binds.Slot9[0].Path, out KSlot9);
                #endregion

                #region MouseKeys
                InputMap.KeysM.TryGetValue(Binds.PrimaryFire[0].Path, out MShoot);
                InputMap.KeysM.TryGetValue(Binds.SecondaryFire[0].Path, out MAltShoot);
                InputMap.KeysM.TryGetValue(Binds.Punch[0].Path, out MPunch);

                InputMap.KeysM.TryGetValue(Binds.Jump[0].Path, out MJump);
                InputMap.KeysM.TryGetValue(Binds.Slide[0].Path, out MSlide);
                InputMap.KeysM.TryGetValue(Binds.Dodge[0].Path, out MDash);

                InputMap.KeysM.TryGetValue(Binds.LastWeapon[0].Path, out MLastWeapon);
                InputMap.KeysM.TryGetValue(Binds.ChangeVariation[0].Path, out MChangeVariation);

                InputMap.KeysM.TryGetValue(Binds.ChangeFist[0].Path, out MSwapHand);
                InputMap.KeysM.TryGetValue(Binds.Hook[0].Path, out MWhiplash);

                InputMap.KeysM.TryGetValue(Binds.Slot0[0].Path, out MSlot0);
                InputMap.KeysM.TryGetValue(Binds.Slot1[0].Path, out MSlot1);
                InputMap.KeysM.TryGetValue(Binds.Slot2[0].Path, out MSlot2);
                InputMap.KeysM.TryGetValue(Binds.Slot3[0].Path, out MSlot3);
                InputMap.KeysM.TryGetValue(Binds.Slot4[0].Path, out MSlot4);
                InputMap.KeysM.TryGetValue(Binds.Slot5[0].Path, out MSlot5);
                InputMap.KeysM.TryGetValue(Binds.Slot6[0].Path, out MSlot6);
                InputMap.KeysM.TryGetValue(Binds.Slot7[0].Path, out MSlot7);
                InputMap.KeysM.TryGetValue(Binds.Slot8[0].Path, out MSlot8);
                InputMap.KeysM.TryGetValue(Binds.Slot9[0].Path, out MSlot9);
                #endregion
            }
            catch (Exception)
            {
                Vars.Log.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps keys are null?" +
                                  "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it.");
            }
        }
        private static void ConvertJSONToKeys(NewConfig._VRBinds VRConfig)
        {
            try
            {
                InputMap.UKeys.TryGetValue(VRConfig.ToggleDV, out ToggleDesktopView);
                InputMap.UKeys.TryGetValue(VRConfig.ToggleAvatarSizeAdj, out ToggleAvatarSizeAdj);
            }
            catch (Exception)
            {
                Vars.Log.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps keys are null?" +
                                  "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it.");
            }
        }
    }
}
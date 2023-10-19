using System;
using WindowsInput;
using WindowsInput.Native;
using Plugin.VRTRAKILL.Input;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;

namespace Plugin.VRTRAKILL.Config
{
    internal sealed class ConfigMaster
    {
        public static string ConfigPath = $"{PluginInfo.PluginPath}\\VRTRAKILL_Config.json";

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

        // joystickwhateverbutton is there because unity is angry when you pass it as null
        public static UnityEngine.KeyCode?
            ToggleDesktopView = UnityEngine.KeyCode.Joystick8Button9,
            ToggleThirdPersonCamera = UnityEngine.KeyCode.Joystick8Button9,
            EnumTPCamMode = UnityEngine.KeyCode.Joystick8Button9,

            TPCamUp = UnityEngine.KeyCode.Joystick8Button9,
            TPCamDown = UnityEngine.KeyCode.Joystick8Button9,
            TPCamLeft = UnityEngine.KeyCode.Joystick8Button9,
            TPCamRight = UnityEngine.KeyCode.Joystick8Button9,
            TPCamHoldMoveMode = UnityEngine.KeyCode.Joystick8Button9,

            ToggleAvatarCalibration = UnityEngine.KeyCode.Joystick8Button9;

        public static void Init()
        {
            ConvertJSONToKeys(Vars.Config.UKKeybinds);
            ConvertJSONToKeys(Vars.Config.VRKeybinds);
        }

        private static void ConvertJSONToKeys(NewConfig._UKKeybinds Config)
        {
            try
            {
                #region Keys
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
                #endregion

                #region MouseKeys
                InputMap.KeysM.TryGetValue(Config.Shoot, out MShoot);
                InputMap.KeysM.TryGetValue(Config.AltShoot, out MAltShoot);
                InputMap.KeysM.TryGetValue(Config.Punch, out MPunch);

                InputMap.KeysM.TryGetValue(Config.Jump, out MJump);
                InputMap.KeysM.TryGetValue(Config.Slide, out MSlide);
                InputMap.KeysM.TryGetValue(Config.Dash, out MDash);

                InputMap.KeysM.TryGetValue(Config.LastWeaponUsed, out MLastWeaponUsed);
                InputMap.KeysM.TryGetValue(Config.ChangeWeaponVariation, out MChangeVariation);
                InputMap.KeysM.TryGetValue(Config.IterateWeapon, out MIterateWeapon);

                InputMap.KeysM.TryGetValue(Config.SwapHand, out MSwapHand);
                InputMap.KeysM.TryGetValue(Config.Whiplash, out MWhiplash);

                InputMap.KeysM.TryGetValue(Config.Escape, out MEscape);

                InputMap.KeysM.TryGetValue(Config.Slot0, out MSlot0);
                InputMap.KeysM.TryGetValue(Config.Slot1, out MSlot1);
                InputMap.KeysM.TryGetValue(Config.Slot2, out MSlot2);
                InputMap.KeysM.TryGetValue(Config.Slot3, out MSlot3);
                InputMap.KeysM.TryGetValue(Config.Slot4, out MSlot4);
                InputMap.KeysM.TryGetValue(Config.Slot5, out MSlot5);
                InputMap.KeysM.TryGetValue(Config.Slot6, out MSlot6);
                InputMap.KeysM.TryGetValue(Config.Slot7, out MSlot7);
                InputMap.KeysM.TryGetValue(Config.Slot8, out MSlot8);
                InputMap.KeysM.TryGetValue(Config.Slot9, out MSlot9);
                #endregion
            }
            catch (Exception)
            {
                Vars.Log.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps keys are null?" +
                                  "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it.");
            }
        }
        private static void ConvertJSONToKeys(NewConfig._VRKeybinds VRConfig)
        {
            try
            {
                InputMap.UKeys.TryGetValue(VRConfig.ToggleDV, out ToggleDesktopView);
                InputMap.UKeys.TryGetValue(VRConfig.ToggleTPC, out ToggleThirdPersonCamera);
                InputMap.UKeys.TryGetValue(VRConfig.EnumTPCMode, out EnumTPCamMode);
                InputMap.UKeys.TryGetValue(VRConfig.TPCamLeft, out TPCamLeft);
                InputMap.UKeys.TryGetValue(VRConfig.TPCamUp, out TPCamUp);
                InputMap.UKeys.TryGetValue(VRConfig.TPCamRight, out TPCamRight);
                InputMap.UKeys.TryGetValue(VRConfig.TPCamDown, out TPCamDown);
                InputMap.UKeys.TryGetValue(VRConfig.TPCamMoveMode, out TPCamHoldMoveMode);
            }
            catch (Exception)
            {
                Vars.Log.LogError("Unable to convert keys in config. Perhaps mismatch? Perhaps keys are null?" +
                                  "Check spelling and replace every null key either with \"\", \"Empty\" or assign a value to it.");
            }
        }

        private readonly PlayerInput PI = new PlayerInput();
        public string GetBindingString(string action)
        {
            if (!PI.Actions.TryGetValue(action, out InputActionState value)) return null;

            ReadOnlyArray<InputBinding> Bindings = value.Action.bindings;
            for (int i = 0; i < Bindings.Count; i++)
            {
                if (Bindings[i].isComposite) return null;

                InputControl inputControl = InputSystem.FindControl(Bindings[i].path);
                if (inputControl == null && inputControl?.device is Keyboard) continue;

                return inputControl.displayName;
            }
            return null;
        }
    }
}
using WindowsInput;
using WindowsInput.Native;

namespace ULTRAVR.Input
{
    public class KeybindMap
    {
        // Dodge Slide Jump ChangeFist Punch Hook PrimaryFire SecondaryFire ChangeVariation
        // Slot0 Slot1 Slot2 Slot3 Slot4 Slot5 Slot6 Slot7 Slot8 Slot9
        // NextWeapon
        // PrevWeapon
        // LastWeapon

        public static VirtualKeyCode?
            KPrimaryFire = null,
            KSecondaryFire = null,
            KPunch = null,
            KJump = null,
            KSlide = null,
            KDodge = null,
            KLastWeapon = null,
            KPrevWeapon = null,
            KNextWeapon = null,
            KChangeVariation = null,
            KSwapHand = null,
            KWhiplash = null,
            KSlot0 = null,
            KSlot1 = null,
            KSlot2 = null,
            KSlot3 = null,
            KSlot4 = null,
            KSlot5 = null,
            KSlot6 = null,
            KSlot7 = null,
            KSlot8 = null,
            KSlot9 = null;

        public static MouseButton?
            MPrimaryFire = null,
            MSecondaryFire = null,
            MPunch = null,
            MJump = null,
            MSlide = null,
            MDodge = null,
            MLastWeapon = null,
            MPrevWeapon = null,
            MNextWeapon = null,
            MChangeVariation = null,
            MSwapHand = null,
            MWhiplash = null,
            MSlot0 = null,
            MSlot1 = null,
            MSlot2 = null,
            MSlot3 = null,
            MSlot4 = null,
            MSlot5 = null,
            MSlot6 = null,
            MSlot7 = null,
            MSlot8 = null,
            MSlot9 = null;

        // joystickwhateverbutton is there because unity is angry when you pass it as null
        public static UnityEngine.KeyCode?
            ToggleDesktopView = UnityEngine.KeyCode.Joystick8Button9,

            TPCamUp = UnityEngine.KeyCode.Joystick8Button9,
            TPCamDown = UnityEngine.KeyCode.Joystick8Button9,
            TPCamLeft = UnityEngine.KeyCode.Joystick8Button9,
            TPCamRight = UnityEngine.KeyCode.Joystick8Button9,
            TPCamHoldMoveMode = UnityEngine.KeyCode.Joystick8Button9,

            ToggleAvatarSizeAdj = UnityEngine.KeyCode.Joystick8Button9;
    }
}

using Plugin.Helpers;
using UnityEngine;

namespace Plugin.VRTRAKILL.Input.NewInput
{
    [ConfigureSingleton(SingletonFlags.PersistAutoInstance)] public class VRInputManager : MonoSingleton<VRInputManager>
    {
        private static bool
            WalkForward = false,  WalkForwardState = false,
            WalkBackward = false, WalkBackwardState = false,
            WalkLeft = false,     WalkLeftState = false,
            WalkRight = false,    WalkRightState = false,

            Jump = false,
            Dash = false,
            Slide = false;

        private static bool
            RHPrimaryFire = false,
            RHAltFire = false,
            ChangeWeaponVariation = false,
            OpenWeaponWheel = false;

        private static bool
            Punch = false,
            SwapHand = false,
            Whiplash = false;

        private static bool
            Slot0 = false,
            Slot1 = false, Slot2 = false, Slot3 = false,
            Slot4 = false, Slot5 = false, Slot6 = false,
            Slot7 = false, Slot8 = false, Slot9 = false;

        private static bool Escape;

        private void Update()
        {

        }
    }
}

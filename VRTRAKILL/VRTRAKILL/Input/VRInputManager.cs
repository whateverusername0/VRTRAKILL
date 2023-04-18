using UnityEngine;
using Valve.VR;
using WindowsInput;
using Plugin.VRTRAKILL.Config;
using Plugin.VRTRAKILL.Config.Input;

namespace Plugin.VRTRAKILL.Input
{
    internal static class VRInputManager
    {
        // Legacy Input System
        // Uses WindowsInput to simulate keypresses
        // To be replaced with hand system (either in 0.x or 1.x) // might not be replaced at all, because unity.inputsystem sucks ass

        private static bool
            Jump = false,
            Dash = false,
            Slide = false;

        private static bool
            RHPrimaryFire = false,
            RHAltFire = false,
            ChangeWeaponVariation = false,
            NextWeapon = false, PreviousWeapon = false,
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

        public static void Init()
        {
            // Movement
            SteamVR_Actions._default.Movement.AddOnUpdateListener(MovementH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Turn.AddOnUpdateListener(TurnH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Jump.AddOnUpdateListener(JumpH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slide.AddOnUpdateListener(SlideH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Dash.AddOnUpdateListener(DashH, SteamVR_Input_Sources.Any);

            // Weapons
            SteamVR_Actions._default.Shoot.AddOnUpdateListener(LHShootH, SteamVR_Input_Sources.LeftHand);
            SteamVR_Actions._default.AltShoot.AddOnUpdateListener(LHAltShootH, SteamVR_Input_Sources.LeftHand);
            SteamVR_Actions._default.Shoot.AddOnUpdateListener(RHShootH, SteamVR_Input_Sources.RightHand);
            SteamVR_Actions._default.AltShoot.AddOnUpdateListener(RHAltShootH, SteamVR_Input_Sources.RightHand);
            SteamVR_Actions._default.IterateWeapon.AddOnUpdateListener(IterateWeaponH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.ChangeWeaponVariation.AddOnUpdateListener(ChangeWeaponVariationH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.OpenWeaponWheel.AddOnUpdateListener(OpenWeaponWheelH, SteamVR_Input_Sources.Any);

            SteamVR_Actions._default.Slot0.AddOnUpdateListener(Slot0H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot1.AddOnUpdateListener(Slot1H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot2.AddOnUpdateListener(Slot2H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot2.AddOnUpdateListener(Slot2H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot3.AddOnUpdateListener(Slot3H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot4.AddOnUpdateListener(Slot4H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot5.AddOnUpdateListener(Slot5H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot6.AddOnUpdateListener(Slot6H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot7.AddOnUpdateListener(Slot7H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot8.AddOnUpdateListener(Slot8H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot9.AddOnUpdateListener(Slot9H, SteamVR_Input_Sources.Any);

            // Hands
            SteamVR_Actions._default.LHP.AddOnUpdateListener(SteamVR_Input_Sources.Any, LeftHandPoseH);
            SteamVR_Actions._default.RHP.AddOnUpdateListener(SteamVR_Input_Sources.Any, RightHandPoseH);

            SteamVR_Actions._default.SwapHand.AddOnUpdateListener(SwapHandH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Whiplash.AddOnUpdateListener(WhiplashH, SteamVR_Input_Sources.Any);

            SteamVR_Actions._default.Escape.AddOnUpdateListener(EscapeH, SteamVR_Input_Sources.Any);
        }

        private static void LeftHandPoseH(SteamVR_Action_Pose fromAction, SteamVR_Input_Sources fromSource)
        {
            // idk do something (lol)
        }
        private static void RightHandPoseH(SteamVR_Action_Pose fromAction, SteamVR_Input_Sources fromSource)
        {
            // idk do something (lol)
        }

        private static void MovementH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        { VRInputVars.MoveVector = axis; }
        private static void TurnH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            if (axis.x > 0 + VRSettings.Deadzone) VRInputVars.TurnOffset += VRSettings.SmoothTurningSpeed * Time.deltaTime;
            if (axis.x < 0 - VRSettings.Deadzone) VRInputVars.TurnOffset -= VRSettings.SmoothTurningSpeed * Time.deltaTime;
        }

        private static void JumpH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Jump) { Jump = newState; InputManager.Instance.InputSource.Jump.Trigger(Jump, !Jump); } }
        private static void SlideH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slide) { Slide = newState; InputManager.Instance.InputSource.Slide.Trigger(Slide, !Slide); } }
        private static void DashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Dash) { Dash = newState; InputManager.Instance.InputSource.Dodge.Trigger(Dash, !Dash); } }

        private static void LHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Punch) { Punch = newState; InputManager.Instance.InputSource.Punch.Trigger(Punch, !Punch); } }
        private static void LHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != SwapHand) { SwapHand = newState; InputManager.Instance.InputSource.ChangeFist.Trigger(SwapHand, !SwapHand); } }

        private static void RHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != RHPrimaryFire) { RHPrimaryFire = newState; InputManager.Instance.InputSource.Fire1.Trigger(RHPrimaryFire, !RHPrimaryFire); } }
        private static void RHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != RHAltFire) { RHAltFire = newState; InputManager.Instance.InputSource.Fire2.Trigger(RHAltFire, !RHAltFire); } }
        private static void IterateWeaponH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            if (axis.y > 0 + VRSettings.Deadzone * 1.5f) NextWeapon = true; else NextWeapon = false;
            if (axis.y < 0 - VRSettings.Deadzone * 1.5f) PreviousWeapon = true; else PreviousWeapon = false;

            if (NextWeapon) InputManager.Instance.InputSource.NextWeapon.Trigger(NextWeapon, !NextWeapon);
            if (PreviousWeapon) InputManager.Instance.InputSource.NextWeapon.Trigger(PreviousWeapon, !PreviousWeapon);
        }
        private static void ChangeWeaponVariationH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != ChangeWeaponVariation) { ChangeWeaponVariation = newState; InputManager.Instance.InputSource.ChangeVariation.Trigger(ChangeWeaponVariation, !ChangeWeaponVariation); } }
        private static void OpenWeaponWheelH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            // waiting for the new wheel to come out... for now it just triggers the last gun used
            if (newState != OpenWeaponWheel) { OpenWeaponWheel = newState; InputManager.Instance.InputSource.LastWeapon.Trigger(OpenWeaponWheel, !OpenWeaponWheel); }
        }

        private static void SwapHandH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != SwapHand) { SwapHand = newState; InputManager.Instance.InputSource.ChangeFist.Trigger(SwapHand, !SwapHand); } }
        private static void WhiplashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Whiplash) { Whiplash = newState; InputManager.Instance.InputSource.Hook.Trigger(Whiplash, !Whiplash); } }

        private static void Slot0H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot0) { Slot0 = newState; InputManager.Instance.InputSource.Slot0.Trigger(Slot0, !Slot0); } }
        private static void Slot1H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot1) { Slot1 = newState; InputManager.Instance.InputSource.Slot1.Trigger(Slot1, !Slot1); } }
        private static void Slot2H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot2) { Slot2 = newState; InputManager.Instance.InputSource.Slot2.Trigger(Slot2, !Slot2); } }
        private static void Slot3H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot3) { Slot3 = newState; InputManager.Instance.InputSource.Slot3.Trigger(Slot3, !Slot3); } }
        private static void Slot4H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot4) { Slot4 = newState; InputManager.Instance.InputSource.Slot4.Trigger(Slot4, !Slot4); } }
        private static void Slot5H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot5) { Slot5 = newState; InputManager.Instance.InputSource.Slot5.Trigger(Slot5, !Slot5); } }
        private static void Slot6H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot6) { Slot6 = newState; InputManager.Instance.InputSource.Slot6.Trigger(Slot6, !Slot6); } }
        private static void Slot7H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot7) { Slot7 = newState; InputManager.Instance.InputSource.Slot7.Trigger(Slot7, !Slot7); } }
        private static void Slot8H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot8) { Slot8 = newState; InputManager.Instance.InputSource.Slot8.Trigger(Slot8, !Slot8); } }
        private static void Slot9H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot9) { Slot9 = newState; InputManager.Instance.InputSource.Slot9.Trigger(Slot9, !Slot9); } }

        private static void EscapeH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Escape) { Escape = newState; TriggerKey(ConfigMaster.Escape, Escape, !Escape); } }

        private static void TriggerKey(WindowsInput.Native.VirtualKeyCode KeyCode, bool Started, bool Ended)
        {
            InputSimulator InpSim = new InputSimulator();
            if (Started) InpSim.Keyboard.KeyDown(KeyCode);
            else if (Ended) InpSim.Keyboard.KeyUp(KeyCode);
        }
        private static void MouseScroll(int Amount)
        {
            InputSimulator InpSim = new InputSimulator();
            InpSim.Mouse.VerticalScroll(Amount);
        }
        public static void Trigger(this InputActionState state, bool started, bool cancelled)
        {
            if (started)
            {
                state.IsPressed = true;
                state.PerformedFrame = Time.frameCount;
                state.PerformedTime = Time.time;
            }
            else if (cancelled)
            {
                state.IsPressed = false;
                state.CanceledFrame = Time.frameCount;
            }
        }
    }
}
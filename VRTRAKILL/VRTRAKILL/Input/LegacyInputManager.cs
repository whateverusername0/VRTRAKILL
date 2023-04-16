using UnityEngine;
using Valve.VR;
using Plugin.Helpers;
using WindowsInput;
using Plugin.VRTRAKILL.Config;
using Plugin.VRTRAKILL.Config.Input;

namespace Plugin.VRTRAKILL.Input
{
    internal static class LegacyInputManager
    {
        // Legacy Input System
        // Uses WindowsInput to simulate keypresses
        // Soon to be replaced with hand system (either in 0.x or 1.x)

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
            IterateWeapon = false,
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
            SteamVR_Actions._default.IterateWeapon.AddOnChangeListener(IterateWeaponH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.ChangeWeaponVariation.AddOnChangeListener(ChangeWeaponVariationH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.OpenWeaponWheel.AddOnChangeListener(OpenWeaponWheelH, SteamVR_Input_Sources.Any);

            SteamVR_Actions._default.Slot0.AddOnChangeListener(Slot0H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot1.AddOnChangeListener(Slot1H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot2.AddOnChangeListener(Slot2H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot3.AddOnChangeListener(Slot3H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot4.AddOnChangeListener(Slot4H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot5.AddOnChangeListener(Slot5H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot6.AddOnChangeListener(Slot6H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot7.AddOnChangeListener(Slot7H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot8.AddOnChangeListener(Slot8H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot9.AddOnChangeListener(Slot9H, SteamVR_Input_Sources.Any);

            // Hands
            SteamVR_Actions._default.LHP.AddOnChangeListener(SteamVR_Input_Sources.Any, LeftHandPoseH);
            SteamVR_Actions._default.RHP.AddOnChangeListener(SteamVR_Input_Sources.Any, RightHandPoseH);

            SteamVR_Actions._default.SwapHand.AddOnChangeListener(SwapHandH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Whiplash.AddOnChangeListener(WhiplashH, SteamVR_Input_Sources.Any);

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
        {
            VRInputVars.MoveVector = axis;

            // Used for noclip and rocket riding
            // For some reason does not affect regular walking
            if (axis.y > 0 + VRSettings.Deadzone) WalkForwardState = true; else WalkForwardState = false;
            if (axis.y < 0 - VRSettings.Deadzone) WalkBackwardState = true; else WalkBackwardState = false;
            if (axis.x > 0 + VRSettings.Deadzone) WalkLeftState = true; else WalkLeftState = false;
            if (axis.x < 0 - VRSettings.Deadzone) WalkRightState = true; else WalkRightState = false;

            if (WalkForwardState != WalkForward) { WalkForward = WalkForwardState; TriggerKey(ConfigMaster.WalkForward, WalkForward, !WalkForward); }
            if (WalkBackwardState != WalkBackward) { WalkBackward = WalkBackwardState; TriggerKey(ConfigMaster.WalkBackward, WalkBackward, !WalkBackward); }
            if (WalkLeftState != WalkLeft) { WalkLeft = WalkLeftState; TriggerKey(ConfigMaster.WalkLeft, WalkLeft, !WalkLeft); }
            if (WalkRightState != WalkRight) { WalkRight = WalkRightState; TriggerKey(ConfigMaster.WalkRight, WalkRight, !WalkRight); }
        }
        private static void TurnH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            if (axis.x > 0 + VRSettings.Deadzone) VRInputVars.TurnOffset += VRSettings.SmoothTurningSpeed * Time.deltaTime;
            if (axis.x < 0 - VRSettings.Deadzone) VRInputVars.TurnOffset -= VRSettings.SmoothTurningSpeed * Time.deltaTime;
        }

        private static void JumpH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Jump) { Jump = newState; TriggerKey(ConfigMaster.Jump, Jump, !Jump); } }
        private static void SlideH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slide) { Slide = newState; TriggerKey(ConfigMaster.Slide, Slide, !Slide); } }
        private static void DashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Dash) { Dash = newState; TriggerKey(ConfigMaster.Dash, Dash, !Dash); /* Dash is broken for some reason */ } }

        private static void LHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Punch) { Punch = newState; InputManager.Instance.InputSource.Punch.Trigger(Punch, !Punch); } }
        private static void LHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != SwapHand) { SwapHand = newState; TriggerKey(ConfigMaster.SwapHand, SwapHand, !SwapHand); } }

        private static void RHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != RHPrimaryFire) { RHPrimaryFire = newState; InputManager.Instance.InputSource.Fire1.Trigger(RHPrimaryFire, !RHPrimaryFire); } }
        private static void RHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != RHAltFire) { RHAltFire = newState; InputManager.Instance.InputSource.Fire2.Trigger(RHAltFire, !RHAltFire); } }
        private static void IterateWeaponH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            // crutch, wait for weapon wheel to come out
            if (axis.y > 0 + VRSettings.Deadzone) MouseScroll(1);
            if (axis.y < 0 - VRSettings.Deadzone) MouseScroll(-1);
        }
        private static void ChangeWeaponVariationH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != ChangeWeaponVariation) { SwapHand = newState; TriggerKey(ConfigMaster.ChangeWeaponVariation, ChangeWeaponVariation, !ChangeWeaponVariation); } }
        private static void OpenWeaponWheelH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            // waiting for the new wheel to come out... for now it just triggers the last gun used
            if (newState != OpenWeaponWheel) { OpenWeaponWheel = newState; TriggerKey(ConfigMaster.LastWeaponUsed, OpenWeaponWheel, !OpenWeaponWheel); }
        }

        private static void SwapHandH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != SwapHand) { SwapHand = newState; TriggerKey(ConfigMaster.SwapHand, SwapHand, !SwapHand); } }
        private static void WhiplashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Whiplash) { Whiplash = newState; TriggerKey(ConfigMaster.Whiplash, Whiplash, !Whiplash); } }

        private static void Slot0H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot0) { Slot0 = newState; TriggerKey(ConfigMaster.Slot0, Slot0, !Slot0); } }
        private static void Slot1H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot1) { Slot1 = newState; TriggerKey(ConfigMaster.Slot1, Slot1, !Slot1); } }
        private static void Slot2H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot2) { Slot2 = newState; TriggerKey(ConfigMaster.Slot2, Slot2, !Slot2); } }
        private static void Slot3H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot3) { Slot3 = newState; TriggerKey(ConfigMaster.Slot3, Slot3, !Slot3); } }
        private static void Slot4H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot4) { Slot4 = newState; TriggerKey(ConfigMaster.Slot4, Slot4, !Slot4); } }
        private static void Slot5H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot5) { Slot5 = newState; TriggerKey(ConfigMaster.Slot5, Slot5, !Slot5); } }
        private static void Slot6H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot6) { Slot6 = newState; TriggerKey(ConfigMaster.Slot6, Slot6, !Slot6); } }
        private static void Slot7H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot7) { Slot7 = newState; TriggerKey(ConfigMaster.Slot7, Slot7, !Slot7); } }
        private static void Slot8H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot8) { Slot8 = newState; TriggerKey(ConfigMaster.Slot8, Slot8, !Slot8); } }
        private static void Slot9H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot9) { Slot9 = newState; TriggerKey(ConfigMaster.Slot9, Slot9, !Slot9); } }

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
                ReadonlyModifier.SetValue(state, nameof(state.IsPressed), true);
                ReadonlyModifier.SetValue(state, nameof(state.PerformedFrame), Time.frameCount);
                ReadonlyModifier.SetValue(state, nameof(state.PerformedTime), Time.time);
            }
            else if (cancelled)
            {
                ReadonlyModifier.SetValue(state, nameof(state.IsPressed), false);
                ReadonlyModifier.SetValue(state, nameof(state.CanceledFrame), Time.frameCount);
            }
        }
    }
}

using UnityEngine;
using Valve.VR;
using Plugin.Helpers;
using WindowsInput;
using Plugin.VRTRAKILL.Config;

namespace Plugin.VRTRAKILL
{
    internal static class VRInputManager
    {
        // Movement
        public static Vector2 MoveVector = Vector2.zero;
        public static float TurnOffset = 0; public static float Deadzone = 0.4f;

        private static bool
            Jump = false,
            Dash = false,
            Slide = false,

            LHPrimaryFire = false,
            LHAltFire = false,
            RHPrimaryFire = false,
            RHAltFire = false;

        // temp, hand system later (lol)
        private static bool
            Punch = false,
            SwapHand = false,
            ChangeWeaponVariation = false;

        private static bool Pause;

        enum eTurnSpeed
        {
            Snail = 50,
            Normal = 100,
            Fast = 150,
            SuperFast = 200,
            SANIC = 300
        }
        private static float TurnSpeed = (float)eTurnSpeed.SANIC;

        public static void Init()
        {
            // Basic Movement
            SteamVR_Actions._default.Movement.AddOnUpdateListener(MovementH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Turn.AddOnUpdateListener(TurnH, SteamVR_Input_Sources.Any);

            // Weapons, WeaponWheelie
            SteamVR_Actions._default.Shoot.AddOnUpdateListener(LHShootH, SteamVR_Input_Sources.LeftHand);
            SteamVR_Actions._default.AltShoot.AddOnUpdateListener(LHAltShootH, SteamVR_Input_Sources.LeftHand);
            SteamVR_Actions._default.Shoot.AddOnUpdateListener(RHShootH, SteamVR_Input_Sources.RightHand);
            SteamVR_Actions._default.AltShoot.AddOnUpdateListener(RHAltShootH, SteamVR_Input_Sources.RightHand);

            // note: ADD WEAPONWHEELIE

            // Movement Buttons
            SteamVR_Actions._default.JumpSlam.AddOnUpdateListener(JumpSlamH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slide.AddOnUpdateListener(SlideH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Dash.AddOnUpdateListener(DashH, SteamVR_Input_Sources.Any);

            // For now it just changes weapons variation
            SteamVR_Actions._default.OpenWeaponWheel.AddOnUpdateListener(ChangeVariationH, SteamVR_Input_Sources.Any);

            SteamVR_Actions._default.Pause.AddOnUpdateListener(PauseH, SteamVR_Input_Sources.Any);
        }

        

        

        private static void MovementH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            MoveVector = axis;
        }
        private static void TurnH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            if (axis.x > 0 + Deadzone) TurnOffset += TurnSpeed * Time.deltaTime;
            if (axis.x < 0 - Deadzone) TurnOffset -= TurnSpeed * Time.deltaTime;
        }

        // note: rename JumpSlam to Jump
        private static void JumpSlamH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != Jump)
            {
                Jump = newState; TriggerKey(ConfigMaster.Jump, Jump, !Jump);
            }
        }
        private static void SlideH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != Slide)
            {
                Slide = newState; TriggerKey(ConfigMaster.Slide, Slide, !Slide);
            }
        }
        private static void DashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != Dash)
            {
                Dash = newState; TriggerKey(ConfigMaster.Dash, Dash, !Dash);
            }
        }

        // hand system later (DO IT LAZY BASTARD)
        private static void LHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != Punch)
            {
                Punch = newState;
                InputManager.Instance.InputSource.Punch.Trigger(Punch, !Punch);
            }
        }
        private static void LHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != SwapHand)
            {
                SwapHand = newState; TriggerKey(ConfigMaster.SwapHand, SwapHand, !SwapHand);
            }
        }

        private static void RHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != RHPrimaryFire)
            {
                RHPrimaryFire = newState;
                InputManager.Instance.InputSource.Fire1.Trigger(RHPrimaryFire, !RHPrimaryFire);
            }
        }
        private static void RHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != RHAltFire)
            {
                RHAltFire = newState;
                InputManager.Instance.InputSource.Fire2.Trigger(RHAltFire, !RHAltFire);
            }
        }

        private static void ChangeVariationH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != ChangeWeaponVariation)
            {
                SwapHand = newState;
                TriggerKey(ConfigMaster.ChangeWeaponVariation, ChangeWeaponVariation, !ChangeWeaponVariation);
            }
        }

        private static void PauseH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != Pause)
            {
                Pause = newState;
                TriggerKey(ConfigMaster.Pause, Pause, !Pause);
            }
        }

        // I don't know how to utilize InputActions because they're mostly readonly and refuse to work.
        // Better simulate keys using WindowsInput.
        private static void TriggerKey(WindowsInput.Native.VirtualKeyCode KeyCode, bool Started, bool Ended)
        {
            InputSimulator InpSim = new InputSimulator();
            if (Started) InpSim.Keyboard.KeyDown(KeyCode);
            else if (Ended) InpSim.Keyboard.KeyUp(KeyCode);
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

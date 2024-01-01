using UnityEngine;
using Valve.VR;
using WindowsInput;
using WindowsInput.Native;
using VRBasePlugin.ULTRAKILL.Config;

namespace VRBasePlugin.ULTRAKILL.Input
{
    internal static class SVRActionsManager
    {
        // simulate keyboard presses (windows only)
        private static InputSimulator InpSim => new InputSimulator();

        public static bool
            Jump = false,
            Dash = false,
            Slide = false;

        public static bool
            RHPrimaryFire = false,
            RHAltFire = false,
            ChangeVariation = false,
            OpenWeaponWheel = false,
            NextWeapon = false,
            PrevWeapon = false;

        public static bool
            Punch = false,
            SwapHand = false,
            Whiplash = false;

        public static bool
            Slot0 = false,
            Slot1 = false, Slot2 = false, Slot3 = false,
            Slot4 = false, Slot5 = false, Slot6 = false,
            Slot7 = false, Slot8 = false, Slot9 = false;

        public static bool Escape = false;

        // Helper methods
        public static string FriendlyBindingName(this ISteamVR_Action_In SVRAI)
        {
            string Direction;
            switch (SVRAI.activeDevice)
            {
                case SteamVR_Input_Sources.LeftHand:
                case SteamVR_Input_Sources.LeftFoot:
                case SteamVR_Input_Sources.LeftShoulder:
                    Direction = "Left"; break;
                case SteamVR_Input_Sources.RightHand:
                case SteamVR_Input_Sources.RightFoot:
                case SteamVR_Input_Sources.RightShoulder:
                    Direction = "Right"; break;
                default: Direction = string.Empty; break;
            }
            if (string.IsNullOrEmpty(Direction))
                return SVRAI.GetLocalizedOriginPart(SteamVR_Input_Sources.Any, EVRInputStringBits.VRInputString_InputSource);
            else return $"{Direction} {SVRAI.GetLocalizedOriginPart(SteamVR_Input_Sources.Any, EVRInputStringBits.VRInputString_InputSource)}";
        }

        // Simulate keyboard input
        private static void TriggerKey(bool Started, bool Ended, VirtualKeyCode? KeyCode = null, MouseButton? Button = null)
        {
            if (KeyCode != null) TriggerKey((VirtualKeyCode)KeyCode, Started, Ended);
            if (Button != null) TriggerKey((MouseButton)Button, Started, Ended);
        }
        private static void TriggerKey(VirtualKeyCode KeyCode, bool Started, bool Ended)
        {
            if (Started) InpSim.Keyboard.KeyDown(KeyCode);
            else if (Ended) InpSim.Keyboard.KeyUp(KeyCode);
        }
        private static void TriggerKey(MouseButton Button, bool Started, bool Ended)
        {
            if (Started)
                switch (Button)
                {
                    case MouseButton.LeftButton:
                        InpSim.Mouse.LeftButtonDown(); break;
                    case MouseButton.RightButton:
                        InpSim.Mouse.RightButtonDown(); break;
                    default:
                        InpSim.Mouse.XButtonDown((int)Button); break;
                }
            else if (Ended)
                switch (Button)
                {
                    case MouseButton.LeftButton:
                        InpSim.Mouse.LeftButtonUp(); break;
                    case MouseButton.RightButton:
                        InpSim.Mouse.RightButtonUp(); break;
                    default:
                        InpSim.Mouse.XButtonUp((int)Button); break;
                }
        }
        private static void MouseScroll(int Amount)
        {
            InpSim.Mouse.VerticalScroll(Amount);
        }

        public static void Init()
        {
            // Movement
            SteamVR_Actions._default.Movement.AddOnUpdateListener(MovementH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Turn.AddOnUpdateListener(TurnH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Jump.AddOnUpdateListener(JumpH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slide.AddOnUpdateListener(SlideH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Dash.AddOnUpdateListener(DashH, SteamVR_Input_Sources.Any);

            // Arms
            SteamVR_Actions._default.Punch.AddOnUpdateListener(PunchH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.SwapHand.AddOnUpdateListener(SwapHandH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Whiplash.AddOnUpdateListener(WhiplashH, SteamVR_Input_Sources.Any);

            // Guns
            SteamVR_Actions._default.Shoot.AddOnUpdateListener(RHShootH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.AltShoot.AddOnUpdateListener(RHAltShootH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.IterateWeapon.AddOnUpdateListener(IterateWeaponH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.ChangeWeaponVariation.AddOnUpdateListener(ChangeWeaponVariationH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.NextWeapon.AddOnUpdateListener(NextWeaponH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.PrevWeapon.AddOnUpdateListener(PrevWeaponH, SteamVR_Input_Sources.Any);

            // Weapon quick switch, open weapon wheel
            SteamVR_Actions._default.OpenWeaponWheel.AddOnUpdateListener(OpenWeaponWheelH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.WeaponWheelScroll.AddOnUpdateListener(WeaponWheelScrollH, SteamVR_Input_Sources.Any);

            // Slots
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

            // Go back, pause, etc.
            SteamVR_Actions._default.Escape.AddOnUpdateListener(EscapeH, SteamVR_Input_Sources.Any);
        }

        // Movemint
        private static void MovementH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        { InputVars.MoveVector = axis; }
        private static void TurnH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        { InputVars.TurnVector = axis; }
        private static void JumpH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Jump) { Jump = newState; TriggerKey(Jump, !Jump, ConfigMaster.KJump, ConfigMaster.MJump); } }
        private static void SlideH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slide) { Slide = newState; TriggerKey(Slide, !Slide, ConfigMaster.KSlide, ConfigMaster.MSlide); } }
        private static void DashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Dash) { Dash = newState; TriggerKey(Dash, !Dash, ConfigMaster.KDash, ConfigMaster.MDash); } }

        // Fisting
        private static void PunchH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != Punch)
            {
                Punch = newState;
                if (Vars.IsPlayerFrozen || Vars.IsPlayerUsingShop) TriggerKey(Punch, !Punch, ConfigMaster.KShoot, ConfigMaster.MShoot);
                else TriggerKey(Punch, !Punch, ConfigMaster.KPunch, ConfigMaster.MPunch);
            }
        }
        private static void SwapHandH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != SwapHand) { SwapHand = newState; TriggerKey(SwapHand, !SwapHand, ConfigMaster.KSwapHand, ConfigMaster.MSwapHand); } }
        private static void WhiplashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Whiplash) { Whiplash = newState; TriggerKey(Whiplash, !Whiplash, ConfigMaster.KWhiplash, ConfigMaster.MWhiplash); } }

        // Shooting
        private static void RHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != RHPrimaryFire) { RHPrimaryFire = newState; TriggerKey(RHPrimaryFire, !RHPrimaryFire, ConfigMaster.KShoot, ConfigMaster.MShoot); } }
        private static void RHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != RHAltFire) { RHAltFire = newState; TriggerKey(RHAltFire, !RHAltFire, ConfigMaster.KAltShoot, ConfigMaster.MAltShoot); } }

        // Weapon scroll
        private static void IterateWeaponH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            if (Vars.IsWeaponWheelPresent) return;
            if (axis.y > 0 + Vars.Config.Controllers.Deadzone * 1.5f) MouseScroll(-1);
            if (axis.y < 0 - Vars.Config.Controllers.Deadzone * 1.5f) MouseScroll(1);
        }
        private static void ChangeWeaponVariationH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != ChangeVariation) { ChangeVariation = newState; TriggerKey(ChangeVariation, !ChangeVariation, ConfigMaster.KChangeVariation, ConfigMaster.MChangeVariation); } }
        private static void NextWeaponH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != NextWeapon) NextWeapon = newState; if (NextWeapon) MouseScroll(1); }
        private static void PrevWeaponH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != PrevWeapon) PrevWeapon = newState; if (PrevWeapon) MouseScroll(-1); }

        // Quick swap & Weapon wheel
        private static void OpenWeaponWheelH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != OpenWeaponWheel) { OpenWeaponWheel = newState; TriggerKey(OpenWeaponWheel, !OpenWeaponWheel, ConfigMaster.KLastWeaponUsed, ConfigMaster.MLastWeaponUsed); } }
        private static void WeaponWheelScrollH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        { InputVars.WWVector = axis; }

        // Slots
        private static void Slot0H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot0) { Slot0 = newState; TriggerKey(Slot0, !Slot0, ConfigMaster.KSlot0, ConfigMaster.MSlot0); } }
        private static void Slot1H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot1) { Slot1 = newState; TriggerKey(Slot1, !Slot1, ConfigMaster.KSlot1, ConfigMaster.MSlot1); } }
        private static void Slot2H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot2) { Slot2 = newState; TriggerKey(Slot2, !Slot2, ConfigMaster.KSlot2, ConfigMaster.MSlot2); } }
        private static void Slot3H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot3) { Slot3 = newState; TriggerKey(Slot3, !Slot3, ConfigMaster.KSlot3, ConfigMaster.MSlot3); } }
        private static void Slot4H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot4) { Slot4 = newState; TriggerKey(Slot4, !Slot4, ConfigMaster.KSlot4, ConfigMaster.MSlot4); } }
        private static void Slot5H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot5) { Slot5 = newState; TriggerKey(Slot5, !Slot5, ConfigMaster.KSlot5, ConfigMaster.MSlot5); } }
        private static void Slot6H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot6) { Slot6 = newState; TriggerKey(Slot6, !Slot6, ConfigMaster.KSlot6, ConfigMaster.MSlot6); } }
        private static void Slot7H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot7) { Slot7 = newState; TriggerKey(Slot7, !Slot7, ConfigMaster.KSlot7, ConfigMaster.MSlot7); } }
        private static void Slot8H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot8) { Slot8 = newState; TriggerKey(Slot8, !Slot8, ConfigMaster.KSlot8, ConfigMaster.MSlot8); } }
        private static void Slot9H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot9) { Slot9 = newState; TriggerKey(Slot9, !Slot9, ConfigMaster.KSlot9, ConfigMaster.MSlot9); } }

        // Go back, pause, etc.
        private static void EscapeH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Escape) { Escape = newState; TriggerKey(Escape, !Escape, ConfigMaster.KEscape, ConfigMaster.MEscape); } }
    }
}
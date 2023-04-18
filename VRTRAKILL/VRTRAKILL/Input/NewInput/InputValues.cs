using Valve.VR;
using UnityEngine;

namespace Plugin.VRTRAKILL.Input.NewInput
{
    internal class InputValues
    {
        // it doesn't matter if it seems reasonable or not, if it's there - it's useful
        public static Vector3 LeftHandPos
        { get { return SteamVR_Actions._default.LHP.GetLocalPosition(SteamVR_Input_Sources.LeftHand); } }
        public static Vector3 RightHandPos
        { get { return SteamVR_Actions._default.RHP.GetLocalPosition(SteamVR_Input_Sources.RightHand); } }

        public static Vector2 MoveAxis
        { get { return SteamVR_Actions._default.Movement.GetAxis(SteamVR_Input_Sources.Any); } }
        public static float MoveXAxis
        { get { return SteamVR_Actions._default.Movement.GetAxis(SteamVR_Input_Sources.Any).x; } }
        public static float MoveYAxis
        { get { return SteamVR_Actions._default.Movement.GetAxis(SteamVR_Input_Sources.Any).y; } }

        public static Vector2 TurnAxis
        { get { return SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any); } }
        public static float TurnXAxis
        { get { return SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any).x; } }
        public static float TurnYAxis
        { get { return SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any).y; } }

        public static bool Jump
        { get { return SteamVR_Actions._default.Jump.GetStateDown(SteamVR_Input_Sources.Any); } }
        public static bool Slide
        { get { return SteamVR_Actions._default.Slide.GetStateDown(SteamVR_Input_Sources.Any); } }
        public static bool Dash
        { get { return SteamVR_Actions._default.Dash.GetStateDown(SteamVR_Input_Sources.Any); } }

        public static bool LHShoot
        { get { return SteamVR_Actions._default.Shoot.GetStateDown(SteamVR_Input_Sources.LeftHand); } }
        public static bool LHAltShoot
        { get { return SteamVR_Actions._default.AltShoot.GetStateDown(SteamVR_Input_Sources.LeftHand); } }
        public static bool RHShoot
        { get { return SteamVR_Actions._default.Shoot.GetStateDown(SteamVR_Input_Sources.RightHand); } }
        public static bool RHAltShoot
        { get { return SteamVR_Actions._default.AltShoot.GetStateDown(SteamVR_Input_Sources.RightHand); } }
        public static Vector2 IterateWeaponAxis
        { get { return SteamVR_Actions._default.IterateWeapon.GetAxis(SteamVR_Input_Sources.Any); } }
        public static bool ChangeWeaponVariation
        { get { return SteamVR_Actions._default.ChangeWeaponVariation.GetStateDown(SteamVR_Input_Sources.Any); } }
        public static bool OpenWeaponWheel
        { get { return SteamVR_Actions._default.OpenWeaponWheel.GetStateDown(SteamVR_Input_Sources.Any); } }

        public static bool SwapHand
        { get { return SteamVR_Actions._default.SwapHand.GetStateDown(SteamVR_Input_Sources.Any); } }
        public static bool Whiplash
        { get { return SteamVR_Actions._default.Whiplash.GetStateDown(SteamVR_Input_Sources.Any); } }

        public static bool Escape
        { get { return SteamVR_Actions._default.Escape.GetStateDown(SteamVR_Input_Sources.Any); } }


    }
}

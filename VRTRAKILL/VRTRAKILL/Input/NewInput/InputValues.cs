using Valve.VR;
using UnityEngine;

namespace Plugin.VRTRAKILL.Input.NewInput
{
    internal class InputValues
    {
        // it doesn't matter if it seems reasonable or not, if it's there - it's useful

        public static Vector2 MoveAxis()
        { return SteamVR_Actions._default.Movement.GetAxis(SteamVR_Input_Sources.Any); }
        public static float MoveXAxis()
        { return SteamVR_Actions._default.Movement.GetAxis(SteamVR_Input_Sources.Any).x; }
        public static float MoveYAxis()
        { return SteamVR_Actions._default.Movement.GetAxis(SteamVR_Input_Sources.Any).y; }

        public static Vector2 TurnAxis()
        { return SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any); }
        public static float TurnXAxis()
        { return SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any).x; }
        public static float TurnYAxis()
        { return SteamVR_Actions._default.Turn.GetAxis(SteamVR_Input_Sources.Any).y; }


    }
}

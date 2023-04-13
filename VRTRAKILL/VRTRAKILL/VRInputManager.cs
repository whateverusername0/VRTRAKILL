using System;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL
{
    internal class VRInputManager
    {
        public static Vector2 MoveVector = Vector2.zero; //public static float Deadzone = 0.45f; // max 0.5
        public static float TurnOffset = 0;

        enum TurnSpeed
        {
            Snail = 50,
            Fast = 100,
            SuperFast = 150,
            SANIC = 200
        }

        public static void Init()
        {
            SteamVR_Actions._default.Movement.AddOnUpdateListener(Movement, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Turn.AddOnUpdateListener(Turn, SteamVR_Input_Sources.Any);
        }

        private static void Movement(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            MoveVector = axis;
            Plugin.PLogger.LogMessage($"Movement Action [{MoveVector.x}, {MoveVector.y}]");
        }
        private static void Turn(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            if (axis.x > 0) TurnOffset += (float)TurnSpeed.SuperFast * Time.deltaTime;
            if (axis.x < 0) TurnOffset -= (float)TurnSpeed.SuperFast * Time.deltaTime;
            Plugin.PLogger.LogMessage($"Turn Action [{TurnOffset}]");
        }
    }
}

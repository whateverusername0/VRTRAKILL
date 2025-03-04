using System;
using UnityEngine;

namespace ULTRAVR.Input
{
    public static class InputManager
    {
        // retranslated joystick axis.
        public static Vector2
            MoveVector = Vector2.zero,
            TurnVector = Vector2.zero,
            WeaponWheelVector = Vector2.zero;

        // used for character rotation.
        public static float TurnOffset = 0;

        public static float GetMovementMultiplier(float refreshRate)
        {
            // it just works. don't question it. there's a lot of absurd numbers here.
            return (float) Math.Round(0.0079861 * (double) refreshRate, 3, MidpointRounding.AwayFromZero);
        }


    }
}

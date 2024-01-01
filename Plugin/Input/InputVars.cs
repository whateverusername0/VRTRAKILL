﻿using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Input
{
    public static class InputVars
    {
        // retranslated joystick axises
        public static Vector2 MoveVector = Vector2.zero,
                              TurnVector = Vector2.zero,
                              WWVector = Vector2.zero;

        // used for character rotation
        public static float TurnOffset = 0;
    }
}
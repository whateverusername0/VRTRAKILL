using UnityEngine;

namespace Plugin.VRTRAKILL.Input
{
    static class VRInputVars
    {
        public static Vector2 MoveVector = Vector2.zero,
                              TurnVector = Vector2.zero, // unused for now
                              WWVector = Vector2.zero;

        public static float TurnOffset = 0;
    }
}
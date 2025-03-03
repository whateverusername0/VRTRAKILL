using System;

namespace ULTRAVR.Input
{
    public class InputManager
    {
        public float GetMovementMultiplier(float refreshRate)
        {
            // it just works. don't question it. there's a lot of absurd numbers here.
            return (float) Math.Round(0.0079861 * (double) refreshRate, 3, MidpointRounding.AwayFromZero);
        }


    }
}

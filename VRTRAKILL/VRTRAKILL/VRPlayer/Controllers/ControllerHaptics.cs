using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class ControllerHaptics : MonoBehaviour
    {
        // uses existing RumbleManager
        float Duration = 0,
              Frequency = 0,
              Amplitude = 0;

        public void Update()
        {
            Pulse(Duration, Frequency, Amplitude, SteamVR_Input_Sources.Any);
        }

        private void Pulse(float Duration, float Frequency, float Amplitude, SteamVR_Input_Sources Source)
        {

        }
    }
}

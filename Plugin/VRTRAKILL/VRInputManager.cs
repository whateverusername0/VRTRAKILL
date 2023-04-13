using System;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL
{
    internal class VRInputManager : MonoBehaviour
    {


        public static Vector2 MoveVector
        {
            get
            {
                return SteamVR_Actions.default_Movement.GetAxis(SteamVR_Input_Sources.Any);
            }
        }

        void Update()
        {

        }
    }
}

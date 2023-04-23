using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    // lol the name
    internal class ControllerController : MonoBehaviour
    {
        private void Start()
        {

        }

        public static void onTransformUpdatedH(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
        {
            fromAction.transform.position = new Vector3(fromAction.transform.position.x,
                                                        fromAction.transform.position.y + 1.4f,
                                                        fromAction.transform.position.z);
        }

        private void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);
        }
    }
}

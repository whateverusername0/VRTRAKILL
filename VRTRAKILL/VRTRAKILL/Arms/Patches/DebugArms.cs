using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.Arms
{
    // note: remove this when time comes
    [HarmonyPatch] internal class DebugArms
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Start")] static void AddPlaceholderHands(NewMovement __instance)
        {
            // Left Hand
            GameObject LHGO = new GameObject(); LHGO.transform.parent = __instance.transform;
            SteamVR_Behaviour_Pose LeftHand = LHGO.AddComponent<SteamVR_Behaviour_Pose>();
            LeftHand.origin = __instance.gameObject.transform;
            LeftHand.poseAction = SteamVR_Actions._default.LHP; LeftHand.inputSource = SteamVR_Input_Sources.LeftHand;
            // Left Hand Model
            GameObject LHMGO = new GameObject(); LHMGO.transform.parent = LHGO.transform;
            LHMGO.AddComponent<SteamVR_RenderModel>(); // it will create model automatically
            // Right Hand
            GameObject RHGO = new GameObject(); RHGO.transform.parent = __instance.transform;
            SteamVR_Behaviour_Pose RightHand = RHGO.AddComponent<SteamVR_Behaviour_Pose>();
            RightHand.origin = __instance.gameObject.transform;
            RightHand.poseAction = SteamVR_Actions._default.RHP; RightHand.inputSource = SteamVR_Input_Sources.RightHand;
            // Right Hand Model
            GameObject RHMGO = new GameObject(); RHMGO.transform.parent = RHGO.transform;
            LHMGO.AddComponent<SteamVR_RenderModel>(); // it will create model automatically
        }
    }
}

using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.Arms
{
    // note: remove this when time comes
    [HarmonyPatch] internal class DebugArms
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(CameraController), "Start")] static void AddPlaceholderHands(NewMovement __instance)
        {
            // Left Hand
            GameObject LHGO = new GameObject("Left Controller"); LHGO.transform.parent = __instance.transform;
            SteamVR_Behaviour_Pose LeftHand = LHGO.AddComponent<SteamVR_Behaviour_Pose>();
            LeftHand.origin = __instance.gameObject.transform;
            LeftHand.poseAction = SteamVR_Actions._default.LHP; LeftHand.inputSource = SteamVR_Input_Sources.LeftHand;
            // Left Hand Model
            GameObject LHMGO = new GameObject("Model"); LHMGO.transform.parent = LHGO.transform;
            SteamVR_RenderModel LHMGORM =  LHMGO.AddComponent<SteamVR_RenderModel>(); // it should create model automatically
            LHMGORM.createComponents = true;
            // Right Hand
            GameObject RHGO = new GameObject("Right Controller"); RHGO.transform.parent = __instance.transform;
            SteamVR_Behaviour_Pose RightHand = RHGO.AddComponent<SteamVR_Behaviour_Pose>();
            RightHand.origin = __instance.gameObject.transform;
            RightHand.poseAction = SteamVR_Actions._default.RHP; RightHand.inputSource = SteamVR_Input_Sources.RightHand;
            // Right Hand Model
            GameObject RHMGO = new GameObject("Model"); RHMGO.transform.parent = RHGO.transform;
            SteamVR_RenderModel RHMGORM = RHMGO.AddComponent<SteamVR_RenderModel>(); // it should create model automatically
            RHMGORM.createComponents = true;
        }
    }
}
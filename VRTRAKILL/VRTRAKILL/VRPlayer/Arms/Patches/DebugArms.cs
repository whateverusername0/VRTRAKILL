using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    // note: remove this when time comes
    [HarmonyPatch] static class DebugArms
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(CameraController), "Start")] static void AddPlaceholderHands(CameraController __instance)
        {
            // Left Hand
            GameObject LHGO = new GameObject("Left Controller"); LHGO.transform.parent = __instance.transform.parent;
            SteamVR_Behaviour_Pose LeftHand = LHGO.AddComponent<SteamVR_Behaviour_Pose>();
            LeftHand.poseAction = SteamVR_Actions._default.LHP; LeftHand.inputSource = SteamVR_Input_Sources.LeftHand;
            // Left Hand Model
            GameObject LHMGO = new GameObject("Model"); LHMGO.transform.parent = LHGO.transform.parent;
            SteamVR_RenderModel LHMGORM =  LHMGO.AddComponent<SteamVR_RenderModel>(); // it should create model automatically
            LHMGORM.createComponents = true;

            // Right Hand
            GameObject RHGO = new GameObject("Right Controller"); RHGO.transform.parent = __instance.transform.parent;
            SteamVR_Behaviour_Pose RightHand = RHGO.AddComponent<SteamVR_Behaviour_Pose>();
            RightHand.poseAction = SteamVR_Actions._default.RHP; RightHand.inputSource = SteamVR_Input_Sources.RightHand;
            // Right Hand Model
            GameObject RHMGO = new GameObject("Model"); RHMGO.transform.parent = RHGO.transform.parent;
            SteamVR_RenderModel RHMGORM = RHMGO.AddComponent<SteamVR_RenderModel>(); // it should create model automatically
            RHMGORM.createComponents = true;
        }
    }
}
using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch(typeof(NewMovement))] internal class DebugArms
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Start))] public static void AddDebugHands(NewMovement __instance)
        {
            __instance.gameObject.SetActive(false);

            // Left Hand
            GameObject LHGO = new GameObject("Left Controller"); LHGO.transform.parent = Vars.VRCameraContainer.transform;

            SteamVR_Behaviour_Pose LeftHand = LHGO.AddComponent<SteamVR_Behaviour_Pose>();
            LeftHand.poseAction = SteamVR_Actions._default.LeftPose;
            LeftHand.inputSource = SteamVR_Input_Sources.LeftHand;

            // Left Hand Model
            GameObject LHMGO = new GameObject("Model"); LHMGO.transform.parent = LHGO.transform;
            LHMGO.AddComponent<LeftArmController>();

            SteamVR_RenderModel LHMGORM = LHMGO.AddComponent<SteamVR_RenderModel>(); // it should create model automatically
            LHMGORM.createComponents = true;

            // Right Hand
            GameObject RHGO = new GameObject("Right Controller"); RHGO.transform.parent = Vars.VRCameraContainer.transform;

            SteamVR_Behaviour_Pose RightHand = RHGO.AddComponent<SteamVR_Behaviour_Pose>();
            RightHand.poseAction = SteamVR_Actions._default.RightPose;
            RightHand.inputSource = SteamVR_Input_Sources.RightHand;

            // Right Hand Model
            GameObject RHMGO = new GameObject("Model"); RHMGO.transform.parent = RHGO.transform;
            RHMGO.AddComponent<RightArmController>();

            SteamVR_RenderModel RHMGORM = RHMGO.AddComponent<SteamVR_RenderModel>(); // it should create model automatically
            RHMGORM.createComponents = true;

            __instance.gameObject.SetActive(true);
        }
    }
}

using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers.Patches
{
    [HarmonyPatch(typeof(NewMovement))] internal class ControllerAdder
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Start))] public static void AddHands(NewMovement __instance)
        {
            __instance.gameObject.SetActive(false);

            // Left Hand
            GameObject LHGO = new GameObject("Left Controller") { layer = (int)Layers.IgnoreRaycast };
            LHGO.transform.parent = Vars.VRCameraContainer.transform;
            LHGO.AddComponent<ControllerController>();

            SteamVR_Behaviour_Pose LeftHand = LHGO.AddComponent<SteamVR_Behaviour_Pose>();
            LeftHand.onTransformUpdatedEvent += ControllerController.onTransformUpdatedH;
            LeftHand.poseAction = SteamVR_Actions._default.LeftPose;
            LeftHand.inputSource = SteamVR_Input_Sources.LeftHand;

            // Right Hand
            GameObject RHGO = new GameObject("Right Controller") { layer = (int)Layers.IgnoreRaycast };
            RHGO.transform.parent = Vars.VRCameraContainer.transform;
            RHGO.AddComponent<ControllerController>();

            SteamVR_Behaviour_Pose RightHand = RHGO.AddComponent<SteamVR_Behaviour_Pose>();
            RightHand.onTransformUpdatedEvent += ControllerController.onTransformUpdatedH;
            RightHand.poseAction = SteamVR_Actions._default.RightPose;
            RightHand.inputSource = SteamVR_Input_Sources.RightHand;

            if (Vars.Config.Controllers.DrawControllers)
            {
                // Left Hand Model
                GameObject LHMGO = new GameObject("Model"); LHMGO.transform.parent = LHGO.transform;
                SteamVR_RenderModel LHMGORM = LHMGO.AddComponent<SteamVR_RenderModel>();
                LHMGORM.createComponents = true;

                // Right Hand Model
                GameObject RHMGO = new GameObject("Model"); RHMGO.transform.parent = RHGO.transform;
                SteamVR_RenderModel RHMGORM = RHMGO.AddComponent<SteamVR_RenderModel>();
                RHMGORM.createComponents = true;
            }

            if (Vars.Config.Controllers.LeftHanded)
            {
                RHGO.AddComponent<ArmController>();
                LHGO.AddComponent<GunController>();
            }
            else
            {
                LHGO.AddComponent<ArmController>();
                RHGO.AddComponent<GunController>();
            }

            __instance.gameObject.SetActive(true);
        }
    }
}

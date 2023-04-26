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
            GameObject LHGO = new GameObject("Left Controller");
            LHGO.transform.parent = Vars.VRCameraContainer.transform;

            LHGO.AddComponent<ControllerController>();
            LHGO.AddComponent<LeftArmController>();

            SteamVR_Behaviour_Pose LeftHand = LHGO.AddComponent<SteamVR_Behaviour_Pose>();
            LeftHand.onTransformUpdatedEvent += ControllerController.onTransformUpdatedH;
            LeftHand.poseAction = SteamVR_Actions._default.LeftPose;
            LeftHand.inputSource = SteamVR_Input_Sources.LeftHand;

            // Right Hand
            GameObject RHGO = new GameObject("Right Controller");
            RHGO.transform.parent = Vars.VRCameraContainer.transform;

            RHGO.AddComponent<ControllerController>();
            RHGO.AddComponent<RightArmController>();

            SteamVR_Behaviour_Pose RightHand = RHGO.AddComponent<SteamVR_Behaviour_Pose>();
            RightHand.onTransformUpdatedEvent += ControllerController.onTransformUpdatedH;
            RightHand.poseAction = SteamVR_Actions._default.RightPose;
            RightHand.inputSource = SteamVR_Input_Sources.RightHand;

            if (!Vars.IsAMenu) try // remove ! 
                {
                    // Left Hand Model
                    GameObject LHMGO = new GameObject("Model"); LHMGO.transform.parent = LHGO.transform;
                    SteamVR_RenderModel LHMGORM = LHMGO.AddComponent<SteamVR_RenderModel>();
                    LHMGORM.createComponents = true;

                    // Right Hand Model
                    GameObject RHMGO = new GameObject("Model"); RHMGO.transform.parent = RHGO.transform;
                    SteamVR_RenderModel RHMGORM = RHMGO.AddComponent<SteamVR_RenderModel>();
                    RHMGORM.createComponents = true;
                } catch {}
            else try
                {
                    for (int i = 0; i < Vars.VRCameraContainer.transform.childCount; i++)
                    {
                        // painful to look at
                        if (Helpers.Misc.HasComponent<SteamVR_RenderModel>(Vars.VRCameraContainer.transform.GetChild(i).transform.GetChild(0).gameObject))
                            GameObject.Destroy(Vars.VRCameraContainer.gameObject.GetComponent<SteamVR_RenderModel>());
                    } 
                } catch {}

            __instance.gameObject.SetActive(true);
        }
    }
}

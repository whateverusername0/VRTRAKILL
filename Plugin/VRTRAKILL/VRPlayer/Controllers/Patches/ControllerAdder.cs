using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers.Patches
{
    [HarmonyPatch(typeof(NewMovement))] internal sealed class ControllerAdder
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Start))] public static void AddHands(NewMovement __instance)
        {
            __instance.gameObject.SetActive(false);

            GameObject LHGO = CreateController("Left Controller", SteamVR_Input_Sources.LeftHand);
            GameObject RHGO = CreateController("Right Controller", SteamVR_Input_Sources.RightHand);

            if (Vars.Config.Controllers.DrawControllers)
            {
                GameObject LHMGO = CreateControllerModel(); LHMGO.transform.parent = LHGO.transform;
                GameObject RHMGO = CreateControllerModel(); RHMGO.transform.parent = RHGO.transform;
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

        private static GameObject CreateController(string Name, SteamVR_Input_Sources Source)
        {
            GameObject GO = new GameObject(Name) { layer = (int)Layers.IgnoreRaycast };
            GO.AddComponent<ControllerController>();
            SteamVR_Behaviour_Pose Controller = GO.AddComponent<SteamVR_Behaviour_Pose>();
            Controller.onTransformUpdatedEvent += ControllerController.onTransformUpdatedH;
            if (Source == SteamVR_Input_Sources.LeftHand)
            {
                Controller.poseAction = SteamVR_Actions._default.LeftPose;
                Controller.inputSource = SteamVR_Input_Sources.LeftHand;
            }
            else if (Source == SteamVR_Input_Sources.RightHand)
            {
                Controller.poseAction = SteamVR_Actions._default.RightPose;
                Controller.inputSource = SteamVR_Input_Sources.RightHand;
            }
            else throw new System.NotImplementedException();
            return GO;
        }
        private static GameObject CreateControllerModel(string Name = "Model")
        {
            GameObject GO = new GameObject(Name) { layer = (int)Layers.IgnoreRaycast };

            return GO;
        }
    }
}

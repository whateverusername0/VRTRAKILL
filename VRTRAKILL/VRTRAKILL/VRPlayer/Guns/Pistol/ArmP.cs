using HarmonyLib;
using UnityEngine;
using Plugin.Helpers.InverseKinematics;
using Joint = Plugin.Helpers.InverseKinematics.Joint;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Pistol
{
    // introducing inverse kinematics (real)
    /* [HarmonyPatch(typeof(Revolver))] */ internal class ArmP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(Revolver.Start))] static void AddIK(Revolver __instance)
        {
            GameObject IKMGO = new GameObject("IK Manager");
            IKMGO.transform.parent = __instance.transform;

            Arms.Feedbacker.Armature Armature = new Arms.Feedbacker.Armature(__instance.gameObject.transform.Find("Revolver_Rerigged_Standard"));
            Joint JArmature = Armature.RArmature.gameObject.AddComponent<Joint>();
            Joint JUpperArm = Armature.UpperArm.gameObject.AddComponent<Joint>(); JArmature.Child = JUpperArm;
            Joint JForearm = Armature.Forearm.gameObject.AddComponent<Joint>(); JUpperArm.Child = JForearm;
            Joint JHand = Armature.Hand.gameObject.AddComponent<Joint>(); JForearm.Child = JHand;

            IKManager IKM = IKMGO.AddComponent<IKManager>();
            IKM.Root = JArmature; IKM.End = JHand; 
        }
    }
}

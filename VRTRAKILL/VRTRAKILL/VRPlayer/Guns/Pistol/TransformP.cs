using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Pistol
{
    [HarmonyPatch(typeof(Revolver))] internal class TransformP
    {
        // this takes a lot of trial and error
        static Vector3 Position    = new Vector3(0, -0.8f, 0.1f),
                       AltPosition = new Vector3(0, -0.075f, 0.1f);
        static Vector3 Scale = new Vector3(0.1f, 0.1f, 0.1f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Revolver.Start))] static void Retransform(Revolver __instance)
        {
            if (__instance.altVersion == true) __instance.wpos.defaultPos = AltPosition; 
            else __instance.wpos.defaultPos = Position;
            __instance.wpos.defaultScale = Scale;
        }
        // for some reason regular revolvers are invisible when changed to until you rotate it upwards
        [HarmonyPostfix] [HarmonyPatch(nameof(Revolver.OnEnable))] static void What(Revolver __instance)
        {
            Quaternion LastRotation = __instance.transform.rotation;
            __instance.transform.rotation = Quaternion.Euler(0, -180, 0);
            __instance.transform.rotation = LastRotation;
        }
    }
}

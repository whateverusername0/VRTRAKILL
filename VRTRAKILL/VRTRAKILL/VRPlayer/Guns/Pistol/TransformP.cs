using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Pistol
{
    [HarmonyPatch(typeof(Revolver))] internal class TransformP
    {
        // this takes a lot of trial and error
        static Vector3 Position = new Vector3(0, -0.075f, 0.1f);
        //static Vector3 Rotation = new Vector3(0, 90, 45);
        static Vector3 Scale = new Vector3(0.1f, 0.1f, 0.1f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Revolver.Start))] static void Retransform(Revolver __instance)
        {
            __instance.wpos.defaultPos = Position;
            //__instance.wpos.defaultRot = Rotation;
            __instance.wpos.defaultScale = Scale;
        }
    }
}

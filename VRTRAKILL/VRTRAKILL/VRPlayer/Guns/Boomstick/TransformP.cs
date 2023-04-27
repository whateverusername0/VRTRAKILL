using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Boomstick
{
    [HarmonyPatch(typeof(Shotgun))] internal class TransformP
    {
        static Vector3 Position = new Vector3(0, -0.075f, 0.1f);
        static Vector3 Scale = new Vector3(0.15f, 0.15f, 0.15f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))] static void Retransform(Shotgun __instance)
        {
            __instance.wpos.defaultPos = Position;
            __instance.wpos.defaultScale = Scale;
        }
    }
}

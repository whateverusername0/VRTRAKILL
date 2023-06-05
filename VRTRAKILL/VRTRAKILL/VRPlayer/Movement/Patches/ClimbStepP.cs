using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Movement.Patches
{
    // ClimbStep fix (make you climb stairs like a real machine)
    [HarmonyPatch(typeof(ClimbStep))] internal static class ClimbStepP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(ClimbStep.FixedUpdate))]
        static bool FixedUpdate(ClimbStep __instance)
        {
            if (__instance.cooldown <= 0f) __instance.cooldown = 0f;
            else __instance.cooldown -= Time.deltaTime;

            Vector2 vector = Input.VRInputVars.MoveVector;
            __instance.movementDirection = Vector3.ClampMagnitude(vector.x * __instance.transform.right + vector.y * __instance.transform.forward, 1f);

            return false;
        }
    }
}

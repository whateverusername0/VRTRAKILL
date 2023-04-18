using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Movement.Patches
{
    // idk what climbstep is, probably the big step thing, meaning this is useful
    [HarmonyPatch(typeof(ClimbStep))] internal class ClimbStepP
    {
        [HarmonyPrefix] [HarmonyPatch("FixedUpdate")]
        static bool FixedUpdate(ClimbStep __instance, ref float ___cooldown, ref Vector3 ___movementDirection)
        {
            if (___cooldown <= 0f) ___cooldown = 0f;
            else ___cooldown -= Time.deltaTime;

            Vector2 vector = Input.VRInputVars.MoveVector;
            ___movementDirection = Vector3.ClampMagnitude(vector.x * __instance.transform.right + vector.y * __instance.transform.forward, 1f);

            return false;
        }
    }
}

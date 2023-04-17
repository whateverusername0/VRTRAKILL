using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.Movement.Patches
{
    // hopefully fixes dashing once and for all
    [HarmonyPatch] internal class Dash
    {
        // i never learn to use a fucking transpiler, oh well
        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Dodge")] static bool FixDash
        (NewMovement __instance, ref bool ___hurting, ref float ___boostLeft, ref float ___preSlideSpeed, ref float ___preSlideDelay,
         ref Vector3 ___movementDirection, ref Vector3 ___movementDirection2, ref bool ___slideEnding)
        {
            if (__instance.sliding)
            {
                if (!___hurting && ___boostLeft <= 0f)
                {
                    __instance.gameObject.layer = 2;
                    __instance.exploded = false;
                }

                float num = 1f;
                if (___preSlideSpeed > 1f)
                {
                    if (___preSlideSpeed > 3f) ___preSlideSpeed = 3f;

                    num = ___preSlideSpeed;
                    if (__instance.gc.onGround)
                        ___preSlideSpeed -= Time.fixedDeltaTime * ___preSlideSpeed;

                    ___preSlideDelay = 0f;
                }

                if (__instance.modNoDashSlide)
                {
                    __instance.StopSlide();
                    return false;
                }

                if ((bool)__instance.groundProperties)
                {
                    if (!__instance.groundProperties.canSlide)
                    {
                        __instance.StopSlide();
                        return false;
                    }
                    num *= __instance.groundProperties.speedMultiplier;
                }

                Vector3 vector = new Vector3(__instance.dodgeDirection.x * __instance.walkSpeed * Time.deltaTime * 4f * num,
                                             __instance.rb.velocity.y,
                                             __instance.dodgeDirection.z * __instance.walkSpeed * Time.deltaTime * 4f * num);
                if ((bool)__instance.groundProperties && __instance.groundProperties.push)
                {
                    Vector3 vector2 = __instance.groundProperties.pushForce;
                    if (__instance.groundProperties.pushDirectionRelative)
                        vector2 = __instance.groundProperties.transform.rotation * vector2;

                    vector += vector2;
                }

                ___movementDirection = Vector3.ClampMagnitude(Input.VRInputVars.MoveVector.x * __instance.transform.right, 1f) * 5f;
                if (!MonoSingleton<HookArm>.Instance || !MonoSingleton<HookArm>.Instance.beingPulled)
                    __instance.rb.velocity = vector + __instance.pushForce + ___movementDirection;
                else __instance.StopSlide();

                return false;
            }

            float y = 0f;
            if (___slideEnding) y = __instance.rb.velocity.y;

            float num2 = 2.75f;
            ___movementDirection2 = new Vector3(__instance.dodgeDirection.x * __instance.walkSpeed * Time.deltaTime * num2,
                                                        y, __instance.dodgeDirection.z * __instance.walkSpeed * Time.deltaTime * num2);
            if (!___slideEnding || (__instance.gc.onGround && !__instance.jumping))
                __instance.rb.velocity = ___movementDirection2 * 3f;

            __instance.gameObject.layer = 15;
            ___boostLeft -= 4f;
            if (___boostLeft <= 0f)
            {
                __instance.boost = false;
                if (!__instance.gc.onGround && !___slideEnding)
                    __instance.rb.velocity = ___movementDirection2;
            }

            ___slideEnding = false;

            return false;
        }


    }
}

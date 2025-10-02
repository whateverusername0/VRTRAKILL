using HarmonyLib;
using Plugin.Data;
using UnityEngine;

namespace Plugin.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(Chainsaw))] internal class PatchChainsaw
{
    [HarmonyPrefix] [HarmonyPatch(nameof(Chainsaw.Update))]
    private static bool Update(Chainsaw __instance)
    {
        __instance.lr.SetPosition(0, Vars.DominantHand.transform.position);
        __instance.lr.SetPosition(1, __instance.transform.position);
        if ((bool)__instance.rb)
        {
            if (__instance.inPlayer)
            {
                __instance.transform.forward = Vars.DominantHand.transform.forward * -1f;
            }
            else
            {
                __instance.transform.LookAt(__instance.transform.position + (__instance.transform.position - __instance.attachedTransform.position));
            }
        }

        if (__instance.sameEnemyHitCooldown > 0f && !__instance.stopped)
        {
            __instance.sameEnemyHitCooldown = Mathf.MoveTowards(__instance.sameEnemyHitCooldown, 0f, Time.deltaTime);
            if (__instance.sameEnemyHitCooldown <= 0f)
            {
                __instance.currentHitEnemy = null;
            }
        }

        if (__instance.inPlayer)
        {
            __instance.transform.position = __instance.attachedTransform.position;
            __instance.playerHitTimer = Mathf.MoveTowards(__instance.playerHitTimer, 0.25f, Time.deltaTime);
            __instance.stoppedAud.pitch = 0.5f;
            __instance.stoppedAud.volume = 1f;
            if (__instance.playerHitTimer >= 0.25f && !__instance.beingPunched)
            {
                Object.Destroy(__instance.gameObject);
            }
        }
        else
        {
            if (__instance.hitAmount <= 1)
            {
                return false;
            }

            if (__instance.multiHitCooldown > 0f)
            {
                __instance.multiHitCooldown = Mathf.MoveTowards(__instance.multiHitCooldown, 0f, Time.deltaTime);
            }
            else if (__instance.stopped)
            {
                if (!__instance.currentHitEnemy.dead && __instance.currentHitAmount > 0)
                {
                    __instance.currentHitAmount--;
                    __instance.DamageEnemy(__instance.hitTarget, __instance.currentHitEnemy);
                }

                if (__instance.currentHitEnemy.dead || __instance.currentHitAmount <= 0)
                {
                    __instance.stopped = false;
                    __instance.rb.velocity = __instance.originalVelocity.normalized * Mathf.Max(__instance.originalVelocity.magnitude, 35f);
                    return false;
                }

                __instance.multiHitCooldown = 0.05f;
            }

            if ((bool)__instance.stoppedAud)
            {
                if (__instance.stopped)
                {
                    __instance.stoppedAud.pitch = 1.1f;
                    __instance.stoppedAud.volume = 1f;
                }
                else
                {
                    __instance.stoppedAud.pitch = 0.85f;
                    __instance.stoppedAud.volume = 0.66f;
                }
            }
        }

        return false;
    }
}

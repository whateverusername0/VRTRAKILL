using HarmonyLib;
using Plugin.Data;
using UnityEngine;

namespace Plugin.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(Vacuum))]
internal class PatchVacuum
{
    [HarmonyPrefix] [HarmonyPatch(nameof(Vacuum.SuckObjects))]
    private static bool SuckObjects(Vacuum __instance)
    {
        if ((!__instance._isSucking && !__instance._isBlowing) || __instance._stuckObject.rigidbody != null)
        {
            return false;
        }

        __instance.UpdateColliders();
        int i = __instance._colliders.Offset;
        for (int num = i + __instance._colliders.Count; i < num; i++)
        {
            Collider collider = __instance._colliders.Array[i];
            if (collider == null || collider.attachedRigidbody == null || collider.attachedRigidbody.TryGetComponent<NewMovement>(out var _))
            {
                continue;
            }

            Rigidbody attachedRigidbody = collider.attachedRigidbody;
            if (__instance._isSucking && attachedRigidbody.TryGetComponent<Cork>(out var component2))
            {
                component2.StartWiggle();
            }

            if (__instance._isSucking && collider.TryGetComponent<TerribleTasteBook>(out var component3))
            {
                component3.ActivateBookShelf();
            }

            GhostDrone component4 = null;
            if (__instance._isSucking && attachedRigidbody.TryGetComponent<GhostDrone>(out component4))
            {
                Vector3 vacuumVelocity = Vector3.Normalize(__instance._suckPoint.position - attachedRigidbody.position) * __instance._suckStrength;
                component4.vacuumVelocity = vacuumVelocity;
            }

            if (__instance._isSucking)
            {
                attachedRigidbody.velocity = Vector3.Normalize(__instance._suckPoint.position - attachedRigidbody.worldCenterOfMass) * __instance._suckStrength;
            }
            else
            {
                attachedRigidbody.velocity = Vars.DominantHand.transform.forward.normalized * __instance._suckStrength * 2f;
            }

            if (__instance._isBlowing || Vector3.Distance(attachedRigidbody.worldCenterOfMass, __instance._suckPoint.position) >= __instance._stuckDistance)
            {
                continue;
            }

            if (!attachedRigidbody.TryGetComponent<GoreSplatter>(out var component5))
            {
                if ((bool)component4)
                {
                    component4.KillGhost();
                    continue;
                }

                __instance.SetStuckObject(attachedRigidbody);
                break;
            }

            if (!__instance.musicStarted)
            {
                if ((bool)__instance.music)
                {
                    __instance.music.SetActive(value: true);
                }

                __instance.musicStarted = true;
            }

            __instance._consumeSound.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            __instance._consumeSound.PlayOneShot(__instance._consumeSound.clip);
            component5.Repool();
        }

        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Vacuum.UpdateStuckObject))]
    private static bool UpdateStuckObject(Vacuum __instance)
    {
        if (!(__instance._stuckObject.rigidbody == null))
        {
            Vector3 vector = __instance._suckPoint.position - __instance._stuckObject.rigidbody.worldCenterOfMass;
            __instance._stuckObject.rigidbody.velocity = vector / Time.fixedDeltaTime;
            float num = Vars.DominantHand.transform.eulerAngles.y - __instance._lastCameraRotation.y;
            num *= Mathf.PI / 180f;
            __instance._stuckObject.rigidbody.angularVelocity = new Vector3(0f, num / Time.fixedDeltaTime, 0f);
            __instance._lastCameraRotation = Vars.DominantHand.transform.eulerAngles;
        }
        return false;
    }
}

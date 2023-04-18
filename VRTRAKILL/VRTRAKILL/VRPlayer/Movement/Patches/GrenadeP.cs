using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Movement.Patches
{
    // Rocket riding patch - change movement vector to vr one
    [HarmonyPatch(typeof(Grenade))] internal class GrenadeP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(Grenade.LateUpdate))] static bool FixRocketRiding(Grenade __instance)
        {
            if (__instance.playerRiding)
            {
                if (Vector3.Distance(__instance.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) > 5f + __instance.rb.velocity.magnitude * Time.deltaTime)
                {
                    __instance.PlayerRideEnd();
                    return false;
                }
                Vector2 vector = Input.VRInputVars.MoveVector;
                __instance.transform.Rotate(vector.y * Time.deltaTime * 165f, vector.x * Time.deltaTime * 165f, 0f, Space.Self);
                if (Physics.Raycast(__instance.transform.position + __instance.transform.forward, __instance.transform.up, 4f, LayerMaskDefaults.Get(LMD.Environment)))
                {
                    RaycastHit raycastHit;
                    if (Physics.Raycast(__instance.transform.position + __instance.transform.forward, Vector3.up, out raycastHit, 2f, LayerMaskDefaults.Get(LMD.Environment)))
                        MonoSingleton<NewMovement>.Instance.transform.position = __instance.transform.position + __instance.transform.forward - Vector3.up * raycastHit.distance;
                    else
                        MonoSingleton<NewMovement>.Instance.transform.position = __instance.transform.position + __instance.transform.forward;
                }
                else
                    MonoSingleton<NewMovement>.Instance.transform.position = __instance.transform.position + __instance.transform.up * 2f + __instance.transform.forward;

                MonoSingleton<CameraController>.Instance.CameraShake(0.1f);
            }

            return false;
        }
    }
}

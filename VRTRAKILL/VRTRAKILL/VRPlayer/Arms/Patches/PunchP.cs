using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch(typeof(Punch))] internal static class PunchP
    {
        /* [HarmonyPrefix] [HarmonyPatch(nameof(Punch.Update))] */ static bool Update(Punch __instance)
        {
            if (MonoSingleton<OptionsManager>.Instance.paused) return false;

            if (Vars.LCC.Speed >= 1 // detect fist speed instead of a button press
                && __instance.ready && !__instance.shopping
                && __instance.fc.fistCooldown <= 0f && __instance.fc.activated
                && !GameStateManager.Instance.PlayerInputLocked)
            {
                // no more scalable cooldown
                __instance.fc.weightCooldown += __instance.cooldownCost * 0.25f /* + __instance.fc.weightCooldown */ * __instance.cooldownCost * 0.1f;
                __instance.fc.fistCooldown += __instance.fc.weightCooldown;
                __instance.PunchStart();
                __instance.holdingInput = true;
            }

            if (__instance.holdingInput && MonoSingleton<InputManager>.Instance.InputSource.Punch.WasCanceledThisFrame)
                __instance.holdingInput = false;

            float layerWeight = __instance.anim.GetLayerWeight(1);
            if (__instance.shopping && layerWeight < 1f)
                __instance.anim.SetLayerWeight(1, Mathf.MoveTowards(layerWeight, 1f, Time.deltaTime / 10f + 5f * Time.deltaTime * (1f - layerWeight)));
            else if (!__instance.shopping && layerWeight > 0f)
                __instance.anim.SetLayerWeight(1, Mathf.MoveTowards(layerWeight, 0f, Time.deltaTime / 10f + 5f * Time.deltaTime * layerWeight));

            if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo()
                && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && __instance.shopping)
                __instance.anim.SetTrigger("ShopTap");

            if (__instance.returnToOrigRot)
            {
                __instance.transform.parent.localRotation =
                    Quaternion.RotateTowards(__instance.transform.parent.localRotation,
                                             Quaternion.identity,
                                             (Quaternion.Angle(__instance.transform.parent.localRotation,
                                                               Quaternion.identity) * 5f + 5f)
                                                               * Time.deltaTime * 5f);
                if (__instance.transform.parent.localRotation == Quaternion.identity)
                    __instance.returnToOrigRot = false;
            }

            if (__instance.fc.shopping && !__instance.shopping) __instance.ShopMode();
            else if (!__instance.fc.shopping && __instance.shopping) __instance.StopShop();

            if (__instance.holding && (bool)__instance.heldItem)
            {
                if (!__instance.heldItem.noHoldingAnimation && __instance.fc.forceNoHold <= 0)
                {
                    __instance.anim.SetBool("SemiHolding", value: false);
                    __instance.anim.SetBool("Holding", value: true);
                }
                else __instance.anim.SetBool("SemiHolding", value: true);
            }
            return false;
        }

        /* [HarmonyPrefix] [HarmonyPatch(nameof(Punch.PunchStart))] */ static bool PunchStart(Punch __instance)
        {
            if (__instance.ready)
            {
                __instance.ready = false;
                __instance.anim.SetFloat("PunchRandomizer", Random.Range(0f, 1f));
                __instance.anim.SetTrigger("Punch");
                __instance.aud.pitch = Random.Range(0.9f, 1.1f);
                __instance.aud.Play();
                __instance.tr.widthMultiplier = 0.5f;
                MonoSingleton<HookArm>.Instance.Cancel();
                if (__instance.holding && (bool)__instance.heldItem)
                    __instance.heldItem.SendMessage("PunchWith", SendMessageOptions.DontRequireReceiver);

                MonoSingleton<RumbleManager>.Instance.SetVibration("rumble.punch");
            }
            return false;
        }
    }
}

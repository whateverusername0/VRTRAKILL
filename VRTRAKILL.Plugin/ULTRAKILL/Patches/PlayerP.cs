using HarmonyLib;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Patches
{
    
    [HarmonyPatch(typeof(NewMovement))] internal class PlayerP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Respawn))] static bool RespawnFix(NewMovement __instance)
        {
            if (__instance.sliding) __instance.StopSlide();

            __instance.sameCheckpointRestarts++;

            if (__instance.difficulty == 0) __instance.hp = 200;
            else __instance.hp = 100;

            __instance.boostCharge = 299f;

            __instance.antiHp = 0f;
            __instance.antiHpCooldown = 0f;

            __instance.rb.constraints = __instance.defaultRBConstraints;
            __instance.rb.position += new Vector3(0, 0.25f, 0);

            __instance.activated = true;

            __instance.blackScreen.gameObject.SetActive(false);

            //__instance.cc.enabled = true;

            if (MonoSingleton<PowerUpMeter>.Instance) MonoSingleton<PowerUpMeter>.Instance.juice = 0f;

            StatsManager instance = MonoSingleton<StatsManager>.Instance;

            instance.stylePoints = instance.stylePoints / 3 * 2;

            if (__instance.gunc == null) __instance.gunc = __instance.GetComponentInChildren<GunControl>();
            __instance.gunc.YesWeapon();

            __instance.screenHud.SetActive(true);

            __instance.dead = false;

            __instance.blackColor.a = 0f;
            __instance.youDiedColor.a = 0f;

            __instance.currentAllPitch = 1f;

            __instance.blackScreen.color = __instance.blackColor;
            __instance.youDiedText.color = __instance.youDiedColor;

            MonoSingleton<TimeController>.Instance.controlPitch = true;
            HookArm instance2 = MonoSingleton<HookArm>.Instance;

            if (instance2 != null) instance2.Cancel();
            if (__instance.punch == null) __instance.punch = __instance.GetComponentInChildren<FistControl>();

            __instance.punch.activated = true;
            __instance.punch.YesFist();

            __instance.slowMode = false;

            MonoSingleton<WeaponCharges>.Instance.MaxCharges();
            if (MonoSingleton<WeaponCharges>.Instance.rocketFrozen) MonoSingleton<WeaponCharges>.Instance.rocketLauncher.UnfreezeRockets();

            return false;
        }
    }
}

using HarmonyLib;
using VRTRAKILL.Data;
using UnityEngine;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(Nailgun))] internal class PatchNailgun
{
    [HarmonyPrefix] [HarmonyPatch(nameof(Nailgun.Shoot))]
    private static bool Shoot(Nailgun __instance)
    {
        __instance.UpdateAnimationWeight();
        __instance.fireCooldown = __instance.currentFireRate;
        __instance.shotSuccesfully = true;
        if (__instance.variation == 1 && (!__instance.wid || __instance.wid.delay == 0f))
        {
            if (__instance.altVersion) __instance.wc.naiSaws -= 1f;
            else __instance.wc.naiAmmo -= 1f;
        }

        __instance.anim.SetTrigger("Shoot");
        __instance.barrelNum++;
        if (__instance.barrelNum >= __instance.shootPoints.Length)
            __instance.barrelNum = 0;

        GameObject gameObject = ((!__instance.burnOut)
        ? Object.Instantiate(__instance.muzzleFlash, __instance.shootPoints[__instance.barrelNum].transform)
        : Object.Instantiate(__instance.muzzleFlash2, __instance.shootPoints[__instance.barrelNum].transform));

        if (!__instance.altVersion)
        {
            AudioSource component = gameObject.GetComponent<AudioSource>();
            if (__instance.burnOut)
            {
                component.volume = 0.65f - __instance.wid.delay * 2f;
                if (component.volume < 0f) component.volume = 0f;
                component.pitch = 2f;
                __instance.currentSpread = __instance.spread * 2f;
            }
            else
            {
                if (__instance.heatSinks < 1f)
                {
                    component.pitch = 0.75f;
                    component.volume = 0.25f - __instance.wid.delay * 2f;
                    if (component.volume < 0f) component.volume = 0f;
                }
                else
                {
                    component.volume = 0.65f - __instance.wid.delay * 2f;
                    if (component.volume < 0f) component.volume = 0f;
                }

                __instance.currentSpread = __instance.spread;
            }
        }
        else if (__instance.burnOut)
        {
            __instance.currentSpread = 45f;
        }
        else if (__instance.altVersion && __instance.variation == 0)
        {
            if (__instance.heatSinks < 1f)
            {
                __instance.currentSpread = 45f;
            }
            else
            {
                __instance.currentSpread = Mathf.Lerp(0f, 45f, Mathf.Max(0f, __instance.heatUp - 0.25f));
            }
        }
        else __instance.currentSpread = 0f;

        GameObject gameObject2 = ((!__instance.burnOut)
        ? Object.Instantiate(__instance.nail, Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward, __instance.transform.rotation)
        : Object.Instantiate(__instance.heatedNail, Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward, __instance.transform.rotation));
        if (__instance.altVersion && __instance.variation == 0 && __instance.heatSinks >= 1f)
            __instance.heatUp = Mathf.MoveTowards(__instance.heatUp, 1f, 0.125f);

        gameObject2.transform.forward = Vars.DominantHand.transform.forward;
        if (Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.forward, 1f, LayerMaskDefaults.Get(LMD.Environment)))
            gameObject2.transform.position = Vars.DominantHand.transform.position;

        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
        {
            gameObject2.transform.position = Vars.DominantHand.transform.position + (__instance.targeter.CurrentTarget.bounds.center - Vars.DominantHand.transform.position).normalized;
            gameObject2.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
        }

        gameObject2.transform.Rotate(Random.Range((0f - __instance.currentSpread) / 3f, __instance.currentSpread / 3f), Random.Range((0f - __instance.currentSpread) / 3f, __instance.currentSpread / 3f), Random.Range((0f - __instance.currentSpread) / 3f, __instance.currentSpread / 3f));
        if (gameObject2.TryGetComponent<Rigidbody>(out var component2))
            component2.velocity = gameObject2.transform.forward * 200f;

        if (gameObject2.TryGetComponent<Nail>(out var component3))
        {
            component3.sourceWeapon = __instance.gc.currentWeapon;
            component3.weaponType = __instance.projectileVariationTypes[__instance.variation];
            if (__instance.altVersion && __instance.variation != 1)
            {
                if (__instance.heatSinks >= 1f && __instance.variation != 2)
                    component3.hitAmount = Mathf.Lerp(3f, 1f, __instance.heatUp);
                else component3.hitAmount = 1f;
            }

            if (component3.sawblade)
                component3.ForceCheckSawbladeRicochet();
        }

        if (__instance.altVersion)
            MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Sawblade);

        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Nailgun.ShootMagnet))]
    private static bool ShootMagnet(Nailgun __instance)
    {
        __instance.UpdateAnimationWeight();
        GameObject gameObject = Object.Instantiate(__instance.magnetNail, Vars.DominantHand.transform.position, __instance.transform.rotation);
        gameObject.transform.forward = __instance.transform.forward;
        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
        {
            gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
        }

        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 100f, ForceMode.VelocityChange);
        if (__instance.canShoot)
        {
            __instance.anim.SetTrigger("Shoot");
        }

        Object.Instantiate(__instance.magnetShotSound);
        Magnet componentInChildren = gameObject.GetComponentInChildren<Magnet>();
        if ((bool)componentInChildren) __instance.wc.magnets.Add(componentInChildren);

        __instance.wc.naiMagnetCharge -= 1f;
        MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Magnet);
        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Nailgun.SuperSaw))]
    private static bool SuperSaw(Nailgun __instance)
    {
        __instance.fireCooldown = __instance.currentFireRate;
        __instance.shotSuccesfully = true;
        __instance.anim.SetLayerWeight(1, 0f);
        __instance.anim.SetTrigger("SuperShoot");
        MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.SuperSaw);
        __instance.barrelNum++;
        if (__instance.barrelNum >= __instance.shootPoints.Length)
        {
            __instance.barrelNum = 0;
        }

        Object.Instantiate(__instance.muzzleFlash2, __instance.shootPoints[__instance.barrelNum].transform);
        __instance.currentSpread = 0f;
        GameObject gameObject = Object.Instantiate(__instance.heatedNail, Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward, __instance.transform.rotation);
        gameObject.transform.forward = Vars.DominantHand.transform.forward;
        if (Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.forward, 1f, LayerMaskDefaults.Get(LMD.Environment)))
        {
            gameObject.transform.position = Vars.DominantHand.transform.position;
        }

        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
        {
            gameObject.transform.position = Vars.DominantHand.transform.position + (__instance.targeter.CurrentTarget.bounds.center - Vars.DominantHand.transform.position).normalized;
            gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
        }

        if (gameObject.TryGetComponent<Rigidbody>(out var component))
        {
            component.velocity = gameObject.transform.forward * 200f;
        }

        if (gameObject.TryGetComponent<Nail>(out var component2))
        {
            component2.weaponType = __instance.projectileVariationTypes[__instance.variation];
            component2.multiHitAmount = Mathf.RoundToInt(__instance.heatUp * 3f);
            component2.ForceCheckSawbladeRicochet();
            component2.sourceWeapon = __instance.gc.currentWeapon;
        }

        __instance.heatSinks -= 1f;
        __instance.heatUp = 0f;
        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Nailgun.ShootZapper))]
    private static bool ShootZapper(Nailgun __instance)
    {
        __instance.UpdateAnimationWeight();
        if ((bool)__instance.currentZapper)
        {
            __instance.currentZapper.Break();
        }

        __instance.currentZapper = Object.Instantiate(__instance.zapper, Vars.DominantHand.transform.position, __instance.transform.rotation);
        __instance.currentZapper.transform.forward = __instance.transform.forward;
        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
        {
            __instance.currentZapper.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
        }

        __instance.currentZapper.GetComponent<Rigidbody>().AddForce(__instance.currentZapper.transform.forward * 100f, ForceMode.VelocityChange);
        if (__instance.canShoot)
        {
            __instance.anim.SetTrigger("Shoot");
        }

        Object.Instantiate(__instance.magnetShotSound);
        __instance.currentZapper.lineStartTransform = __instance.zapperAttachTransform;
        __instance.currentZapper.connectedRB = MonoSingleton<NewMovement>.Instance.rb;
        __instance.currentZapper.sourceWeapon = __instance.gameObject;
        MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Magnet);
        return false;
    }
}
using HarmonyLib;
using VRTRAKILL.Data;
using UnityEngine;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(Shotgun))] internal static class PatchShotgun
{
    [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))]
    static void Transform(Shotgun __instance)
    {
        WeaponTransform.ApplyTransform(ref __instance.wpos, new(-.02f, .2f, .26f), new(), new(.1f, .1f, .1f));

        // add our own hand until hakita decides otherwise.
        Transform Hand = Object.Instantiate(Assets.HandPose_Shotgun.transform);
        // Shotgun ******(Clone)/ShogunNewAnims/GunArmature/MainBone
        Hand.SetParent(__instance.transform.GetChild(2).GetChild(0).GetChild(0), false);
        Hand.localPosition = Vector3.zero;
        Hand.localEulerAngles = new Vector3(0, 0, 270);
        Hand.localScale = new Vector3(.1f, .1f, .1f);

        Hand.GetChild(1).GetChild(0).localPosition = new(-.5f, -.95f, -.45f);
        Hand.GetChild(1).GetChild(0).localEulerAngles = new(0, 180, 0);
        Hand.GetChild(1).GetChild(0).localScale = new(1500, 1500, 1500);
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Shotgun.Shoot))]
    static bool Shoot(Shotgun __instance)
    {
        __instance.gunReady = false;
        int num = 12;
        if (__instance.variation == 1)
        {
            switch (__instance.primaryCharge)
            {
                case 0:
                    num = 10;
                    __instance.gunAud.pitch = Random.Range(1.15f, 1.25f);
                    break;
                case 1:
                    num = 16;
                    __instance.gunAud.pitch = Random.Range(0.95f, 1.05f);
                    break;
                case 2:
                    num = 24;
                    __instance.gunAud.pitch = Random.Range(0.75f, 0.85f);
                    break;
                case 3:
                    num = 0;
                    __instance.gunAud.pitch = Random.Range(0.75f, 0.85f);
                    break;
            }
        }
        Vector3 direction = Vars.DominantHand.transform.forward;
        if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
        {
            direction = __instance.targeter.CurrentTarget.bounds.center - Vars.DominantHand.transform.position;
        }
        __instance.rhits = Physics.RaycastAll(Vars.DominantHand.transform.position, direction, 4f, LayerMaskDefaults.Get(LMD.Enemies));
        if (__instance.rhits.Length != 0)
        {
            foreach (RaycastHit raycastHit in __instance.rhits)
            {
                if (raycastHit.collider.gameObject.tag == "Body")
                {
                    EnemyIdentifierIdentifier componentInParent = raycastHit.collider.GetComponentInParent<EnemyIdentifierIdentifier>();
                    if (componentInParent)
                    {
                        EnemyIdentifier eid = componentInParent.eid;
                        if (!eid.dead && !eid.blessed && __instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Equip"))
                        {
                            MonoSingleton<StyleHUD>.Instance.AddPoints(50, "ultrakill.quickdraw", __instance.gc.currentWeapon, eid, -1, "", "");
                        }
                        eid.hitter = "shotgunzone";
                        if (!eid.hitterWeapons.Contains("shotgun" + __instance.variation))
                        {
                            eid.hitterWeapons.Add("shotgun" + __instance.variation);
                        }
                        eid.DeliverDamage(raycastHit.collider.gameObject, (eid.transform.position - __instance.transform.position).normalized * 10000f, raycastHit.point, 4f, false, 0f, __instance.gameObject, false);
                    }
                }
            }
        }
        MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFireProjectiles, __instance.gameObject);
        if (__instance.variation != 1 || __instance.primaryCharge != 3)
        {
            for (int j = 0; j < num; j++)
            {
                GameObject gameObject = Object.Instantiate<GameObject>(__instance.bullet, Vars.DominantHand.transform.position, Vars.DominantHand.transform.rotation);
                Projectile component = gameObject.GetComponent<Projectile>();
                component.weaponType = "shotgun" + __instance.variation;
                component.sourceWeapon = __instance.gc.currentWeapon;
                if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                {
                    gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
                }
                if (__instance.variation == 1)
                {
                    switch (__instance.primaryCharge)
                    {
                        case 0:
                            gameObject.transform.Rotate(Random.Range(-__instance.spread / 1.5f, __instance.spread / 1.5f),
                                                        Random.Range(-__instance.spread / 1.5f, __instance.spread / 1.5f),
                                                        Random.Range(-__instance.spread / 1.5f, __instance.spread / 1.5f));
                            break;
                        case 1:
                            gameObject.transform.Rotate(Random.Range(-__instance.spread, __instance.spread),
                                                        Random.Range(-__instance.spread, __instance.spread),
                                                        Random.Range(-__instance.spread, __instance.spread));
                            break;
                        case 2:
                            gameObject.transform.Rotate(Random.Range(-__instance.spread * 2f, __instance.spread * 2f),
                                                        Random.Range(-__instance.spread * 2f, __instance.spread * 2f),
                                                        Random.Range(-__instance.spread * 2f, __instance.spread * 2f));
                            break;
                    }
                }
                else
                {
                    gameObject.transform.Rotate(Random.Range(-__instance.spread, __instance.spread),
                                                Random.Range(-__instance.spread, __instance.spread),
                                                Random.Range(-__instance.spread, __instance.spread));
                }
            }
        }
        else
        {
            Vector3 position = Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward;
            RaycastHit raycastHit2;
            if (Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.forward, out raycastHit2, 1f, LayerMaskDefaults.Get(LMD.Environment)))
            {
                position = raycastHit2.point - Vars.DominantHand.transform.forward * 0.1f;
            }
            GameObject gameObject2 = Object.Instantiate<GameObject>(__instance.explosion, position, Vars.DominantHand.transform.rotation);
            if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
            {
                gameObject2.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
            }
            foreach (Explosion explosion in gameObject2.GetComponentsInChildren<Explosion>())
            {
                explosion.sourceWeapon = __instance.gc.currentWeapon;
                explosion.enemyDamageMultiplier = 1f;
                explosion.maxSize *= 1.5f;
                explosion.damage = 50;
            }
        }
        if (__instance.variation != 1)
        {
            __instance.gunAud.pitch = Random.Range(0.95f, 1.05f);
        }
        __instance.gunAud.clip = __instance.shootSound;
        __instance.gunAud.volume = 0.45f;
        __instance.gunAud.panStereo = 0f;
        __instance.gunAud.Play();
        if (__instance.variation == 1)
        {
            __instance.anim.SetTrigger("PumpFire");
        }
        else
        {
            __instance.anim.SetTrigger("Fire");
        }
        foreach (Transform transform in __instance.shootPoints)
        {
            Object.Instantiate<GameObject>(__instance.muzzleFlash, transform.transform.position, transform.transform.rotation);
        }
        __instance.releasingHeat = false;
        __instance.tempColor.a = 1f;
        __instance.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", __instance.tempColor);
        if (__instance.variation == 1)
        {
            __instance.primaryCharge = 0;
        }

        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Shotgun.Update))]
    static bool UpdatePrefix(Shotgun __instance)
    {

        if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked && !__instance.charging)
        {
            if (!__instance.wid || __instance.wid.delay == 0f)
                __instance.Shoot();
            else
            {
                __instance.gunReady = false;
                __instance.Invoke("Shoot", __instance.wid.delay);
            }
        }

        if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && __instance.variation == 1 && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
        {
            __instance.gunReady = false;
            if (!__instance.wid || __instance.wid.delay == 0f)
            {
                __instance.Pump();
            }
            else
            {
                __instance.Invoke("Pump", __instance.wid.delay);
            }
        }

        if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && __instance.variation != 1 && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked && (__instance.variation != 2 || MonoSingleton<WeaponCharges>.Instance.shoSawCharge >= 1f))
        {
            __instance.charging = true;
            if (__instance.grenadeForce < 60f)
            {
                __instance.grenadeForce = Mathf.MoveTowards(__instance.grenadeForce, 60f, Time.deltaTime * 60f);
            }

            __instance.grenadeVector = new Vector3(Vars.DominantHand.transform.forward.x, Vars.DominantHand.transform.forward.y, Vars.DominantHand.transform.forward.z);
            if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
            {
                __instance.grenadeVector = Vector3.Normalize(__instance.targeter.CurrentTarget.bounds.center - Vars.DominantHand.transform.position);
            }

            __instance.grenadeVector += new Vector3(0f, __instance.grenadeForce * 0.002f, 0f);
            float num = 3000f;
            if (__instance.variation == 2)
            {
                num = 12000f;
            }

            __instance.transform.localPosition = new Vector3(__instance.wpos.currentDefault.x + Random.Range(__instance.grenadeForce / num * -1f, __instance.grenadeForce / num), __instance.wpos.currentDefault.y + Random.Range(__instance.grenadeForce / num * -1f, __instance.grenadeForce / num), __instance.wpos.currentDefault.z + Random.Range(__instance.grenadeForce / num * -1f, __instance.grenadeForce / num));
            if (__instance.tempChargeSound == null)
            {
                GameObject gameObject = Object.Instantiate(__instance.chargeSoundBubble);
                __instance.tempChargeSound = gameObject.GetComponent<AudioSource>();
                if ((bool)__instance.wid && __instance.wid.delay > 0f)
                {
                    __instance.tempChargeSound.volume -= __instance.wid.delay * 2f;
                    if (__instance.tempChargeSound.volume < 0f)
                    {
                        __instance.tempChargeSound.volume = 0f;
                    }
                }
            }

            MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.ShotgunCharge, __instance.tempChargeSound.gameObject).intensityMultiplier = __instance.grenadeForce / 60f;
            if (__instance.variation == 0)
            {
                __instance.tempChargeSound.pitch = __instance.grenadeForce / 60f;
            }
            else
            {
                __instance.tempChargeSound.pitch = (__instance.grenadeForce / 2f + 30f) / 60f;
            }
        }

        if ((MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasCanceledThisFrame
        || (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo()
          && !GameStateManager.Instance.PlayerInputLocked
          && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame))
        && __instance.variation != 1 && __instance.gunReady && __instance.gc.activated && __instance.charging)
        {
            __instance.charging = false;
            if (__instance.variation == 2)
            {
                MonoSingleton<WeaponCharges>.Instance.shoSawCharge = 0f;
            }

            if (!__instance.wid || __instance.wid.delay == 0f)
            {
                if (__instance.variation == 0)
                {
                    __instance.ShootSinks();
                }
                else
                {
                    __instance.ShootSaw();
                }
            }
            else
            {
                __instance.gunReady = false;
                __instance.Invoke((__instance.variation == 0) ? "ShootSinks" : "ShootSaw", __instance.wid.delay);
            }

            Object.Destroy(__instance.tempChargeSound.gameObject);
        }

        if (__instance.variation == 2)
        {
            if (__instance.charging && __instance.chainsawBladeScroll.scrollSpeedX == 0f)
            {
                __instance.chainsawBladeRenderer.material = __instance.chainsawBladeMotionMaterial;
            }
            else if (!__instance.charging && __instance.chainsawBladeScroll.scrollSpeedX > 0f)
            {
                __instance.chainsawBladeRenderer.material = __instance.chainsawBladeMaterial;
            }

            __instance.chainsawBladeScroll.scrollSpeedX = __instance.grenadeForce / 6f;
            __instance.anim.SetBool("Sawing", __instance.charging);
            __instance.sawZone.enabled = __instance.charging;
            if (__instance.charging && Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.forward, out var hitInfo, 3f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
            {
                ((Component)(object)__instance.environmentalSawSpark).transform.position = hitInfo.point;
                if (!__instance.environmentalSawSpark.isEmitting)
                {
                    __instance.environmentalSawSpark.Play();
                }

                if (!__instance.environmentalSawSound.isPlaying)
                {
                    __instance.environmentalSawSound.Play();
                }
            }
            else
            {
                if (__instance.environmentalSawSpark.isEmitting)
                {
                    __instance.environmentalSawSpark.Stop();
                }

                if (__instance.environmentalSawSound.isPlaying)
                {
                    __instance.environmentalSawSound.Stop();
                }
            }
        }

        if (__instance.releasingHeat)
        {
            __instance.tempColor.a -= Time.deltaTime * 2.5f;
            __instance.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", __instance.tempColor);
        }

        __instance.UpdateMeter();

        return false;
    }

    [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Update))]
    static void UpdatePostfix(Shotgun __instance)
    {
        // reset shitty animator angles ruining my immersion
        if (__instance.anim.GetBool("Sawing"))
            __instance.transform.GetChild(2).localEulerAngles = new(__instance.transform.GetChild(2).localEulerAngles.x, __instance.transform.GetChild(2).localEulerAngles.y, 25);
        else
            __instance.transform.GetChild(2).localEulerAngles = new(__instance.transform.GetChild(2).localEulerAngles.x, __instance.transform.GetChild(2).localEulerAngles.y, 0);
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Shotgun.ShootSinks))]
    static bool ShootSinks(Shotgun __instance)
    {
        __instance.gunReady = false;
        __instance.transform.localPosition = __instance.wpos.currentDefault;
        foreach (Transform transform in __instance.shootPoints)
        {
            GameObject gameObject = Object.Instantiate<GameObject>(__instance.grenade, Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward * 0.5f, Random.rotation);
            gameObject.GetComponentInChildren<Grenade>().sourceWeapon = __instance.gc.currentWeapon;
            gameObject.GetComponent<Collider>();
            gameObject.GetComponent<Rigidbody>().AddForce(__instance.grenadeVector * (__instance.grenadeForce + 10f), ForceMode.VelocityChange);
        }
        Object.Instantiate<GameObject>(__instance.grenadeSoundBubble).GetComponent<AudioSource>().volume = 0.45f * Mathf.Sqrt(Mathf.Pow(1f, 2f) - Mathf.Pow(__instance.grenadeForce, 2f) / Mathf.Pow(60f, 2f));
        __instance.anim.SetTrigger("Secondary Fire");
        __instance.gunAud.clip = __instance.shootSound;
        __instance.gunAud.volume = 0.45f * (__instance.grenadeForce / 60f);
        __instance.gunAud.panStereo = 0f;
        __instance.gunAud.pitch = Random.Range(0.75f, 0.85f);
        __instance.gunAud.Play();
        __instance.meterOverride = true;
        __instance.chargeSlider.value = 0f;
        __instance.sliderFill.color = Color.black;
        foreach (Transform transform2 in __instance.shootPoints)
        {
            Object.Instantiate<GameObject>(__instance.muzzleFlash, transform2.transform.position, transform2.transform.rotation);
        }
        __instance.releasingHeat = false;
        __instance.tempColor.a = 0f;
        __instance.heatSinkSMR.sharedMaterials[3].SetColor("_TintColor", __instance.tempColor);
        __instance.grenadeForce = 0f;

        return false;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(Shotgun.ShootSaw))]
    static bool ShootSaw(Shotgun __instance)
    {
        __instance.gunReady = true;
        Transform[] array = __instance.shootPoints;
        for (int i = 0; i < array.Length; i++)
        {
            _ = array[i];
            Vector3 position = Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward * 0.5f;
            if (Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.forward, out var hitInfo, 5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
                position = hitInfo.point - Vars.DominantHand.transform.forward * 5f;

            Chainsaw chainsaw = Object.Instantiate(__instance.chainsaw, position, Random.rotation);
            chainsaw.weaponType = "shotgun" + __instance.variation;
            chainsaw.CheckMultipleRicochets(onStart: true);
            chainsaw.sourceWeapon = __instance.gc.currentWeapon;
            chainsaw.attachedTransform = MonoSingleton<PlayerTracker>.Instance.GetTarget();
            chainsaw.lineStartTransform = __instance.chainsawAttachPoint;
            chainsaw.GetComponent<Rigidbody>().AddForce(Vars.DominantHand.transform.forward * (__instance.grenadeForce + 10f) * 1.5f, ForceMode.VelocityChange);
            __instance.currentChainsaws.Add(chainsaw);
        }

        __instance.chainsawBladeRenderer.material = __instance.chainsawBladeMaterial;
        __instance.chainsawBladeScroll.scrollSpeedX = 0f;
        __instance.chainsawAttachPoint.gameObject.SetActive(value: false);
        Object.Instantiate(__instance.grenadeSoundBubble).GetComponent<AudioSource>().volume = 0.45f * Mathf.Sqrt(Mathf.Pow(1f, 2f) - Mathf.Pow(__instance.grenadeForce, 2f) / Mathf.Pow(60f, 2f));
        __instance.anim.Play("FireNoReload");
        __instance.gunAud.clip = __instance.shootSound;
        __instance.gunAud.volume = 0.45f * Mathf.Max(0.5f, __instance.grenadeForce / 60f);
        __instance.gunAud.panStereo = 0f;
        __instance.gunAud.pitch = Random.Range(0.75f, 0.85f);
        __instance.gunAud.Play();
        __instance.releasingHeat = false;
        __instance.grenadeForce = 0f;
        return false;
    }
}
using HarmonyLib;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Guns.Patches
{
    [HarmonyPatch] internal sealed class AimingP
    {
        // Replaces all guns shooting raycasts directions with controller directions
        [HarmonyPrefix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.Shoot))] static bool RevolverA(int shotType, Revolver __instance)
        {
            __instance.shootReady = false;
            __instance.shootCharge = 0f;
            if (__instance.altVersion) MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[__instance.gunVariation] = 2f;
            var altShootPos = Vars.DominantHand.transform.position + (Vars.DominantHand.transform.forward * 1.25f) + new Vector3(0, .035f, 0);

            switch (shotType)
            {
                case 1:
                    {
                        GameObject gameObject2 = Object.Instantiate(__instance.revolverBeam, Vars.DominantHand.transform.position, Vars.DominantHand.transform.rotation);
                        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                            gameObject2.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);

                        RevolverBeam component2 = gameObject2.GetComponent<RevolverBeam>();
                        component2.sourceWeapon = __instance.gc.currentWeapon;
                        component2.alternateStartPoint = altShootPos;
                        component2.gunVariation = __instance.gunVariation;
                        if (__instance.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                            component2.quickDraw = true;

                        __instance.currentGunShot = Random.Range(0, __instance.gunShots.Length);
                        __instance.gunAud.clip = __instance.gunShots[__instance.currentGunShot];
                        __instance.gunAud.volume = 0.55f;
                        __instance.gunAud.pitch = Random.Range(0.9f, 1.1f);
                        __instance.gunAud.Play();
                        MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFire, __instance.gameObject);
                        break;
                    }
                case 2:
                    {
                        GameObject gameObject = Object.Instantiate(__instance.revolverBeamSuper, Vars.DominantHand.transform.position, Vars.DominantHand.transform.rotation);
                        if ((bool)__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                        {
                            gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
                        }

                        RevolverBeam component = gameObject.GetComponent<RevolverBeam>();
                        component.sourceWeapon = __instance.gc.currentWeapon;
                        component.alternateStartPoint = __instance.gunBarrel.transform.position;
                        component.gunVariation = __instance.gunVariation;
                        if (__instance.gunVariation == 2)
                            component.ricochetAmount = Mathf.Min(3, Mathf.FloorToInt(__instance.pierceShotCharge / 25f));

                        __instance.pierceShotCharge = 0f;
                        if (__instance.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                            component.quickDraw = true;

                        __instance.pierceReady = false;
                        __instance.pierceCharge = 0f;
                        if (__instance.gunVariation == 0)
                        {
                            __instance.screenAud.clip = __instance.chargingSound;
                            __instance.screenAud.loop = true;
                            if (__instance.altVersion) __instance.screenAud.pitch = 0.5f;
                            else __instance.screenAud.pitch = 1f;

                            __instance.screenAud.volume = 0.55f;
                            __instance.screenAud.Play();
                        }
                        else if (!__instance.wid || __instance.wid.delay == 0f)
                            __instance.wc.rev2charge -= (__instance.altVersion ? 300 : 100);

                        if ((bool)__instance.superGunSound)
                            Object.Instantiate(__instance.superGunSound);

                        if (__instance.gunVariation == 2 && (bool)__instance.twirlShotSound)
                            Object.Instantiate(__instance.twirlShotSound, __instance.transform.position, Quaternion.identity);
                        MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFireStrong, __instance.gameObject);
                        break;
                    }
            }

            if (!__instance.altVersion)
                __instance.cylinder.DoTurn();

            __instance.anim.SetFloat("RandomChance", Random.Range(0f, 1f));
            if (shotType == 1) __instance.anim.SetTrigger("Shoot");
            else __instance.anim.SetTrigger("ChargeShoot");

            __instance.gunReady = false;

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(Revolver), nameof(Revolver.ThrowCoin))] static bool RevolverCoin(Revolver __instance)
        {
            if (__instance.punch == null || !__instance.punch.gameObject.activeInHierarchy)
                __instance.punch = MonoSingleton<FistControl>.Instance.currentPunch;

            if ((bool)__instance.punch) __instance.punch.CoinFlip();

            GameObject obj;
            if (Vars.Config.EnableMBP)
                obj = Object.Instantiate(__instance.coin,
                                         Vars.NonDominantHand.transform.position + Vars.NonDominantHand.transform.up * -.5f,
                                         Vars.NonDominantHand.transform.rotation);
            else if (Vars.Config.EnableCBS)
                obj = Object.Instantiate(__instance.coin,
                                         Vars.DominantHand.transform.position + Vars.DominantHand.transform.up * -.5f,
                                         Vars.DominantHand.transform.rotation);
            else obj = Object.Instantiate(__instance.coin,
                                          __instance.camObj.transform.position + __instance.camObj.transform.up * -0.5f,
                                          __instance.camObj.transform.rotation);

            obj.GetComponent<Coin>().sourceWeapon = __instance.gc.currentWeapon;

            MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.CoinToss);

            Vector3 zero = Vector3.zero;
            obj.GetComponent<Rigidbody>().AddForce(Vars.DominantHand.transform.forward * 20f + Vector3.up * 15f
                                                   + (MonoSingleton<NewMovement>.Instance.ridingRocket
                                                      ? MonoSingleton<NewMovement>.Instance.ridingRocket.rb.velocity
                                                      : MonoSingleton<NewMovement>.Instance.rb.velocity) + zero,
                                                   ForceMode.VelocityChange);
            __instance.pierceCharge = 0f;
            __instance.pierceReady = false;

            return false;
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Shoot))] static bool ShotgunA(Shotgun __instance)
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
            __instance.rhits = Physics.RaycastAll(Vars.DominantHand.transform.position, direction, 4f, __instance.shotgunZoneLayerMask);
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
        [HarmonyPrefix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.Update))] static bool ShotgunBA(Shotgun __instance)
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
        [HarmonyPrefix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.ShootSinks))] static bool ShotgunBB(Shotgun __instance)
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
        [HarmonyPrefix] [HarmonyPatch(typeof(Shotgun), nameof(Shotgun.ShootSaw))] static bool ShotgunBC(Shotgun __instance)
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

        [HarmonyPrefix] [HarmonyPatch(typeof(ShotgunHammer), nameof(ShotgunHammer.Impact))] static bool HammerA(ShotgunHammer __instance)
        {
            bool hasHitProjectile = false;
            __instance.hitEnemy = null;
            __instance.hitGrenade = null;
            __instance.target = null;
            __instance.hitPosition = Vector3.zero;
            __instance.hammerCooldown = 0.5f;
            Vector3 position = __instance.transform.position;
            __instance.direction = __instance.transform.forward;
            if (MonoSingleton<ObjectTracker>.Instance.grenadeList.Count > 0 || MonoSingleton<WeaponCharges>.Instance.shoSawAmount > 0 || MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0)
            {
                Collider[] cols = Physics.OverlapSphere(position, 0.01f);
                if (cols.Length != 0)
                {
                    for (int j = 0; j < cols.Length; j++)
                    {
                        Transform transform = cols[j].transform;
                        if (transform.TryGetComponent<ParryHelper>(out var component))
                        {
                            transform = component.target;
                        }

                        if (MonoSingleton<ObjectTracker>.Instance.grenadeList.Count > 0 && transform.gameObject.layer == 10)
                        {
                            Grenade componentInParent = transform.GetComponentInParent<Grenade>();
                            if ((bool)componentInParent)
                            {
                                hasHitProjectile = true;
                                __instance.hitGrenade = componentInParent;
                                cols[j].enabled = false;
                                Object.Instantiate(__instance.hitSound, __instance.transform.position, Quaternion.identity);
                                MonoSingleton<TimeController>.Instance.TrueStop(0.25f);
                                __instance.HitNade();
                            }
                        }
                        else if (MonoSingleton<WeaponCharges>.Instance.shoSawAmount > 0 || MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0)
                        {
                            if (MonoSingleton<WeaponCharges>.Instance.shoSawAmount > 0 && transform.TryGetComponent<Chainsaw>(out var component2))
                            {
                                hasHitProjectile = true;
                                Object.Instantiate(__instance.hitSound, __instance.transform.position, Quaternion.identity);
                                component2.GetPunched();
                                component2.transform.position = Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward;
                                component2.rb.velocity = (Punch.GetParryLookTarget() - component2.transform.position).normalized * 105f;
                            }
                            else if (MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0 && transform.TryGetComponent<Landmine>(out Landmine component3))
                            {
                                hasHitProjectile = true;
                                component3.transform.LookAt(Punch.GetParryLookTarget());
                                component3.Parry();
                                Object.Instantiate(__instance.hitSound, __instance.transform.position, Quaternion.identity);
                                __instance.anim.Play("Fire", -1, 0f);
                                MonoSingleton<TimeController>.Instance.TrueStop(0.25f);
                            }
                        }
                    }
                }
            }

            if (MonoSingleton<WeaponCharges>.Instance.shoSawAmount > 0 || MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0)
            {
                RaycastHit[] rhits = Physics.RaycastAll(position, __instance.direction, 8f, 16384, QueryTriggerInteraction.Collide);
                for (int j = 0; j < rhits.Length; j++)
                {
                    Transform transform2 = rhits[j].transform;
                    if (transform2.TryGetComponent<ParryHelper>(out var component4))
                    {
                        transform2 = component4.target;
                    }

                    Landmine component6;
                    if (transform2.TryGetComponent<Chainsaw>(out var component5))
                    {
                        hasHitProjectile = true;
                        Object.Instantiate(__instance.hitSound, __instance.transform.position, Quaternion.identity);
                        component5.GetPunched();
                        component5.transform.position = __instance.transform.position + __instance.transform.forward;
                        component5.rb.velocity = (Punch.GetParryLookTarget() - component5.transform.position).normalized * 105f;
                    }
                    else if (transform2.TryGetComponent<Landmine>(out component6))
                    {
                        hasHitProjectile = true;
                        component6.transform.LookAt(Punch.GetParryLookTarget());
                        component6.Parry();
                        Object.Instantiate(__instance.hitSound, __instance.transform.position, Quaternion.identity);
                        __instance.anim.Play("Fire", -1, 0f);
                        MonoSingleton<TimeController>.Instance.TrueStop(0.25f);
                        
                    }
                }
            }

            if (!hasHitProjectile && Physics.Raycast(position, __instance.direction, out var rhit, 8f, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment), QueryTriggerInteraction.Collide))
            {
                if (rhit.transform.gameObject.layer == 11 || rhit.transform.gameObject.layer == 10)
                {
                    EnemyIdentifierIdentifier component10;
                    if (rhit.transform.gameObject.TryGetComponent<ParryHelper>(out var component7))
                    {
                        EnemyIdentifier component9;
                        if (component7.target.TryGetComponent<EnemyIdentifierIdentifier>(out var component8) && (bool)component8.eid && !component8.eid.dead)
                        {
                            __instance.hitEnemy = component8.eid;
                        }
                        else if (component7.target.TryGetComponent<EnemyIdentifier>(out component9) && !component9.dead)
                        {
                            __instance.hitEnemy = component9;
                        }
                    }
                    else if (rhit.transform.TryGetComponent<EnemyIdentifierIdentifier>(out component10) && (bool)component10.eid && !component10.eid.dead)
                    {
                        __instance.hitEnemy = component10.eid;
                    }
                    else if (MonoSingleton<ObjectTracker>.Instance.grenadeList.Count > 0 && rhit.transform.gameObject.layer == 10)
                    {
                        Grenade componentInParent2 = rhit.transform.GetComponentInParent<Grenade>();
                        if ((bool)componentInParent2)
                        {
                            hasHitProjectile = true;
                            __instance.hitGrenade = componentInParent2;
                            rhit.collider.enabled = false;
                            Object.Instantiate(__instance.hitSound, __instance.transform.position, Quaternion.identity);
                            __instance.anim.Play("Fire", -1, 0f);
                            MonoSingleton<TimeController>.Instance.TrueStop(0.25f);
                            __instance.HitNade();
                        }
                    }
                }

                __instance.target = rhit.transform;
                __instance.hitPosition = rhit.point;
            }

            if (!hasHitProjectile && __instance.hitEnemy == null)
            {
                Vector3 vector = position + __instance.direction * 2.5f;
                Collider[] array = Physics.OverlapSphere(vector, 2.5f);
                if (array.Length != 0)
                {
                    float num = 2.5f;
                    for (int k = 0; k < array.Length; k++)
                    {
                        if (array[k].TryGetComponent<ParryHelper>(out var component11) && component11.target.TryGetComponent<Collider>(out var component12))
                        {
                            array[k] = component12;
                        }

                        if (array[k].gameObject.layer != 10 && array[k].gameObject.layer != 11)
                        {
                            continue;
                        }

                        Vector3 vector2 = array[k].ClosestPoint(vector);
                        if (Physics.Raycast(position, vector2 - position, out rhit, Vector3.Distance(vector2, position), LayerMaskDefaults.Get(LMD.Environment)))
                        {
                            continue;
                        }

                        float num2 = Vector3.Distance(vector, vector2);
                        if (num2 < num)
                        {
                            Transform transform3 = ((array[k].attachedRigidbody != null) ? array[k].attachedRigidbody.transform : array[k].transform);
                            EnemyIdentifier component13 = null;
                            if (transform3.TryGetComponent<EnemyIdentifierIdentifier>(out var component14))
                            {
                                component13 = component14.eid;
                            }
                            else
                            {
                                transform3.TryGetComponent<EnemyIdentifier>(out component13);
                            }

                            if ((bool)component13 && (!component13.dead || __instance.hitEnemy == null))
                            {
                                __instance.hitEnemy = component14.eid;
                                num = num2;
                                __instance.target = transform3;
                                __instance.hitPosition = vector2;
                            }
                        }
                    }
                }

                RaycastHit[] array2 = Physics.SphereCastAll(position + __instance.direction * 2.5f, 2.5f, __instance.direction, 3f, LayerMaskDefaults.Get(LMD.Enemies));
                if (array2.Length != 0)
                {
                    float num3 = -1f;
                    if (__instance.hitEnemy != null)
                    {
                        num3 = Vector3.Dot(__instance.direction, __instance.hitPosition - position);
                    }

                    for (int l = 0; l < array2.Length; l++)
                    {
                        if (Physics.Raycast(position, array2[l].point - position, out rhit, Vector3.Distance(array2[l].point, position), LayerMaskDefaults.Get(LMD.Environment)))
                        {
                            continue;
                        }

                        float num4 = Vector3.Dot(__instance.direction, array2[l].point - position);
                        if (num4 > num3)
                        {
                            Transform transform4 = array2[l].transform;
                            Vector3 point = array2[l].point;
                            if (transform4.TryGetComponent<ParryHelper>(out var component15))
                            {
                                transform4 = component15.target.transform;
                            }

                            EnemyIdentifier component16 = null;
                            if (transform4.TryGetComponent<EnemyIdentifierIdentifier>(out var component17))
                            {
                                component16 = component17.eid;
                            }
                            else
                            {
                                transform4.TryGetComponent<EnemyIdentifier>(out component16);
                            }

                            if ((bool)component16 && (!component16.dead || __instance.hitEnemy == null))
                            {
                                __instance.hitEnemy = component16;
                                num3 = num4;
                                __instance.target = transform4;
                                __instance.hitPosition = point;
                            }
                        }
                    }
                }
            }

            __instance.forceWeakHit = true;
            if (__instance.target != null)
            {
                if (__instance.hitEnemy != null)
                {
                    float num5 = 0.05f;
                    __instance.damage = 3f;
                    if (__instance.tier == 2)
                    {
                        num5 = 0.5f;
                        __instance.damage = 12f;
                    }
                    else if (__instance.tier == 1)
                    {
                        num5 = 0.25f;
                        __instance.damage = 6f;
                    }

                    if (__instance.hitEnemy.dead)
                    {
                        num5 = 0f;
                    }

                    if (num5 > 0f)
                    {
                        __instance.forceWeakHit = false;
                        __instance.launchPlayer = true;
                        Object.Instantiate(__instance.hitSound, __instance.transform.position, Quaternion.identity);
                        MonoSingleton<TimeController>.Instance.TrueStop(num5);
                    }
                    else
                    {
                        __instance.launchPlayer = false;
                    }

                    __instance.DeliverDamage();
                }
                else if (__instance.target.TryGetComponent<Breakable>(out Breakable component18) && !component18.precisionOnly && !component18.unbreakable)
                {
                    component18.Break();
                }
                else if (__instance.target.TryGetComponent<Glass>(out Glass component19) && !component19.broken)
                {
                    component19.Shatter();
                }
            }

            if (!__instance.hitGrenade && __instance.hitEnemy == null && __instance.target != null && (__instance.target.gameObject.layer == 8 || __instance.target.gameObject.layer == 24))
            {
                MonoSingleton<NewMovement>.Instance.Launch(-__instance.direction * ((float)(100 * __instance.tier + 300) / ((float)(MonoSingleton<NewMovement>.Instance.hammerJumps + 3) / 3f)));
                MonoSingleton<NewMovement>.Instance.hammerJumps++;
            }

            __instance.ImpactEffects();
            __instance.hitEnemy = null;
            __instance.hitGrenade = null;

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(ShotgunHammer), nameof(ShotgunHammer.HitNade))] static bool HammerHitNade(ShotgunHammer __instance)
        {
            if (Physics.Raycast(__instance.transform.position, __instance.direction, out var hitInfo, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment), QueryTriggerInteraction.Ignore))
                __instance.hitGrenade.GrenadeBeam(hitInfo.point);
            else
                __instance.hitGrenade.GrenadeBeam(__instance.transform.position + __instance.direction * 1000f);
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(ShotgunHammer), nameof(ShotgunHammer.ImpactEffects))] static bool HammerIE(ShotgunHammer __instance)
        {
            Vector3 position = ((__instance.hitPosition != Vector3.zero) ? (__instance.hitPosition - (__instance.hitPosition - __instance.transform.position).normalized) : (__instance.transform.position + __instance.direction * 2.5f));
            if (__instance.primaryCharge > 0)
            {
                GameObject gameObject = Object.Instantiate((__instance.primaryCharge == 3) ? __instance.overPumpExplosion : __instance.pumpExplosion, position, Quaternion.LookRotation(__instance.direction));
                Explosion[] componentsInChildren = gameObject.GetComponentsInChildren<Explosion>();
                foreach (Explosion explosion in componentsInChildren)
                {
                    explosion.sourceWeapon = __instance.gameObject;
                    explosion.hitterWeapon = "hammer";
                    if (__instance.primaryCharge == 2)
                    {
                        explosion.maxSize *= 2f;
                    }
                }

                if (__instance.primaryCharge == 2 && gameObject.TryGetComponent<AudioSource>(out var component))
                {
                    component.volume = 1f;
                    component.pitch -= 0.4f;
                }

                __instance.primaryCharge = 0;
            }

            if (__instance.forceWeakHit || __instance.tier == 0)
            {
                __instance.anim.Play("Fire", -1, 0f);
            }
            else if (__instance.tier == 1)
            {
                __instance.anim.Play("FireStrong", -1, 0f);
            }
            else
            {
                __instance.anim.Play("FireStrongest", -1, 0f);
            }

            Object.Instantiate(__instance.hitImpactParticle[(!__instance.forceWeakHit) ? __instance.tier : 0], position, __instance.transform.rotation);
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(ShotgunHammer), nameof(ShotgunHammer.ThrowNade))] static bool HammerTN(ShotgunHammer __instance)
        {
            MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge = 0f;
            __instance.pulledOut = 0.3f;
            __instance.gunReady = false;
            __instance.aboutToSecondary = false;
            Vector3 up = __instance.transform.up;
            if (Vector3.Dot(__instance.transform.forward, Vector3.up) > Vector3.Dot(__instance.transform.forward, up))
            {
                up = Vector3.up;
            }

            GameObject obj = Object.Instantiate(__instance.grenade, __instance.transform.position + __instance.transform.forward * 2f - up * 0.5f, Random.rotation);
            if (obj.TryGetComponent<Rigidbody>(out var component))
            {
                component.velocity = MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity() + __instance.transform.forward * 3f + up * (MonoSingleton<UnderwaterController>.Instance.inWater ? 4 : 10);
            }

            if (obj.TryGetComponent<Grenade>(out var component2))
            {
                component2.sourceWeapon = __instance.gameObject;
            }

            __instance.anim.Play("NadeSpawn", -1, 0f);
            Object.Instantiate(__instance.nadeSpawnSound);
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(ShotgunHammer), nameof(ShotgunHammer.ShootSaw))] static bool HammerSS(ShotgunHammer __instance)
        {
            __instance.gunReady = true;
            __instance.transform.localPosition = __instance.wpos.currentDefault;
            Vector3 position = __instance.transform.position + __instance.transform.forward * 0.5f;
            if (Physics.Raycast(Vars.DominantHand.transform.position, __instance.transform.forward, out var hitInfo, 5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
            {
                position = hitInfo.point - __instance.transform.forward * 5f;
            }

            Chainsaw chainsaw = Object.Instantiate(__instance.chainsaw, position, Random.rotation);
            chainsaw.weaponType = "hammer" + __instance.variation;
            chainsaw.CheckMultipleRicochets(onStart: true);
            chainsaw.sourceWeapon = __instance.gc.currentWeapon;
            chainsaw.attachedTransform = MonoSingleton<PlayerTracker>.Instance.GetTarget();
            chainsaw.lineStartTransform = __instance.chainsawAttachPoint;
            chainsaw.GetComponent<Rigidbody>().AddForce(__instance.transform.forward * (__instance.chargeForce + 10f) * 1.5f, ForceMode.VelocityChange);
            __instance.currentChainsaws.Add(chainsaw);
            __instance.chainsawBladeRenderer.material = __instance.chainsawBladeMaterial;
            __instance.chainsawBladeScroll.scrollSpeedX = 0f;
            __instance.chainsawAttachPoint.gameObject.SetActive(value: false);
            Object.Instantiate(__instance.nadeSpawnSound);
            __instance.anim.Play("SawingShot");
            __instance.chargeForce = 0f;
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(ShotgunHammer), nameof(ShotgunHammer.Update))] static bool HammerUpdate(ShotgunHammer __instance)
        {
            __instance.overheated = MonoSingleton<WeaponCharges>.Instance.shoaltcooldowns[__instance.variation] > 0f;
            if (__instance.overheated && !__instance.overheatAud.isPlaying)
            {
                __instance.overheatAud.Play();
                __instance.overheatParticle.Play();
                __instance.anim.SetBool("Cooldown", value: true);
            }
            else if (!__instance.overheated && __instance.overheatAud.isPlaying)
            {
                __instance.overheatAud.Stop();
                __instance.overheatParticle.Stop();
                __instance.anim.SetBool("Cooldown", value: false);
            }

            float num = Mathf.Min(MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity().magnitude / 60f, 1f);
            __instance.currentSpeed = (__instance.overheated ? 0f : Mathf.MoveTowards(__instance.currentSpeed, num, Time.deltaTime * 2f));
            if (MonoSingleton<HookArm>.Instance.beingPulled)
            {
                __instance.currentSpeed = Mathf.Min(__instance.currentSpeed, 0.5f);
            }

            __instance.UpdateMeter();
            if ((float)__instance.pulledOut >= 0.5f)
            {
                __instance.gunReady = true;
            }

            if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && !__instance.chargingSwing && __instance.hammerCooldown <= 0f && !__instance.overheated && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && (!__instance.fireHeldOnPullOut || (float)__instance.pulledOut >= 0.25f) && __instance.gc.activated)
            {
                __instance.fireHeldOnPullOut = false;
                __instance.chargingSwing = true;
            }
            else if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && !MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && __instance.swingCharge == 1f && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
            {
                __instance.chargingSwing = false;
                __instance.swingCharge = 0f;
                if (!__instance.wid || __instance.wid.delay == 0f)
                {
                    __instance.Impact();
                }
                else
                {
                    __instance.gunReady = false;
                    __instance.Invoke("Impact", __instance.wid.delay);
                }
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && __instance.variation == 2 && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked && MonoSingleton<WeaponCharges>.Instance.shoSawCharge >= 1f)
            {
                __instance.charging = true;
                if (__instance.chargeForce < 60f)
                {
                    __instance.chargeForce = Mathf.MoveTowards(__instance.chargeForce, 60f, Time.deltaTime * 60f);
                }

                float num2 = 12000f;
                __instance.transform.localPosition = new Vector3(__instance.wpos.currentDefault.x + Random.Range(__instance.chargeForce / num2 * -1f, __instance.chargeForce / num2), __instance.wpos.currentDefault.y + Random.Range(__instance.chargeForce / num2 * -1f, __instance.chargeForce / num2), __instance.wpos.currentDefault.z + Random.Range(__instance.chargeForce / num2 * -1f, __instance.chargeForce / num2));
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

                MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.ShotgunCharge, __instance.tempChargeSound.gameObject).intensityMultiplier = __instance.chargeForce / 60f;
                __instance.tempChargeSound.pitch = (__instance.chargeForce / 2f + 30f) / 60f;
            }

            if (!MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && __instance.variation == 2 && __instance.gunReady && __instance.gc.activated && __instance.charging)
            {
                __instance.charging = false;
                MonoSingleton<WeaponCharges>.Instance.shoSawCharge = 0f;
                if (!__instance.wid || __instance.wid.delay == 0f)
                {
                    __instance.ShootSaw();
                }
                else
                {
                    __instance.gunReady = false;
                    __instance.Invoke("ShootSaw", __instance.wid.delay);
                }

                if ((bool)__instance.tempChargeSound)
                {
                    Object.Destroy(__instance.tempChargeSound.gameObject);
                }
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

                __instance.chainsawBladeScroll.scrollSpeedX = __instance.chargeForce / 6f;
                __instance.anim.SetBool("Sawing", __instance.charging);
                __instance.sawZone.enabled = __instance.charging;
                if (__instance.charging && Physics.Raycast(__instance.transform.position, __instance.transform.forward, out var hitInfo, 3f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
                {
                    __instance.environmentalSawSpark.transform.position = hitInfo.point;
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

            if (__instance.chargingSwing)
            {
                __instance.swingCharge = Mathf.MoveTowards(__instance.swingCharge, 1f, Time.deltaTime * 2f);
            }

            __instance.modelTransform.localPosition = new Vector3(__instance.defaultModelPosition.x + Random.Range((0f - __instance.swingCharge) / 30f, __instance.swingCharge / 30f), __instance.defaultModelPosition.y + Random.Range((0f - __instance.swingCharge) / 30f, __instance.swingCharge / 30f), __instance.defaultModelPosition.z + Random.Range((0f - __instance.swingCharge) / 30f, __instance.swingCharge / 30f));
            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && __instance.variation != 2 && (__instance.variation == 1 || MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge >= 2f) && !__instance.aboutToSecondary && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
            {
                __instance.gunReady = false;
                if (!__instance.wid || __instance.wid.delay == 0f)
                {
                    if (__instance.variation == 0)
                    {
                        __instance.ThrowNade();
                    }
                    else
                    {
                        __instance.Pump();
                    }
                }
                else
                {
                    __instance.aboutToSecondary = true;
                    __instance.Invoke((__instance.variation == 0) ? "ThrowNade" : "Pump", __instance.wid.delay);
                }
            }

            if (__instance.secondaryMeterFill >= 1f)
            {
                __instance.secondaryMeter.fillAmount = 1f;
            }
            else if (__instance.secondaryMeterFill <= 0f)
            {
                __instance.secondaryMeter.fillAmount = 0f;
            }
            else
            {
                __instance.secondaryMeter.fillAmount = Mathf.Lerp(0.275f, 0.625f, __instance.secondaryMeterFill);
            }

            if (__instance.hammerCooldown > 0f)
            {
                __instance.hammerCooldown = Mathf.MoveTowards(__instance.hammerCooldown, 0f, Time.deltaTime);
            }

            if (NoWeaponCooldown.NoCooldown)
            {
                MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge = 2f;
            }

            if (MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge < 2f)
            {
                __instance.nadeCharging = true;
            }
            else if (__instance.nadeCharging)
            {
                __instance.nadeCharging = false;
                Object.Instantiate(__instance.nadeReadySound);
            }

            return false;
        }

        [HarmonyPostfix] [HarmonyPatch(typeof(ShotgunHammer), nameof(ShotgunHammer.LateUpdate))] static void Hammer_VRIK(ShotgunHammer __instance)
        {
            if (VRAvatar.VRigController.Instance != null)
            {
                __instance.transform.position = VRAvatar.VRigController.Instance.Rig.FeedbackerB.Forearm.position;
                __instance.transform.LookAt(VRAvatar.VRigController.Instance.Rig.FeedbackerB.Hand.Root.position);
            }
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(Nailgun), nameof(Nailgun.Shoot))] static bool NailgunA(Nailgun __instance)
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
        [HarmonyPrefix] [HarmonyPatch(typeof(Nailgun), nameof(Nailgun.ShootMagnet))] static bool NailgunBA(Nailgun __instance)
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
        [HarmonyPrefix] [HarmonyPatch(typeof(Nailgun), nameof(Nailgun.SuperSaw))] static bool NailgunBB(Nailgun __instance)
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
        [HarmonyPrefix] [HarmonyPatch(typeof(Nailgun), nameof(Nailgun.ShootZapper))] static bool NailgunBC(Nailgun __instance)
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

        [HarmonyPrefix] [HarmonyPatch(typeof(Railcannon), nameof(Railcannon.Shoot))] static bool RailgunA(Railcannon __instance)
        {
            GameObject gameObject = Object.Instantiate<GameObject>(__instance.beam, Vars.DominantHand.transform.position, Vars.DominantHand.transform.rotation);
            if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
            {
                gameObject.transform.LookAt(__instance.targeter.CurrentTarget.bounds.center);
            }
            if (__instance.variation != 1)
            {
                RevolverBeam revolverBeam;
                if (gameObject.TryGetComponent<RevolverBeam>(out revolverBeam))
                {
                    revolverBeam.sourceWeapon = __instance.gc.currentWeapon;
                    revolverBeam.alternateStartPoint = __instance.shootPoint.position;
                }
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 250f, ForceMode.VelocityChange);
            }
            Object.Instantiate<GameObject>(__instance.fireSound);
            __instance.anim.SetTrigger("Shoot");
            MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.GunFireStrong);

            return false;
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.Shoot))] static bool RocketLauncherA(RocketLauncher __instance)
        {
            if (__instance.aud)
            {
                __instance.aud.pitch = Random.Range(0.9f, 1.1f);
                __instance.aud.Play();
            }
            if (__instance.variation == 1 && __instance.cbCharge > 0f)
            {
                __instance.chargeSound.Stop();
                __instance.cbCharge = 0f;
            }
            Object.Instantiate<GameObject>(__instance.muzzleFlash, __instance.shootPoint.position, Vars.DominantHand.transform.rotation);
            __instance.anim.SetTrigger("Fire");
            __instance.cooldown = __instance.rateOfFire;
            GameObject gameObject = Object.Instantiate<GameObject>(__instance.rocket, Vars.DominantHand.transform.position, __instance.transform.rotation);
            if (MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
            {
                gameObject.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
            }
            Grenade component = gameObject.GetComponent<Grenade>();
            if (component)
            {
                component.sourceWeapon = MonoSingleton<GunControl>.Instance.currentWeapon;
            }
            MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.GunFire, __instance.gameObject);

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.ShootCannonball))] static bool RocketLauncherBA(RocketLauncher __instance)
        {
            if (__instance.aud)
            {
                __instance.aud.pitch = Random.Range(0.6f, 0.8f);
                __instance.aud.Play();
            }
            Object.Instantiate<GameObject>(__instance.muzzleFlash, __instance.shootPoint.position, Vars.DominantHand.transform.rotation);
            __instance.anim.SetTrigger("Fire");
            __instance.cooldown = __instance.rateOfFire;
            Rigidbody rigidbody = Object.Instantiate<Rigidbody>(__instance.cannonBall, Vars.DominantHand.transform.position + __instance.transform.forward, __instance.transform.rotation);
            if (MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
            {
                rigidbody.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
            }
            rigidbody.velocity = rigidbody.transform.forward * Mathf.Max(15f, __instance.cbCharge * 150f);
            Cannonball cannonball;
            if (rigidbody.TryGetComponent<Cannonball>(out cannonball))
            {
                cannonball.sourceWeapon = MonoSingleton<GunControl>.Instance.currentWeapon;
            }
            __instance.cbCharge = 0f;

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.ShootNapalm))] static bool RocketLauncherBB(RocketLauncher __instance)
        {
            __instance.anim.SetTrigger("Spray");
            __instance.napalmProjectileCooldown = 0.02f;
            MonoSingleton<WeaponCharges>.Instance.rocketNapalmFuel -= 0.015f;
            Rigidbody rigidbody = Object.Instantiate(__instance.napalmProjectile, Vars.DominantHand.transform.position + __instance.transform.forward, __instance.transform.rotation);
            if ((bool)MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
            {
                rigidbody.transform.LookAt(MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center);
            }

            rigidbody.transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            rigidbody.velocity = rigidbody.transform.forward * 150f;

            return false;
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(RocketLauncher), nameof(RocketLauncher.Update))] static void RL_VRIK(RocketLauncher __instance)
        {
            if (VRAvatar.VRigController.Instance != null)
            {
                __instance.transform.position = VRAvatar.VRigController.Instance.Rig.FeedbackerB.Forearm.position;
                __instance.transform.LookAt(Vars.DominantHand.transform/*VRAvatar.VRigController.Instance.Rig.FeedbackerB.Hand.Root*/);
            }
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(Chainsaw), nameof(Chainsaw.Update))] static bool ChainsawA(Chainsaw __instance)
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


        [HarmonyPrefix] [HarmonyPatch(typeof(Washer), nameof(Washer.Update))] static bool WasherA(Washer __instance)
        {
            Transform transform = Vars.DominantHand.transform;
            if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, 50f, LayerMaskDefaults.Get(LMD.Environment)))
            {
                if (hitInfo.distance < 2.25f)
                {
                    __instance.transform.position = transform.position;
                    __instance.transform.rotation = transform.rotation;
                    __instance.correctCameraView.canModifyTarget = false;
                }
                else
                {
                    __instance.correctCameraView.canModifyTarget = true;
                }
            }

            if (MonoSingleton<GunControl>.Instance.activated && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (__instance.inputManager.InputSource.Fire1.IsPressed && !__instance.isSpraying)
                {
                    __instance.StartWashing();
                }
                else if (!__instance.inputManager.InputSource.Fire1.IsPressed && __instance.isSpraying)
                {
                    __instance.StopWashing();
                }

                if (__instance.inputManager.InputSource.Fire2.WasPerformedThisFrame)
                {
                    __instance.SwitchNozzle();
                }
            }

            float f = (float)((double)Time.time % 6.283185);
            __instance.aud.pitch = ((__instance.nozzleMode == 2) ? 2.1f : 1.1f) + Mathf.Sin(f) * 0.025f;
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(Vacuum), nameof(Vacuum.SuckObjects))] static bool VacuumSO(Vacuum __instance)
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
        [HarmonyPrefix] [HarmonyPatch(typeof(Vacuum), nameof(Vacuum.UpdateStuckObject))] static bool VacuumUSO(Vacuum __instance)
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
}

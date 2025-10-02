using HarmonyLib;
using Plugin.Data;
using Plugin.Systems.VRAvatar;
using UnityEngine;

namespace Plugin.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(ShotgunHammer))] internal class PatchShotgunHammer
{
    [HarmonyPrefix] [HarmonyPatch(nameof(ShotgunHammer.Impact))]
    static bool Impact(ShotgunHammer __instance)
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
    
    [HarmonyPrefix] [HarmonyPatch(nameof(ShotgunHammer.HitNade))]
    static bool HitNade(ShotgunHammer __instance)
    {
        if (Physics.Raycast(__instance.transform.position, __instance.direction, out var hitInfo, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment), QueryTriggerInteraction.Ignore))
            __instance.hitGrenade.GrenadeBeam(hitInfo.point);
        else
            __instance.hitGrenade.GrenadeBeam(__instance.transform.position + __instance.direction * 1000f);
        return false;
    }
    
    [HarmonyPrefix] [HarmonyPatch(nameof(ShotgunHammer.ImpactEffects))]
    static bool ImpactEffects(ShotgunHammer __instance)
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
    
    [HarmonyPrefix] [HarmonyPatch(nameof(ShotgunHammer.ThrowNade))]
    static bool ThrowNade(ShotgunHammer __instance)
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

    [HarmonyPrefix] [HarmonyPatch(nameof(ShotgunHammer.ShootSaw))]
    static bool ShootSaw(ShotgunHammer __instance)
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

    [HarmonyPrefix] [HarmonyPatch(nameof(ShotgunHammer.Update))]
    static bool Update(ShotgunHammer __instance)
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
            __instance.currentSpeed = Mathf.Min(__instance.currentSpeed, 0.5f);

        __instance.UpdateMeter();
        if ((float)__instance.pulledOut >= 0.5f)
            __instance.gunReady = true;

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
                __instance.Impact();
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
                        __instance.tempChargeSound.volume = 0f;
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
                Object.Destroy(__instance.tempChargeSound.gameObject);
        }

        if (__instance.variation == 2)
        {
            if (__instance.charging && __instance.chainsawBladeScroll.scrollSpeedX == 0f)
                __instance.chainsawBladeRenderer.material = __instance.chainsawBladeMotionMaterial;
            else if (!__instance.charging && __instance.chainsawBladeScroll.scrollSpeedX > 0f)
                __instance.chainsawBladeRenderer.material = __instance.chainsawBladeMaterial;

            __instance.chainsawBladeScroll.scrollSpeedX = __instance.chargeForce / 6f;
            __instance.anim.SetBool("Sawing", __instance.charging);
            __instance.sawZone.enabled = __instance.charging;
            if (__instance.charging && Physics.Raycast(Vars.DominantHand.transform.position, Vars.DominantHand.transform.transform.forward, out var hitInfo, 3f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
            {
                __instance.environmentalSawSpark.transform.position = hitInfo.point;
                if (!__instance.environmentalSawSpark.isEmitting)
                    __instance.environmentalSawSpark.Play();

                if (!__instance.environmentalSawSound.isPlaying)
                    __instance.environmentalSawSound.Play();
            }
            else
            {
                if (__instance.environmentalSawSpark.isEmitting) __instance.environmentalSawSpark.Stop();

                if (__instance.environmentalSawSound.isPlaying) __instance.environmentalSawSound.Stop();
            }
        }

        if (__instance.chargingSwing) __instance.swingCharge = Mathf.MoveTowards(__instance.swingCharge, 1f, Time.deltaTime * 2f);

        __instance.modelTransform.localPosition = new Vector3(__instance.defaultModelPosition.x + Random.Range((0f - __instance.swingCharge) / 30f, __instance.swingCharge / 30f), __instance.defaultModelPosition.y + Random.Range((0f - __instance.swingCharge) / 30f, __instance.swingCharge / 30f), __instance.defaultModelPosition.z + Random.Range((0f - __instance.swingCharge) / 30f, __instance.swingCharge / 30f));
        if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && __instance.variation != 2 && (__instance.variation == 1 || MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge >= 1f) && !__instance.aboutToSecondary && __instance.gunReady && __instance.gc.activated && !GameStateManager.Instance.PlayerInputLocked)
        {
            __instance.gunReady = false;
            if (!__instance.wid || __instance.wid.delay == 0f)
            {
                if (__instance.variation == 0) __instance.ThrowNade();
                else __instance.Pump();
            }
            else
            {
                __instance.aboutToSecondary = true;
                __instance.Invoke((__instance.variation == 0) ? "ThrowNade" : "Pump", __instance.wid.delay);
            }
        }

        if (__instance.secondaryMeterFill >= 1f) __instance.secondaryMeter.fillAmount = 1f;
        else if (__instance.secondaryMeterFill <= 0f) __instance.secondaryMeter.fillAmount = 0f;
        else __instance.secondaryMeter.fillAmount = Mathf.Lerp(0.275f, 0.625f, __instance.secondaryMeterFill);

        if (__instance.hammerCooldown > 0f) __instance.hammerCooldown = Mathf.MoveTowards(__instance.hammerCooldown, 0f, Time.deltaTime);

        if (MonoSingleton<WeaponCharges>.Instance.shoAltNadeCharge < 1f)
            __instance.nadeCharging = true;
        else if (__instance.nadeCharging)
        {
            __instance.nadeCharging = false;
            Object.Instantiate(__instance.nadeReadySound);
        }

        return false;
    }

    [HarmonyPostfix] [HarmonyPatch(nameof(ShotgunHammer.LateUpdate))]
    static void LateUpdate_AddIK(ShotgunHammer __instance)
    {
        if (VRigController.Instance != null)
        {
            __instance.transform.position = VRigController.Instance.Rig.FeedbackerB.Forearm.position;
            __instance.transform.LookAt(VRigController.Instance.Rig.FeedbackerB.Hand.Root.position);
        }
    }
}

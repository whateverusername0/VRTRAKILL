﻿using HarmonyLib;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Movement.Patches
{
    // change move vector to joystick axis, fix dash, jump, etc.
    [HarmonyPatch(typeof(NewMovement))] internal class NewMovementP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Start))] static void Start(NewMovement __instance)
        {
            __instance.walkSpeed *= Vars.Config.MovementMultiplier;
            __instance.jumpPower *= Vars.Config.MovementMultiplier;
            __instance.wallJumpPower *= Vars.Config.MovementMultiplier;
        }

        // why the fuck player physics are in Update()?
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Update))] static bool Update(NewMovement __instance)
        {
            Vector2 vector = Vector2.zero;
            if (__instance.activated)
            {
                vector = Input.InputVars.MoveVector;

                __instance.cc.movementHor = vector.x;
                __instance.cc.movementVer = vector.y;

                __instance.movementDirection = Vector3.ClampMagnitude(vector.x * __instance.transform.right + vector.y * __instance.transform.forward, 1f);

                if (__instance.punch == null) __instance.punch = __instance.GetComponentInChildren<FistControl>();
                else if (!__instance.punch.enabled) __instance.punch.YesFist();
            }
            else
            {
                __instance.rb.velocity = new Vector3(0f, __instance.rb.velocity.y, 0f);

                if (__instance.currentFallParticle != null) Object.Destroy(__instance.currentFallParticle);

                if (__instance.currentSlideParticle != null) Object.Destroy(__instance.currentSlideParticle);
                else if (__instance.slideScrape != null) Object.Destroy(__instance.slideScrape);

                if (__instance.punch == null) __instance.punch = __instance.GetComponentInChildren<FistControl>();

                else __instance.punch.NoFist();
            }

            if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad && __instance.gamepadFreezeCount > 0)
            {
                vector = Vector2.zero;
                __instance.rb.velocity = new Vector3(0f, __instance.rb.velocity.y, 0f);
                __instance.cc.movementHor = 0f;
                __instance.cc.movementVer = 0f;
                __instance.movementDirection = Vector3.zero;
                return false;
            }

            if (__instance.dead && !__instance.endlessMode)
            {
                __instance.currentAllPitch -= 0.1f * Time.deltaTime;
                MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allPitch", __instance.currentAllPitch);
                MonoSingleton<AudioMixerController>.Instance.doorSound.SetFloat("allPitch", __instance.currentAllPitch);
                if (__instance.blackColor.a < 0.5f)
                {
                    __instance.blackColor.a += 0.75f * Time.deltaTime;
                    __instance.youDiedColor.a += 0.75f * Time.deltaTime;
                }
                else
                {
                    __instance.blackColor.a += 0.05f * Time.deltaTime;
                    __instance.youDiedColor.a += 0.05f * Time.deltaTime;
                }

                __instance.blackScreen.color = __instance.blackColor;
                __instance.youDiedText.color = __instance.youDiedColor;
            }

            if (__instance.gc.onGround != __instance.pa.onGround)
                __instance.pa.onGround = __instance.gc.onGround;

            if (!__instance.gc.onGround)
            {
                if (__instance.fallTime < 1f)
                {
                    __instance.fallTime += Time.deltaTime * 5f;
                    if (__instance.fallTime > 1f) __instance.falling = true;
                }
                else if (__instance.rb.velocity.y < -2f) __instance.fallSpeed = __instance.rb.velocity.y;
            }
            else if (__instance.gc.onGround)
            {
                __instance.fallTime = 0f;
                __instance.clingFade = 0f;
            }

            if (!__instance.gc.onGround && __instance.rb.velocity.y < -20f)
            {
                __instance.aud3.pitch = __instance.rb.velocity.y * -1f / 120f;
                if (__instance.activated) __instance.aud3.volume = __instance.rb.velocity.y * -1f / 80f;
                else __instance.aud3.volume = __instance.rb.velocity.y * -1f / 240f;
            }
            else if (__instance.rb.velocity.y > -20f)
            {
                __instance.aud3.pitch = 0f;
                __instance.aud3.volume = 0f;
            }

            if (__instance.rb.velocity.y < -100f) __instance.rb.velocity = new Vector3(__instance.rb.velocity.x, -100f, __instance.rb.velocity.z);

            if (__instance.gc.onGround && __instance.falling && !__instance.jumpCooldown)
            {
                __instance.falling = false;
                __instance.slamStorage = false;
                if (__instance.fallSpeed > -50f)
                {
                    __instance.aud2.clip = __instance.landingSound;
                    __instance.aud2.volume = 0.5f + __instance.fallSpeed * -0.01f;
                    __instance.aud2.pitch = Random.Range(0.9f, 1.1f);
                    __instance.aud2.Play();
                }
                else
                {
                    Object.Instantiate(__instance.impactDust, __instance.gc.transform.position, Quaternion.identity).transform.forward = Vector3.up;
                    __instance.cc.CameraShake(0.5f);
                    MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.FallImpact);
                }

                __instance.fallSpeed = 0f; __instance.gc.heavyFall = false;
                if (__instance.currentFallParticle != null) Object.Destroy(__instance.currentFallParticle);
            }

            if (!__instance.gc.onGround && __instance.activated
            && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame
            && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (__instance.sliding) __instance.StopSlide();

                if (__instance.boost)
                {
                    __instance.boostLeft = 0f;
                    __instance.boost = false;
                }

                RaycastHit val = default(RaycastHit);
                if (__instance.fallTime > 0.5f
                    && !Physics.Raycast(__instance.gc.transform.position + __instance.transform.up,
                                        __instance.transform.up * -1f, out val, 3f,
                                        (int)__instance.lmask) && !__instance.gc.heavyFall)
                {
                    __instance.stillHolding = true;
                    __instance.rb.velocity = new Vector3(0f, -100f, 0f);
                    __instance.falling = true;
                    __instance.fallSpeed = -100f;
                    __instance.gc.heavyFall = true;
                    __instance.slamForce = 1f;
                    if (__instance.currentFallParticle != null)
                        Object.Destroy(__instance.currentFallParticle);

                    __instance.currentFallParticle = Object.Instantiate(__instance.fallParticle, __instance.transform);
                }
            }

            if (__instance.gc.heavyFall && !__instance.slamStorage)
                __instance.rb.velocity = new Vector3(0f, -100f, 0f);

            if (__instance.gc.heavyFall || __instance.sliding) Physics.IgnoreLayerCollision(2, 12, true);
            else Physics.IgnoreLayerCollision(2, 12, false);

            if (!__instance.slopeCheck.onGround
            && __instance.slopeCheck.forcedOff <= 0
            && !__instance.jumping && !__instance.boost)
            {
                float num = __instance.playerCollider.height / 2f - __instance.playerCollider.center.y;
                RaycastHit val2 = default(RaycastHit);
                if (__instance.rb.velocity != Vector3.zero
                    && Physics.Raycast(__instance.transform.position, __instance.transform.up * -1f, out val2, num + 1f, (int)__instance.lmask))
                {
                    Vector3 target = new Vector3(__instance.transform.position.x,
                                                 __instance.transform.position.y - ((RaycastHit)(val2)).distance + num,
                                                 __instance.transform.position.z);
                    __instance.transform.position = Vector3.MoveTowards(__instance.transform.position, target,
                                                                        ((RaycastHit)(val2)).distance * Time.deltaTime * 10f);
                    if (__instance.rb.velocity.y > 0f)
                        __instance.rb.velocity = new Vector3(__instance.rb.velocity.x, 0f, __instance.rb.velocity.z);
                }
            }

            if (__instance.gc.heavyFall)
            {
                __instance.slamForce += Time.deltaTime * 5f;
                RaycastHit val3 = default(RaycastHit);
                if (Physics.Raycast(__instance.gc.transform.position + __instance.transform.up, __instance.transform.up * -1f, out val3, 5f, (int)__instance.lmask)
                                    || Physics.SphereCast(__instance.gc.transform.position + __instance.transform.up, 1f, __instance.transform.up * -1f,
                                       out val3, 5f, (int)__instance.lmask))
                {
                    Breakable component = ((Component)(object)((RaycastHit)(val3)).collider).GetComponent<Breakable>();
                    if (component != null && component.weak && !component.precisionOnly)
                    {
                        Object.Instantiate(__instance.impactDust, ((RaycastHit)(val3)).point, Quaternion.identity);
                        component.Break();
                    }

                    if (((Component)(object)((RaycastHit)(val3)).collider).gameObject.TryGetComponent<Bleeder>(out var component2))
                        component2.GetHit(((RaycastHit)(val3)).point, GoreType.Head);

                    if (((RaycastHit)(val3)).transform.TryGetComponent<Idol>(out var component3)) component3.Death();
                }
            }

            if (__instance.stillHolding && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame) __instance.stillHolding = false;

            if (__instance.activated)
            {
                if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame
                    && (!__instance.falling || __instance.gc.canJump || __instance.wc.CheckForEnemyCols()) && !__instance.jumpCooldown)
                {
                    if (__instance.gc.canJump || __instance.wc.CheckForEnemyCols())
                    {
                        __instance.currentWallJumps = 0;
                        __instance.rocketJumps = 0;
                        __instance.clingFade = 0f;
                        __instance.rocketRides = 0;
                    }

                    __instance.Jump();
                }

                if (!__instance.gc.onGround && __instance.wc.onWall)
                {
                    RaycastHit val4 = default(RaycastHit);
                    if (Physics.Raycast(__instance.transform.position, __instance.movementDirection, out val4, 1f, (int)__instance.lmask))
                    {
                        if (__instance.rb.velocity.y < -1f && !__instance.gc.heavyFall)
                        {
                            __instance.rb.velocity = (new Vector3(Mathf.Clamp(__instance.rb.velocity.x, -1f, 1f), -2f * __instance.clingFade, Mathf.Clamp(__instance.rb.velocity.z, -1f, 1f)));
                            if (__instance.scrapeParticle == null)
                            {
                                __instance.scrapeParticle = Object.Instantiate(__instance.scrapePrefab, ((RaycastHit)(val4)).point, Quaternion.identity);
                            }

                            __instance.scrapeParticle.transform.position = new Vector3(((RaycastHit)(val4)).point.x, ((RaycastHit)(val4)).point.y + 1f, ((RaycastHit)(val4)).point.z);
                            __instance.scrapeParticle.transform.forward = ((RaycastHit)(val4)).normal;
                            __instance.clingFade = Mathf.MoveTowards(__instance.clingFade, 50f, Time.deltaTime * 4f);
                        }
                    }
                    else if (__instance.scrapeParticle != null)
                    {
                        Object.Destroy(__instance.scrapeParticle);
                        __instance.scrapeParticle = null;
                    }

                    if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame
                        && !__instance.jumpCooldown && __instance.currentWallJumps < 3 && (bool)__instance.wc && __instance.wc.CheckForCols())
                            __instance.WallJump();
                }
                else if (__instance.scrapeParticle != null)
                {
                    Object.Destroy(__instance.scrapeParticle);
                    __instance.scrapeParticle = null;
                }
            }
            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && __instance.gc.onGround && __instance.activated
            && (!__instance.slowMode || __instance.crouching) && !GameStateManager.Instance.PlayerInputLocked && !__instance.sliding)
                    __instance.StartSlide();

            RaycastHit val5 = default(RaycastHit);
            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && !__instance.gc.onGround && !__instance.sliding && !__instance.jumping
            && __instance.activated && !__instance.slowMode && !GameStateManager.Instance.PlayerInputLocked
            && Physics.Raycast(__instance.gc.transform.position + __instance.transform.up, __instance.transform.up * -1f, out val5, 2f, (int)__instance.lmask))
                    __instance.StartSlide();
            if ((MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame || (__instance.slowMode && !__instance.crouching)) && __instance.sliding) __instance.StopSlide();

            if (__instance.sliding && __instance.activated)
            {
                __instance.standing = false;
                __instance.slideLength += Time.deltaTime;
                if (__instance.cc.defaultPos.y != __instance.cc.originalPos.y - 0.625f)
                {
                    Vector3 vector2 = new Vector3(__instance.cc.originalPos.x, __instance.cc.originalPos.y - 0.625f, __instance.cc.originalPos.z);
                    __instance.cc.defaultPos = Vector3.MoveTowards(__instance.cc.defaultPos, vector2, ((__instance.cc.defaultPos - vector2).magnitude + 0.5f) * Time.deltaTime * 20f);
                }

                if (__instance.currentSlideParticle != null) __instance.currentSlideParticle.transform.position = __instance.transform.position + __instance.dodgeDirection * 10f;

                if (__instance.slideSafety > 0f) __instance.slideSafety -= Time.deltaTime * 5f;

                if (__instance.gc.onGround)
                {
                    __instance.slideScrape.transform.position = __instance.transform.position + __instance.dodgeDirection;
                    __instance.cc.CameraShake(0.1f);
                }
                else __instance.slideScrape.transform.position = Vector3.one * 5000f;

                if (__instance.rising)
                {
                    if (__instance.cc.defaultPos != __instance.cc.originalPos - Vector3.up * 0.625f)
                    {
                        __instance.cc.defaultPos =
                            Vector3.MoveTowards(__instance.cc.defaultPos, __instance.cc.originalPos,
                                                ((__instance.cc.originalPos - __instance.cc.defaultPos).magnitude + 0.5f) * Time.deltaTime * 10f);
                    }
                    else __instance.rising = false;
                }
            }
            else if ((bool)__instance.groundProperties && __instance.groundProperties.forceCrouch)
            {
                __instance.playerCollider.height = 1.25f;
                __instance.crouching = true;
                if (__instance.standing)
                {
                    __instance.standing = false;
                    __instance.transform.position = new Vector3(__instance.transform.position.x, __instance.transform.position.y - 1.125f, __instance.transform.position.z);
                    __instance.gc.transform.localPosition = __instance.groundCheckPos + Vector3.up * 1.125f;
                }

                if (__instance.cc.defaultPos != __instance.cc.originalPos - Vector3.up * 0.625f)
                {
                    __instance.cc.defaultPos = Vector3.MoveTowards(__instance.cc.defaultPos, __instance.cc.originalPos - Vector3.up * 0.625f,
                                                           ((__instance.cc.originalPos - Vector3.up * 0.625f - __instance.cc.defaultPos).magnitude + 0.5f) * Time.deltaTime * 10f);
                }
            }
            else
            {
                if (__instance.activated)
                {
                    if (!__instance.standing)
                    {
                        if ((bool)(Object)(object)__instance.playerCollider && __instance.playerCollider.height != 3.5f)
                        {
                            if (!Physics.Raycast(__instance.transform.position, Vector3.up, 2.25f, (int)__instance.lmask, (QueryTriggerInteraction)1)
                                && !Physics.SphereCast(new Ray(__instance.transform.position, Vector3.up), 0.5f, 2f, (int)__instance.lmask, (QueryTriggerInteraction)1))
                            {
                                __instance.playerCollider.height = 3.5f;
                                __instance.gc.transform.localPosition = __instance.groundCheckPos;
                                if (Physics.Raycast(__instance.transform.position, Vector3.up * -1f, 2.25f, (int)__instance.lmask, (QueryTriggerInteraction)1))
                                {
                                    __instance.transform.position = new Vector3(__instance.transform.position.x,
                                                                                __instance.transform.position.y + 1.125f,
                                                                                __instance.transform.position.z);
                                }
                                else
                                {
                                    __instance.transform.position = new Vector3(__instance.transform.position.x,
                                                                                __instance.transform.position.y - 0.625f,
                                                                                __instance.transform.position.z);
                                    __instance.cc.defaultPos = __instance.cc.originalPos;
                                    __instance.standing = true;
                                }

                                if (__instance.crouching)
                                {
                                    __instance.crouching = false;
                                    __instance.slowMode = false;
                                }
                            }
                            else
                            {
                                __instance.crouching = true;
                                __instance.slowMode = true;
                            }
                        }
                        else if (__instance.cc.defaultPos.y != __instance.cc.originalPos.y)
                            __instance.cc.defaultPos = Vector3.MoveTowards(__instance.cc.defaultPos, __instance.cc.originalPos,
                                                                   (__instance.cc.originalPos.y - __instance.cc.defaultPos.y + 0.5f) * Time.deltaTime * 10f);
                        else __instance.standing = true;
                    }
                    else if (__instance.rising)
                    {
                        if (__instance.cc.defaultPos != __instance.cc.originalPos)
                            __instance.cc.defaultPos = Vector3.MoveTowards(__instance.cc.defaultPos, __instance.cc.originalPos,
                                                                   ((__instance.cc.originalPos - __instance.cc.defaultPos).magnitude + 0.5f) * Time.deltaTime * 10f);
                        else __instance.rising = false;
                    }
                }

                if (__instance.currentSlideParticle != null) Object.Destroy(__instance.currentSlideParticle);

                if (__instance.slideScrape != null) Object.Destroy(__instance.slideScrape);
            }

            if (__instance.rising && Vector3.Distance(__instance.cc.defaultPos, __instance.cc.originalPos) > 10f)
            {
                __instance.rising = false;
                __instance.cc.defaultPos = __instance.cc.originalPos;
            }

            // Dash
            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame
            && __instance.activated && !__instance.slowMode && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (((bool)__instance.groundProperties && !__instance.groundProperties.canDash) || __instance.modNoDashSlide)
                    if (__instance.modNoDashSlide || !__instance.groundProperties.silentDashFail) Object.Instantiate(__instance.staminaFailSound);
                if (__instance.boostCharge >= 100f)
                {
                    if (__instance.sliding) __instance.StopSlide();

                    __instance.boostLeft = 100f;
                    __instance.boost = true;

                    __instance.dodgeDirection = __instance.movementDirection.normalized;
                    if (__instance.dodgeDirection == Vector3.zero)
                        __instance.dodgeDirection = __instance.transform.forward;

                    Quaternion identity = Quaternion.identity;
                    identity.SetLookRotation(__instance.dodgeDirection * -1f);
                    Object.Instantiate(__instance.dodgeParticle, __instance.transform.position + __instance.dodgeDirection * 10f, identity);

                    if (!__instance.asscon.majorEnabled || !__instance.asscon.infiniteStamina) __instance.boostCharge -= 100f;

                    if (__instance.dodgeDirection == __instance.transform.forward) __instance.cc.dodgeDirection = 0; // what the fuck, hakita?
                    else if (__instance.dodgeDirection == __instance.transform.forward * -1f) __instance.cc.dodgeDirection = 1;
                    else __instance.cc.dodgeDirection = 2;

                    __instance.aud.clip = __instance.dodgeSound;
                    __instance.aud.volume = 1f;
                    __instance.aud.pitch = 1f;
                    __instance.aud.Play();

                    MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Dash);
                    if (__instance.gc.heavyFall)
                    {
                        __instance.fallSpeed = 0f;
                        __instance.gc.heavyFall = false;
                        if (__instance.currentFallParticle != null)
                            Object.Destroy(__instance.currentFallParticle);
                    }
                }
                else Object.Instantiate(__instance.staminaFailSound);
            }

            if (!__instance.walking && vector.sqrMagnitude > 0f && !__instance.sliding && __instance.gc.onGround)
            {
                __instance.walking = true;
                __instance.anim.SetBool("WalkF", true);
            }
            else if ((__instance.walking && Mathf.Approximately(vector.sqrMagnitude, 0f)) || !__instance.gc.onGround || __instance.sliding)
            {
                __instance.walking = false;
                __instance.anim.SetBool("WalkF", false);
            }

            if (__instance.hurting && __instance.hp > 0)
            {
                __instance.currentColor.a -= Time.deltaTime;
                __instance.hurtScreen.color = __instance.currentColor;
                if (__instance.currentColor.a <= 0f) __instance.hurting = false;
            }

            if (__instance.boostCharge != 300f && !__instance.sliding && !__instance.slowMode)
            {
                float num2 = 1f;
                if (__instance.difficulty == 1) num2 = 1.5f;
                else if (__instance.difficulty == 0) num2 = 2f;

                __instance.boostCharge = Mathf.MoveTowards(__instance.boostCharge, 300f, 70f * Time.deltaTime * num2);
            }

            Vector3 vector3 = __instance.hudOriginalPos - __instance.cc.transform.InverseTransformDirection(__instance.rb.velocity) / 1000f;
            float num3 = Vector3.Distance(vector3, __instance.screenHud.transform.localPosition);
            __instance.screenHud.transform.localPosition = Vector3.MoveTowards(__instance.screenHud.transform.localPosition, vector3, Time.deltaTime * 15f * num3);

            int rankIndex = MonoSingleton<StyleHUD>.Instance.rankIndex;
            if (rankIndex == 7 || __instance.difficulty <= 1)
            {
                __instance.antiHp = 0f;
                __instance.antiHpCooldown = 0f;
            }
            else if (__instance.antiHpCooldown > 0f)
            {
                if (rankIndex >= 4) __instance.antiHpCooldown = Mathf.MoveTowards(__instance.antiHpCooldown, 0f, Time.deltaTime * (float)(rankIndex / 2));
                else __instance.antiHpCooldown = Mathf.MoveTowards(__instance.antiHpCooldown, 0f, Time.deltaTime);
            }
            else if (__instance.antiHp > 0f)
            {
                if (rankIndex >= 4) __instance.antiHp = Mathf.MoveTowards(__instance.antiHp, 0f, Time.deltaTime * (float)rankIndex * 10f);
                else __instance.antiHp = Mathf.MoveTowards(__instance.antiHp, 0f, Time.deltaTime * 15f);
            }

            if (!__instance.gc.heavyFall && __instance.currentFallParticle != null) Object.Destroy(__instance.currentFallParticle);

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Dodge))] static bool Dash(NewMovement __instance)
        {
            if (__instance.sliding)
            {
                if (!__instance.hurting && __instance.boostLeft <= 0f)
                {
                    __instance.gameObject.layer = 2;
                    __instance.exploded = false;
                }

                float num = 1f;
                if (__instance.preSlideSpeed > 1f)
                {
                    if (__instance.preSlideSpeed > 3f) __instance.preSlideSpeed = 3f;

                    num = __instance.preSlideSpeed;
                    if (__instance.gc.onGround)
                        __instance.preSlideSpeed -= Time.fixedDeltaTime * __instance.preSlideSpeed;

                    __instance.preSlideDelay = 0f;
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

                __instance.movementDirection = Vector3.ClampMagnitude(Input.InputVars.MoveVector.x * __instance.transform.right, 1f) * 5f
                                             * Vars.Config.MovementMultiplier;
                if (!MonoSingleton<HookArm>.Instance || !MonoSingleton<HookArm>.Instance.beingPulled)
                    __instance.rb.velocity = vector + __instance.pushForce + __instance.movementDirection.normalized;
                else __instance.StopSlide();

                return false;
            }

            float y = 0f;
            if (__instance.slideEnding) y = __instance.rb.velocity.y;

            float num2 = 2.75f;
            __instance.movementDirection2 = new Vector3(__instance.dodgeDirection.x * __instance.walkSpeed * Time.deltaTime * num2,
                                                        y,
                                                        __instance.dodgeDirection.z * __instance.walkSpeed * Time.deltaTime * num2);
            if (!__instance.slideEnding || (__instance.gc.onGround && !__instance.jumping))
                __instance.rb.velocity = __instance.movementDirection2 * 3f;

            __instance.gameObject.layer = 15;
            __instance.boostLeft -= 4f;
            if (__instance.boostLeft <= 0f)
            {
                __instance.boost = false;
                if (!__instance.gc.onGround && !__instance.slideEnding)
                    __instance.rb.velocity = __instance.movementDirection2;
            }

            __instance.slideEnding = false;

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.StartSlide))] static bool Slide(NewMovement __instance)
        {
            if (__instance.currentSlideParticle != null) Object.Destroy(__instance.currentSlideParticle);

            if (__instance.slideScrape != null) Object.Destroy(__instance.slideScrape);

            if (__instance.modNoDashSlide) __instance.StopSlide();
            else
            {
                if ((bool)MonoSingleton<HookArm>.Instance && MonoSingleton<HookArm>.Instance.beingPulled) return false;

                if ((bool)__instance.groundProperties && !__instance.groundProperties.canSlide)
                {
                    if (!__instance.groundProperties.silentSlideFail) __instance.StopSlide();
                    return false;
                }

                if (!__instance.crouching)
                {
                    __instance.playerCollider.height = 1.25f;
                    __instance.transform.position = new Vector3(__instance.transform.position.x,
                                                                __instance.transform.position.y - 1.125f,
                                                                __instance.transform.position.z);
                    __instance.gc.transform.localPosition = __instance.groundCheckPos + Vector3.up * 1.125f;
                }

                __instance.slideSafety = 1f;
                __instance.sliding = true;
                __instance.boost = true;
                __instance.dodgeDirection = __instance.movementDirection.normalized;
                if (__instance.dodgeDirection == Vector3.zero) __instance.dodgeDirection = __instance.transform.forward;

                Quaternion identity = Quaternion.identity;
                identity.SetLookRotation(__instance.dodgeDirection * -1f);
                __instance.currentSlideParticle = Object.Instantiate(__instance.slideParticle, __instance.transform.position + __instance.dodgeDirection * 10f, identity);
                __instance.slideScrape = Object.Instantiate(__instance.slideScrapePrefab, __instance.transform.position + __instance.dodgeDirection * 2f, identity);
                if (__instance.dodgeDirection == __instance.transform.forward) __instance.cc.dodgeDirection = 0;
                else if (__instance.dodgeDirection == __instance.transform.forward * -1f) __instance.cc.dodgeDirection = 1;
                else __instance.cc.dodgeDirection = 2;

                MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Slide);
            }
            return false;
        }

        // Dying thingamajig fix
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.GetHurt))] static bool GetHurt(int damage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, NewMovement __instance)
        {
            if (!__instance.dead && (!invincible || __instance.gameObject.layer != 15) && damage > 0)
            {
                if (explosion)
                {
                    __instance.exploded = true;
                }
                if (__instance.asscon.majorEnabled)
                {
                    damage = Mathf.RoundToInt((float)damage * __instance.asscon.damageTaken);
                }
                if (invincible)
                {
                    __instance.gameObject.layer = 15;
                }
                if (damage >= 50)
                {
                    __instance.currentColor.a = 0.8f;
                }
                else
                {
                    __instance.currentColor.a = 0.5f;
                }
                __instance.hurting = true;
                __instance.cc.CameraShake((float)(damage / 20));
                __instance.hurtAud.pitch = Random.Range(0.8f, 1f);
                __instance.hurtAud.PlayOneShot(__instance.hurtAud.clip);
                if (__instance.hp - damage > 0)
                {
                    __instance.hp -= damage;
                }
                else
                {
                    __instance.hp = 0;
                }
                if (invincible && scoreLossMultiplier != 0f && __instance.difficulty >= 2 && (!__instance.asscon.majorEnabled || !__instance.asscon.disableHardDamage) && __instance.hp <= 100)
                {
                    float num = 0.35f;
                    if (__instance.difficulty >= 4)
                    {
                        num = 0.5f;
                    }
                    if (__instance.antiHp + (float)damage * num < 99f)
                    {
                        __instance.antiHp += (float)damage * num;
                    }
                    else
                    {
                        __instance.antiHp = 99f;
                    }
                    if (__instance.antiHpCooldown == 0f)
                    {
                        __instance.antiHpCooldown += 1f;
                    }
                    if (__instance.difficulty >= 3)
                    {
                        __instance.antiHpCooldown += 1f;
                    }
                    __instance.antiHpFlash.Flash(1f);
                    __instance.antiHpCooldown += (float)(damage / 20);
                }
                if (__instance.shud == null)
                {
                    __instance.shud = MonoSingleton<StyleHUD>.Instance;
                }
                if (scoreLossMultiplier > 0.5f)
                {
                    __instance.shud.RemovePoints(0);
                    __instance.shud.DescendRank();
                }
                else
                {
                    __instance.shud.RemovePoints(Mathf.RoundToInt((float)damage));
                }
                StatsManager instance = MonoSingleton<StatsManager>.Instance;
                if (damage <= 200)
                {
                    instance.stylePoints -= Mathf.RoundToInt((float)(damage * 5) * scoreLossMultiplier);
                }
                else
                {
                    instance.stylePoints -= Mathf.RoundToInt(1000f * scoreLossMultiplier);
                }
                instance.tookDamage = true;
                if (__instance.hp == 0)
                {
                    if (!__instance.endlessMode)
                    {
                        __instance.blackScreen.gameObject.SetActive(true);
                        MonoSingleton<TimeController>.Instance.controlPitch = false;
                        if (instablack)
                        {
                            __instance.blackColor.a = 1f;
                        }
                        __instance.screenHud.SetActive(false);
                    }
                    else
                    {
                        __instance.GetComponentInChildren<FinalCyberRank>().GameOver();
                        CrowdReactions instance2 = MonoSingleton<CrowdReactions>.Instance;
                        if (instance2 != null)
                        {
                            instance2.React(instance2.aww);
                        }
                    }
                    //__instance.rb.constraints = RigidbodyConstraints.None;
                    if (MonoSingleton<PowerUpMeter>.Instance)
                    {
                        MonoSingleton<PowerUpMeter>.Instance.juice = 0f;
                    }
                    __instance.cc.enabled = false;
                    if (__instance.gunc == null)
                    {
                        __instance.gunc = __instance.GetComponentInChildren<GunControl>();
                    }
                    __instance.gunc.NoWeapon();
                    //__instance.rb.constraints = RigidbodyConstraints.None;
                    __instance.dead = true;
                    __instance.activated = false;
                    if (__instance.punch == null)
                    {
                        __instance.punch = __instance.GetComponentInChildren<FistControl>();
                    }
                    __instance.punch.NoFist();
                }
            }
            return false;
        }
    }
}

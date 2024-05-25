using HarmonyLib;
using ULTRAKILL.Cheats;
using UnityEngine;
using VRBasePlugin.ULTRAKILL.Input;

namespace VRBasePlugin.ULTRAKILL.Movement.Patches
{
    [HarmonyPatch(typeof(NewMovement))] internal sealed class NewMovementP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Start))] static void Start(NewMovement __instance)
        {
            __instance.walkSpeed *= Vars.Config.MovementMultiplier;
            __instance.jumpPower *= Vars.Config.MovementMultiplier;
            __instance.wallJumpPower *= Vars.Config.MovementMultiplier;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Update))] static bool Update(NewMovement __instance)
        {

            if (__instance.gc.onGround)
                __instance.CheckForGasoline();
            else if (__instance.oilSlideEffect.gameObject.activeSelf)
                __instance.oilSlideEffect.gameObject.SetActive(value: false);

            Vector2 vector = Vector2.zero;
            if (__instance.activated)
            {
                vector = InputVars.MoveVector;
                __instance.movementDirection = Vector3.ClampMagnitude(vector.x * __instance.transform.right + vector.y * __instance.transform.forward, 1f);
                if (__instance.punch == null)
                    __instance.punch = __instance.GetComponentInChildren<FistControl>();
                else if (!__instance.punch.enabled)
                    __instance.punch.YesFist();
            }
            else
            {
                if (__instance.currentFallParticle != null)
                    Object.Destroy(__instance.currentFallParticle);
                if (__instance.currentSlideParticle != null)
                    Object.Destroy(__instance.currentSlideParticle);
                else if (__instance.slideScrape != null)
                    Object.Destroy(__instance.slideScrape);

                if (__instance.punch == null)
                    __instance.punch = __instance.GetComponentInChildren<FistControl>();
                else __instance.punch.NoFist();
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
                else if (__instance.rb.velocity.y < -2f)
                    __instance.fallSpeed = __instance.rb.velocity.y;
            }
            else if (__instance.gc.onGround)
            {
                __instance.fallTime = 0f;
                __instance.clingFade = 0f;
            }

            if (!__instance.gc.onGround && __instance.rb.velocity.y < -20f)
            {
                __instance.aud3.pitch = __instance.rb.velocity.y * -1f / 120f;
                if (__instance.activated)
                    __instance.aud3.volume = __instance.rb.velocity.y * -1f / 80f;
                else __instance.aud3.volume = __instance.rb.velocity.y * -1f / 240f;
            }
            else if (__instance.rb.velocity.y > -20f)
            {
                __instance.aud3.pitch = 0f;
                __instance.aud3.volume = 0f;
            }

            if (__instance.rb.velocity.y < -100f)
                __instance.rb.velocity = new Vector3(__instance.rb.velocity.x, -100f, __instance.rb.velocity.z);

            if (__instance.gc.onGround && __instance.falling && !__instance.jumpCooldown)
            {
                __instance.falling = false;
                __instance.slamStorage = false;
                if (__instance.fallSpeed > -50f)
                {
                    __instance.aud2.clip = __instance.landingSound;
                    __instance.aud2.volume = 0.5f + __instance.fallSpeed * -0.01f;
                    __instance.aud2.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    __instance.aud2.Play();
                }
                else
                {
                    Object.Instantiate(__instance.impactDust, __instance.gc.transform.position, Quaternion.identity).transform.forward = Vector3.up;
                    __instance.cc.CameraShake(0.5f);
                    MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.FallImpact);
                }

                __instance.fallSpeed = 0f;
                __instance.gc.heavyFall = false;
                if (__instance.currentFallParticle != null)
                    Object.Destroy(__instance.currentFallParticle);
            }

            if (!__instance.gc.onGround && __instance.activated
            && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame
            && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (__instance.sliding)
                    __instance.StopSlide();

                if (__instance.boost)
                {
                    __instance.boostLeft = 0f;
                    __instance.boost = false;
                }

                if (__instance.fallTime > 0.5f
                && !Physics.Raycast(__instance.gc.transform.position + __instance.transform.up, __instance.transform.up * -1f, out var _, 3f, __instance.lmask) && !__instance.gc.heavyFall)
                {
                    __instance.stillHolding = true;
                    __instance.rb.velocity = new Vector3(0f, -100f, 0f);
                    __instance.falling = true;
                    __instance.fallSpeed = -100f;
                    __instance.gc.heavyFall = true;
                    __instance.slamForce = 1f;
                    if (__instance.currentFallParticle != null)
                    {
                        Object.Destroy(__instance.currentFallParticle);
                    }

                    __instance.currentFallParticle = Object.Instantiate(__instance.fallParticle, __instance.transform);
                }
            }

            if (__instance.gc.heavyFall && !__instance.slamStorage)
                __instance.rb.velocity = new Vector3(0f, -100f, 0f);

            if (__instance.gc.heavyFall || __instance.sliding)
                Physics.IgnoreLayerCollision(2, 12, ignore: true);
            else
                Physics.IgnoreLayerCollision(2, 12, ignore: false);

            if (!__instance.slopeCheck.onGround && __instance.slopeCheck.forcedOff <= 0 && __instance.modForcedFrictionMultip != 0f && !__instance.jumping && !__instance.boost)
            {
                float num = __instance.playerCollider.height / 2f - __instance.playerCollider.center.y;
                if (__instance.rb.velocity != Vector3.zero && Physics.Raycast(__instance.transform.position, __instance.transform.up * -1f, out var hitInfo2, num + 1f, __instance.lmask, QueryTriggerInteraction.Ignore))
                {
                    Vector3 target = new Vector3(__instance.transform.position.x, __instance.transform.position.y - hitInfo2.distance + num, __instance.transform.position.z);
                    __instance.transform.position = Vector3.MoveTowards(__instance.transform.position, target, hitInfo2.distance * Time.deltaTime * 10f);
                    if (__instance.rb.velocity.y > 0f)
                        __instance.rb.velocity = new Vector3(__instance.rb.velocity.x, 0f, __instance.rb.velocity.z);
                }
            }

            if (__instance.gc.heavyFall)
            {
                __instance.slamForce += Time.deltaTime * 5f;
                if (Physics.Raycast(__instance.gc.transform.position + __instance.transform.up, __instance.transform.up * -1f, out var hitInfo3, 5f, __instance.lmask)
                || Physics.SphereCast(__instance.gc.transform.position + __instance.transform.up, 1f, __instance.transform.up * -1f, out hitInfo3, 5f, __instance.lmask))
                {
                    Breakable component = hitInfo3.collider.GetComponent<Breakable>();
                    if (component != null && (component.weak || component.forceGroundSlammable) && !component.precisionOnly && !component.unbreakable)
                    {
                        Object.Instantiate(__instance.impactDust, hitInfo3.point, Quaternion.identity);
                        component.Break();
                    }

                    if (hitInfo3.collider.gameObject.TryGetComponent<Bleeder>(out var component2))
                    {
                        component2.GetHit(hitInfo3.point, GoreType.Head);
                    }

                    if (hitInfo3.transform.TryGetComponent<Idol>(out var component3))
                    {
                        component3.Death();
                    }
                }
            }

            if (__instance.stillHolding && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame)
                __instance.stillHolding = false;

            if (__instance.activated)
            {
                if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame && (!__instance.falling || __instance.gc.canJump || __instance.wc.CheckForEnemyCols()) && !__instance.jumpCooldown)
                {
                    if (__instance.gc.canJump || __instance.wc.CheckForEnemyCols())
                    {
                        __instance.currentWallJumps = 0;
                        __instance.rocketJumps = 0;
                        __instance.hammerJumps = 0;
                        __instance.clingFade = 0f;
                        __instance.rocketRides = 0;
                    }

                    __instance.Jump();
                }

                if (!__instance.gc.onGround && __instance.wc.onWall)
                {
                    if (!__instance.sliding && Physics.Raycast(__instance.transform.position, __instance.movementDirection, out var hitInfo4, 1f, __instance.lmask))
                    {
                        if (__instance.rb.velocity.y < -1f && !__instance.gc.heavyFall)
                        {
                            __instance.rb.velocity = new Vector3(Mathf.Clamp(__instance.rb.velocity.x, -1f, 1f), -2f * __instance.clingFade, Mathf.Clamp(__instance.rb.velocity.z, -1f, 1f));
                            if (__instance.scrapeParticle == null)
                                __instance.scrapeParticle = Object.Instantiate(__instance.scrapePrefab, hitInfo4.point, Quaternion.identity);

                            __instance.scrapeParticle.transform.position = new Vector3(hitInfo4.point.x, hitInfo4.point.y + 1f, hitInfo4.point.z);
                            __instance.scrapeParticle.transform.forward = hitInfo4.normal;
                            __instance.clingFade = Mathf.MoveTowards(__instance.clingFade, 50f, Time.deltaTime * 4f);
                        }
                    }
                    else if (__instance.scrapeParticle != null)
                    {
                        Object.Destroy(__instance.scrapeParticle);
                        __instance.scrapeParticle = null;
                    }

                    if (!GameStateManager.Instance.PlayerInputLocked
                    && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame
                    && !__instance.jumpCooldown && __instance.currentWallJumps < 3
                    && (bool)__instance.wc && __instance.wc.CheckForCols())
                        __instance.WallJump();
                }
                else if (__instance.scrapeParticle != null)
                {
                    Object.Destroy(__instance.scrapeParticle);
                    __instance.scrapeParticle = null;
                }
            }

            if (!GameStateManager.Instance.PlayerInputLocked && !GameStateManager.Instance.IsStateActive("alter-menu"))
            {
                if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame
                && (__instance.gc.onGround || (float)__instance.gc.sinceLastGrounded < 0.03f) && __instance.activated
                && (!__instance.slowMode || __instance.crouching) && !GameStateManager.Instance.PlayerInputLocked && !__instance.sliding)
                    __instance.StartSlide();

                if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame
                && !__instance.gc.onGround && !__instance.sliding && !__instance.jumping && __instance.activated && !__instance.slowMode
                && !GameStateManager.Instance.PlayerInputLocked
                && Physics.Raycast(__instance.gc.transform.position + __instance.transform.up, __instance.transform.up * -1f, out var _, 2f, __instance.lmask, QueryTriggerInteraction.Ignore))
                    __instance.StartSlide();
            }

            if ((MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame
            || (__instance.slowMode && !__instance.crouching)) && __instance.sliding)
                __instance.StopSlide();

            if (__instance.sliding && __instance.activated)
            {
                __instance.standing = false;
                __instance.slideLength += Time.deltaTime;

                Vector3 normalized = Vector3.ProjectOnPlane(__instance.rb.velocity.normalized, Vector3.up).normalized;
                if (__instance.currentSlideParticle != null)
                {
                    __instance.currentSlideParticle.transform.position = __instance.transform.position + normalized * 10f;
                    __instance.currentSlideParticle.transform.forward = -__instance.dodgeDirection;
                }

                if (__instance.slideSafety > 0f)
                    __instance.slideSafety -= Time.deltaTime * 5f;

                if (__instance.gc.onGround || __instance.wc.onWall)
                {
                    __instance.slideScrape.transform.position = __instance.transform.position + normalized;
                    __instance.slideScrape.transform.forward = -normalized;
                    __instance.cc.CameraShake(0.1f);
                }
                else
                    __instance.slideScrape.transform.position = Vector3.one * 5000f;
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
            }
            else
            {
                if (__instance.activated)
                {
                    if ((bool)__instance.playerCollider && __instance.playerCollider.height != 3.5f)
                    {
                        if (!Physics.Raycast(__instance.transform.position, Vector3.up, 2.25f, __instance.lmask, QueryTriggerInteraction.Ignore)
                        && !Physics.SphereCast(new Ray(__instance.transform.position, Vector3.up), 0.5f, 2f, __instance.lmask, QueryTriggerInteraction.Ignore))
                        {
                            __instance.playerCollider.height = 3.5f;
                            __instance.gc.transform.localPosition = __instance.groundCheckPos;
                            if (Physics.Raycast(__instance.transform.position, Vector3.up * -1f, 2.25f, __instance.lmask, QueryTriggerInteraction.Ignore))
                                __instance.transform.position = new Vector3(__instance.transform.position.x, __instance.transform.position.y + 1.125f, __instance.transform.position.z);
                            else
                            {
                                __instance.transform.position = new Vector3(__instance.transform.position.x, __instance.transform.position.y - 0.625f, __instance.transform.position.z);
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
                    else
                        __instance.standing = true;
                }

                if (__instance.currentSlideParticle != null)
                    Object.Destroy(__instance.currentSlideParticle);

                if (__instance.slideScrape != null)
                    Object.Destroy(__instance.slideScrape);
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame && __instance.activated && !__instance.slowMode && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (((bool)__instance.groundProperties && !__instance.groundProperties.canDash) || __instance.modNoDashSlide)
                {
                    if (__instance.modNoDashSlide || !__instance.groundProperties.silentDashFail)
                        Object.Instantiate(__instance.staminaFailSound);
                }
                else if (__instance.boostCharge >= 100f)
                {
                    if (__instance.sliding)
                        __instance.StopSlide();

                    __instance.boostLeft = 100f;
                    __instance.dashStorage = 1f;
                    __instance.boost = true;
                    __instance.dodgeDirection = __instance.movementDirection.normalized;
                    if (__instance.dodgeDirection == Vector3.zero)
                        __instance.dodgeDirection = __instance.transform.forward.normalized;

                    Quaternion identity = Quaternion.identity;
                    identity.SetLookRotation(__instance.dodgeDirection * -1f);
                    Object.Instantiate(__instance.dodgeParticle, __instance.transform.position + __instance.dodgeDirection * 10f, identity);
                    if (!__instance.asscon.majorEnabled || !__instance.asscon.infiniteStamina)
                        __instance.boostCharge -= 100f;

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
                else
                    Object.Instantiate(__instance.staminaFailSound);
            }

            if (!__instance.walking && vector.sqrMagnitude > 0f && !__instance.sliding && __instance.gc.onGround)
            {
                __instance.walking = true;
                __instance.anim.SetBool("WalkF", value: true);
            }
            else if ((__instance.walking && Mathf.Approximately(vector.sqrMagnitude, 0f)) || !__instance.gc.onGround || __instance.sliding)
            {
                __instance.walking = false;
                __instance.anim.SetBool("WalkF", value: false);
            }

            if (__instance.hurting && __instance.hp > 0)
            {
                __instance.currentColor.a -= Time.deltaTime;
                __instance.hurtScreen.color = __instance.currentColor;
                if (__instance.currentColor.a <= 0f)
                    __instance.hurting = false;
            }

            if (__instance.safeExplosionLaunchCooldown > 0f)
                __instance.safeExplosionLaunchCooldown = Mathf.MoveTowards(__instance.safeExplosionLaunchCooldown, 0f, Time.deltaTime);

            if (__instance.boostCharge != 300f && !__instance.sliding && !__instance.slowMode)
            {
                float num2 = 1f;
                if (__instance.difficulty == 1) num2 = 1.5f;
                else if (__instance.difficulty == 0) num2 = 2f;
                __instance.boostCharge = Mathf.MoveTowards(__instance.boostCharge, 300f, 70f * Time.deltaTime * num2);
            }

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

            if (!__instance.gc.heavyFall && __instance.currentFallParticle != null)
                Object.Destroy(__instance.currentFallParticle);

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Dodge))] static bool Dodge(NewMovement __instance)
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
                    if (__instance.preSlideSpeed > 3f)
                        __instance.preSlideSpeed = 3f;

                    num = __instance.preSlideSpeed;
                    if (__instance.gc.onGround && __instance.friction != 0f)
                        __instance.preSlideSpeed -= Time.fixedDeltaTime * __instance.preSlideSpeed * __instance.friction;

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

                Vector3 vector = new Vector3(__instance.dodgeDirection.x * __instance.walkSpeed * Time.deltaTime * 4f * num, __instance.rb.velocity.y, __instance.dodgeDirection.z * __instance.walkSpeed * Time.deltaTime * 4f * num);
                if ((bool)__instance.groundProperties && __instance.groundProperties.push)
                {
                    Vector3 vector2 = __instance.groundProperties.pushForce;
                    if (__instance.groundProperties.pushDirectionRelative)
                        vector2 = __instance.groundProperties.transform.rotation * vector2;

                    vector += vector2;
                }

                if (__instance.boostLeft > 0f)
                {
                    __instance.dashStorage = Mathf.MoveTowards(__instance.dashStorage, 0f, Time.fixedDeltaTime);
                    if (__instance.dashStorage <= 0f) __instance.boostLeft = 0f;
                }

                __instance.movementDirection = Vector3.ClampMagnitude(InputVars.MoveVector.x * __instance.transform.right, 1f) * 5f;
                if (!MonoSingleton<HookArm>.Instance || !MonoSingleton<HookArm>.Instance.beingPulled)
                    __instance.rb.velocity = vector + __instance.pushForce + __instance.movementDirection;
                else __instance.StopSlide();

                return false;
            }

            float y = 0f;
            if (__instance.slideEnding)
                y = __instance.rb.velocity.y;

            float num2 = 2.75f;
            __instance.movementDirection2 = new Vector3(__instance.dodgeDirection.x * __instance.walkSpeed * Time.deltaTime * num2, y, __instance.dodgeDirection.z * __instance.walkSpeed * Time.deltaTime * num2);
            __instance.gameObject.layer = 15;
            if (__instance.slideEnding)
            {
                __instance.slideEnding = false;
                if (!__instance.gc.onGround || __instance.friction == 0f)
                {
                    __instance.boost = false;
                    return false;
                }
            }

            if (__instance.boostLeft > 0f)
            {
                __instance.rb.velocity = __instance.movementDirection2 * 3f;
                __instance.boostLeft -= 4f;
                return false;
            }

            if (!__instance.gc.onGround || __instance.friction != 0f)
                __instance.rb.velocity = __instance.movementDirection2;

            __instance.boost = false;
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Launch))] static bool Launch(NewMovement __instance, Vector3 direction, float multiplier = 8f, bool ignoreMass = false)
        {
            if (((bool)__instance.groundProperties && !__instance.groundProperties.launchable) || (direction == Vector3.down && __instance.gc.onGround))
                return false;

            __instance.jumping = true;
            __instance.Invoke("NotJumping", 0.5f);
            __instance.jumpCooldown = true;
            __instance.Invoke("JumpReady", 0.2f);
            __instance.boost = false;
            if (__instance.gc.heavyFall)
            {
                __instance.fallSpeed = 0f;
                __instance.gc.heavyFall = false;
                if (__instance.currentFallParticle != null)
                   Object.Destroy(__instance.currentFallParticle);
            }

            if (direction.magnitude > 0f)
                __instance.rb.velocity = Vector3.zero;

            __instance.rb.AddForce(Vector3.ClampMagnitude(direction, 1000f) * multiplier, (!ignoreMass) ? ForceMode.Impulse : ForceMode.VelocityChange);
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.LaunchFromPoint))] static bool LaunchFromPoint(NewMovement __instance, Vector3 position, float strength, float maxDistance = 1)
        {
            if (!__instance.groundProperties || __instance.groundProperties.launchable)
            {
                Vector3 vector = (__instance.transform.position - position).normalized;
                if (position == __instance.transform.position)
                {
                    vector = Vector3.up;
                }

                Vector3 direction;
                if (__instance.jumping)
                {
                    direction = vector * maxDistance * strength;
                    direction.y = 0.5f * maxDistance * strength;
                }
                else
                {
                    float num = maxDistance - Vector3.Distance(__instance.transform.position, position);
                    direction = vector * num * strength;
                    direction.y = 0.5f * num * strength;
                }

                __instance.Launch(direction);
            }

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.GetHurt))] static bool GetHurt(NewMovement __instance, int damage, bool invincible, float scoreLossMultiplier = 1f, bool explosion = false, bool instablack = false, float hardDamageMultiplier = 0.35f, bool ignoreInvincibility = false)
        {
            if (__instance.dead || !(!invincible || __instance.gameObject.layer != 15 || ignoreInvincibility) || damage <= 0)
            {
                return false;
            }

            if (explosion)
            {
                __instance.exploded = true;
            }

            if (__instance.asscon.majorEnabled)
            {
                damage = Mathf.RoundToInt((float)damage * __instance.asscon.damageTaken);
            }

            if (Invincibility.Enabled)
            {
                damage = 0;
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
            __instance.cc.CameraShake(damage / 20);
            __instance.hurtAud.pitch = UnityEngine.Random.Range(0.8f, 1f);
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
                if (__instance.antiHp + (float)damage * hardDamageMultiplier < 99f)
                {
                    __instance.antiHp += (float)damage * hardDamageMultiplier;
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
                __instance.antiHpCooldown += damage / 20;
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
                __instance.shud.RemovePoints(Mathf.RoundToInt(damage));
            }

            StatsManager statsManager = MonoSingleton<StatsManager>.Instance;
            if (damage <= 200)
            {
                statsManager.stylePoints -= Mathf.RoundToInt((float)(damage * 5) * scoreLossMultiplier);
            }
            else
            {
                statsManager.stylePoints -= Mathf.RoundToInt(1000f * scoreLossMultiplier);
            }

            statsManager.tookDamage = true;
            if (__instance.hp != 0)
            {
                return false;
            }

            if (!__instance.endlessMode)
            {
                __instance.blackScreen.gameObject.SetActive(value: true);
                MonoSingleton<TimeController>.Instance.controlPitch = false;
                if (instablack)
                {
                    __instance.blackColor.a = 1f;
                }

                __instance.screenHud.SetActive(value: false);
            }
            else
            {
                __instance.GetComponentInChildren<FinalCyberRank>().GameOver();
                CrowdReactions crowdReactions = MonoSingleton<CrowdReactions>.Instance;
                if (crowdReactions != null)
                {
                    crowdReactions.React(crowdReactions.aww);
                }
            }

            //rb.constraints = RigidbodyConstraints.None;
            if ((bool)MonoSingleton<PowerUpMeter>.Instance)
            {
                MonoSingleton<PowerUpMeter>.Instance.juice = 0f;
            }

            __instance.cc.enabled = false;
            if (__instance.gunc == null)
            {
                __instance.gunc = __instance.GetComponentInChildren<GunControl>();
            }

            __instance.gunc.NoWeapon();
            //rb.constraints = RigidbodyConstraints.None;
            __instance.dead = true;
            __instance.activated = false;
            if (__instance.punch == null)
            {
                __instance.punch = __instance.GetComponentInChildren<FistControl>();
            }

            __instance.punch.NoFist();
            return false;
        }
    }
}

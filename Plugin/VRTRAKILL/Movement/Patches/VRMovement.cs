using HarmonyLib;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Plugin.VRTRAKILL.Movement.Patches
{
    internal class VRMovement
    {
        // fucking hell this shit took more time than any other thing in here combined
        // great job, hakita. nice shitnames. cool private fields.
        // this is why we can't have nice things
        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Update")] static bool UpdateMovement
        #region Args that i'm ashamed of
            (
            NewMovement __instance,
            ref bool ___modNoDashSlide, ref bool ___modNoJump, ref float ___modForcedFrictionMultip, ref InputManager ___inman,
            ref AssistController ___asscon, ref float ___walkSpeed, ref float ___jumpPower, ref float ___airAcceleration,
            ref float ___wallJumpPower, ref bool ___jumpCooldown, ref bool ___falling, ref Rigidbody ___rb, ref Vector3 ___movementDirection,
            ref Vector3 ___movementDirection2, ref Vector3 ___airDirection, ref float ___timeBetweenSteps, ref float ___stepTime,
            ref int ___currentStep, ref Animator ___anim, ref Quaternion ___tempRotation, ref GameObject ___forwardPoint,
            ref GroundCheck ___gc, ref GroundCheck ___slopeCheck, ref WallCheck ___wc, ref PlayerAnimations ___pa, ref Vector3 ___wallJumpPos,
            ref int ___currentWallJumps, ref AudioSource ___aud, ref AudioSource ___aud2, ref AudioSource ___aud3, ref int ___currentSound,
            ref AudioClip ___jumpSound, ref AudioClip ___landingSound, ref AudioClip ___finalWallJump, ref bool ___walking, ref int ___hp,
            ref float ___antiHp, ref float ___antiHpCooldown, ref Image ___hurtScreen, ref AudioSource ___hurtAud, ref Color ___hurtColor,
            ref Color ___currentColor, ref bool ___hurting, ref bool ___dead, ref bool ___endlessMode,
            ref Image ___blackScreen, ref Color ___blackColor, ref Text ___youDiedText, ref Color ___youDiedColor, ref FlashImage ___hpFlash,
            ref FlashImage ___antiHpFlash, ref AudioSource ___greenHpAud, ref float ___currentAllPitch, ref float ___currentAllVolume,
            ref bool ___boost, ref Vector3 ___dodgeDirection, ref float ___boostLeft, ref float ___boostCharge, ref AudioClip ___dodgeSound,
            ref CameraController ___cc, ref GameObject ___staminaFailSound, ref GameObject ___screenHud, ref Vector3 ___hudOriginalPos,
            ref GameObject ___dodgeParticle, ref GameObject ___scrnBlood, ref Canvas ___fullHud, ref GameObject ___hudCam,
            ref Vector3 ___camOriginalPos, ref RigidbodyConstraints ___defaultRBConstraints, ref GameObject ___revolver, ref StyleHUD ___shud,
            ref GameObject ___scrapePrefab, ref GameObject ___scrapeParticle, ref LayerMask ___lmask, ref StyleCalculator ___scalc,
            ref bool ___activated, ref int ___gamepadFreezeCount, ref float ___fallSpeed, ref bool ___jumping, ref float ___fallTime,
            ref GameObject ___impactDust, ref GameObject ___fallParticle, ref GameObject ___currentFallParticle, ref CapsuleCollider ___playerCollider,
            ref bool ___sliding, ref float ___slideSafety, ref GameObject ___slideParticle, ref GameObject ___currentSlideParticle,
            ref GameObject ___slideScrapePrefab, ref GameObject ___slideScrape, ref Vector3 ___slideMovDirection, ref GameObject ___slideStopSound,
            ref bool ___crouching, ref bool ___standing, ref bool ___rising, ref bool ___slideEnding, ref Vector3 ___groundCheckPos,
            ref GunControl ___gunc, ref float ___currentSpeed, ref FistControl ___punch, ref GameObject ___dashJumpSound, ref bool ___slowMode,
            ref Vector3 ___pushForce, ref float ___slideLength, ref float ___longestSlide, ref float ___preSlideSpeed, ref float ___preSlideDelay,
            ref bool ___quakeJump, ref GameObject ___quakeJumpSound, ref bool ___exploded, ref float ___clingFade, ref bool ___stillHolding,
            ref float ___slamForce, ref bool ___slamStorage, ref bool ___launched, ref int ___difficulty, ref int ___sameCheckpointRestarts,
            ref CustomGroundProperties ___groundProperties, ref int ___rocketJumps, ref Grenade ___ridingRocket, ref int ___rocketRides
            )
        #endregion
        {
            Vector2 vector = Vector2.zero;
            if (___activated)
            {
                vector = VRInputManager.MoveVector;

                ___cc.movementHor = vector.x;
                ___cc.movementVer = vector.y;

                ___movementDirection = Vector3.ClampMagnitude(vector.x * __instance.transform.right + vector.y * __instance.transform.forward, 1f);

                if (___punch == null)
                    ___punch = __instance.GetComponentInChildren<FistControl>();
                else if (!___punch.enabled) ___punch.YesFist();
            }
            else
            {
                ___rb.velocity = new Vector3(0f, ___rb.velocity.y, 0f);

                if (___currentFallParticle != null) Object.Destroy(___currentFallParticle);

                if (___currentSlideParticle != null) Object.Destroy(___currentSlideParticle);
                else if (___slideScrape != null) Object.Destroy(___slideScrape);

                if (___punch == null)
                    ___punch = __instance.GetComponentInChildren<FistControl>();

                else
                    ___punch.NoFist();
            }

            if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad && ___gamepadFreezeCount > 0)
            {
                vector = Vector2.zero;
                ___rb.velocity = new Vector3(0f, ___rb.velocity.y, 0f);
                ___cc.movementHor = 0f;
                ___cc.movementVer = 0f;
                ___movementDirection = Vector3.zero;
                return false;
            }

            if (___dead && !___endlessMode)
            {
                ___currentAllPitch -= 0.1f * Time.deltaTime;
                MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allPitch", ___currentAllPitch);
                MonoSingleton<AudioMixerController>.Instance.doorSound.SetFloat("allPitch", ___currentAllPitch);
                if (___blackColor.a < 0.5f)
                {
                    ___blackColor.a += 0.75f * Time.deltaTime;
                    ___youDiedColor.a += 0.75f * Time.deltaTime;
                }
                else
                {
                    ___blackColor.a += 0.05f * Time.deltaTime;
                    ___youDiedColor.a += 0.05f * Time.deltaTime;
                }

                ___blackScreen.color = ___blackColor;
                ___youDiedText.color = ___youDiedColor;
            }

            if (___gc.onGround != ___pa.onGround)
                ___pa.onGround = ___gc.onGround;

            if (!___gc.onGround)
            {
                if (___fallTime < 1f)
                {
                    ___fallTime += Time.deltaTime * 5f;
                    if (___fallTime > 1f) ___falling = true;
                }
                else if (___rb.velocity.y < -2f)
                    ___fallSpeed = ___rb.velocity.y;
            }
            else if (___gc.onGround)
            {
                ___fallTime = 0f;
                ___clingFade = 0f;
            }

            if (!___gc.onGround && ___rb.velocity.y < -20f)
            {
                ___aud3.pitch = ___rb.velocity.y * -1f / 120f;
                if (___activated) ___aud3.volume = ___rb.velocity.y * -1f / 80f;
                else ___aud3.volume = ___rb.velocity.y * -1f / 240f;
            }
            else if (__instance.rb.velocity.y > -20f)
            {
                ___aud3.pitch = 0f;
                ___aud3.volume = 0f;
            }

            if (___rb.velocity.y < -100f) ___rb.velocity = new Vector3(___rb.velocity.x, -100f, ___rb.velocity.z);

            if (___gc.onGround && ___falling && !___jumpCooldown)
            {
                ___falling = false;
                ___slamStorage = false;
                if (___fallSpeed > -50f)
                {
                    ___aud2.clip = ___landingSound;
                    ___aud2.volume = 0.5f + ___fallSpeed * -0.01f;
                    ___aud2.pitch = Random.Range(0.9f, 1.1f);
                    ___aud2.Play();
                }
                else
                {
                    Object.Instantiate(___impactDust, ___gc.transform.position, Quaternion.identity).transform.forward = Vector3.up;
                    ___cc.CameraShake(0.5f);
                    MonoSingleton<RumbleManager>.Instance.SetVibration("rumble.fall_impact");
                }

                ___fallSpeed = 0f; ___gc.heavyFall = false;
                if (___currentFallParticle != null) Object.Destroy(___currentFallParticle);
            }

            if (!___gc.onGround && ___activated
                && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame
                && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (___sliding) __instance.StopSlide();

                if (___boost)
                {
                    ___boostLeft = 0f;
                    ___boost = false;
                }

                RaycastHit val = default(RaycastHit);
                if (___fallTime > 0.5f
                    && !Physics.Raycast(___gc.transform.position + __instance.transform.up,
                                        __instance.transform.up * -1f, out val, 3f,
                                        (int)__instance.lmask) && !___gc.heavyFall)
                {
                    ___stillHolding = true;
                    ___rb.velocity = new Vector3(0f, -100f, 0f);
                    ___falling = true;
                    ___fallSpeed = -100f;
                    ___gc.heavyFall = true;
                    ___slamForce = 1f;
                    if (___currentFallParticle != null)
                        Object.Destroy(___currentFallParticle);

                    ___currentFallParticle = Object.Instantiate(___fallParticle, __instance.transform);
                }
            }

            if (___gc.heavyFall && !___slamStorage)
                ___rb.velocity = new Vector3(0f, -100f, 0f);

            if (___gc.heavyFall || ___sliding) Physics.IgnoreLayerCollision(2, 12, true);
            else Physics.IgnoreLayerCollision(2, 12, false);

            if (!___slopeCheck.onGround
                && ___slopeCheck.forcedOff <= 0
                && !___jumping && !___boost)
            {
                float num = ___playerCollider.height / 2f - ___playerCollider.center.y;
                RaycastHit val2 = default(RaycastHit);
                if (___rb.velocity != Vector3.zero
                    && Physics.Raycast(__instance.transform.position, __instance.transform.up * -1f, out val2, num + 1f, (int)___lmask))
                {
                    Vector3 target = new Vector3(__instance.transform.position.x,
                                                 __instance.transform.position.y - ((RaycastHit)(val2)).distance + num,
                                                 __instance.transform.position.z);
                    __instance.transform.position = Vector3.MoveTowards(__instance.transform.position, target,
                                                                        ((RaycastHit)(val2)).distance * Time.deltaTime * 10f);
                    if (___rb.velocity.y > 0f)
                        __instance.rb.velocity = new Vector3(__instance.rb.velocity.x, 0f, __instance.rb.velocity.z);
                }
            }

            if (___gc.heavyFall)
            {
                ___slamForce += Time.deltaTime * 5f;
                RaycastHit val3 = default(RaycastHit);
                if (Physics.Raycast(___gc.transform.position + __instance.transform.up, __instance.transform.up * -1f, out val3, 5f, (int)___lmask)
                                    || Physics.SphereCast(___gc.transform.position + __instance.transform.up, 1f, __instance.transform.up * -1f,
                                       out val3, 5f, (int)___lmask))
                {
                    Breakable component = ((Component)(object)((RaycastHit)(val3)).collider).GetComponent<Breakable>();
                    if (component != null && component.weak && !component.precisionOnly)
                    {
                        Object.Instantiate(___impactDust, ((RaycastHit)(val3)).point, Quaternion.identity);
                        component.Break();
                    }

                    if (((Component)(object)((RaycastHit)(val3)).collider).gameObject.TryGetComponent<Bleeder>(out var component2))
                        component2.GetHit(((RaycastHit)(val3)).point, GoreType.Head);

                    if (((RaycastHit)(val3)).transform.TryGetComponent<Idol>(out var component3)) component3.Death();
                }
            }

            if (___stillHolding && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame) ___stillHolding = false;

            if (___activated)
            {
                if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame
                    && (!___falling || ___gc.canJump || ___wc.CheckForEnemyCols()) && !___jumpCooldown)
                {
                    if (___gc.canJump || ___wc.CheckForEnemyCols())
                    {
                        ___currentWallJumps = 0;
                        ___rocketJumps = 0;
                        ___clingFade = 0f;
                        ___rocketRides = 0;
                    }

                    __instance.Jump();
                }

                if (!___gc.onGround && ___wc.onWall)
                {
                    RaycastHit val4 = default(RaycastHit);
                    if (Physics.Raycast(__instance.transform.position, ___movementDirection, out val4, 1f, (int)___lmask))
                    {
                        if (___rb.velocity.y < -1f && !___gc.heavyFall)
                        {
                            ___rb.velocity = (new Vector3(Mathf.Clamp(___rb.velocity.x, -1f, 1f), -2f * ___clingFade, Mathf.Clamp(___rb.velocity.z, -1f, 1f)));
                            if (___scrapeParticle == null)
                            {
                                ___scrapeParticle = Object.Instantiate(___scrapePrefab, ((RaycastHit)(val4)).point, Quaternion.identity);
                            }

                            ___scrapeParticle.transform.position = new Vector3(((RaycastHit)(val4)).point.x, ((RaycastHit)(val4)).point.y + 1f, ((RaycastHit)(val4)).point.z);
                            ___scrapeParticle.transform.forward = ((RaycastHit)(val4)).normal;
                            ___clingFade = Mathf.MoveTowards(___clingFade, 50f, Time.deltaTime * 4f);
                        }
                    }
                    else if (___scrapeParticle != null)
                    {
                        Object.Destroy(___scrapeParticle);
                        ___scrapeParticle = null;
                    }

                    if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame
                        && !___jumpCooldown && ___currentWallJumps < 3 && (bool)___wc && ___wc.CheckForCols())
                        Traverse.Create(__instance).Method("WallJump").GetValue();
                }
                else if (___scrapeParticle != null)
                {
                    Object.Destroy(___scrapeParticle);
                    ___scrapeParticle = null;
                }
            }
            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && ___gc.onGround && ___activated
                && (!___slowMode || ___crouching) && !GameStateManager.Instance.PlayerInputLocked && !___sliding)
                Traverse.Create(__instance).Method("StartSlide").GetValue();

            RaycastHit val5 = default(RaycastHit);
            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && !___gc.onGround && !___sliding && !___jumping
                && ___activated && !___slowMode && !GameStateManager.Instance.PlayerInputLocked
                && Physics.Raycast(___gc.transform.position + __instance.transform.up, __instance.transform.up * -1f, out val5, 2f, (int)___lmask))
                Traverse.Create(__instance).Method("StartSlide").GetValue();
            if ((MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame || (___slowMode && !___crouching)) && ___sliding) __instance.StopSlide();

            if (___sliding && ___activated)
            {
                ___standing = false;
                ___slideLength += Time.deltaTime;
                if (___cc.defaultPos.y != ___cc.originalPos.y - 0.625f)
                {
                    Vector3 vector2 = new Vector3(___cc.originalPos.x, ___cc.originalPos.y - 0.625f, ___cc.originalPos.z);
                    ___cc.defaultPos = Vector3.MoveTowards(___cc.defaultPos, vector2, ((___cc.defaultPos - vector2).magnitude + 0.5f) * Time.deltaTime * 20f);
                }

                if (___currentSlideParticle != null) ___currentSlideParticle.transform.position = __instance.transform.position + ___dodgeDirection * 10f;

                if (___slideSafety > 0f) ___slideSafety -= Time.deltaTime * 5f;

                if (___gc.onGround)
                {
                    ___slideScrape.transform.position = __instance.transform.position + ___dodgeDirection;
                    ___cc.CameraShake(0.1f);
                }
                else ___slideScrape.transform.position = Vector3.one * 5000f;

                if (___rising)
                {
                    if (___cc.defaultPos != ___cc.originalPos - Vector3.up * 0.625f)
                    {
                        ___cc.defaultPos = Vector3.MoveTowards(___cc.defaultPos, ___cc.originalPos, 
                                                               ((___cc.originalPos - ___cc.defaultPos).magnitude + 0.5f) * Time.deltaTime * 10f);
                    }
                    else ___rising = false;
                }
            }
            else if ((bool)___groundProperties && ___groundProperties.forceCrouch)
            {
                ___playerCollider.height = 1.25f;
                ___crouching = true;
                if (___standing)
                {
                    ___standing = false;
                    __instance.transform.position = new Vector3(__instance.transform.position.x, __instance.transform.position.y - 1.125f, __instance.transform.position.z);
                    ___gc.transform.localPosition = ___groundCheckPos + Vector3.up * 1.125f;
                }

                if (___cc.defaultPos != ___cc.originalPos - Vector3.up * 0.625f)
                {
                    ___cc.defaultPos = Vector3.MoveTowards(___cc.defaultPos, ___cc.originalPos - Vector3.up * 0.625f,
                                                           ((___cc.originalPos - Vector3.up * 0.625f - ___cc.defaultPos).magnitude + 0.5f) * Time.deltaTime * 10f);
                }
            }
            else
            {
                if (___activated)
                {
                    if (!___standing)
                    {
                        if ((bool)(Object)(object)___playerCollider && ___playerCollider.height != 3.5f)
                        {
                            if (!Physics.Raycast(__instance.transform.position, Vector3.up, 2.25f, (int)___lmask, (QueryTriggerInteraction)1)
                                && !Physics.SphereCast(new Ray(__instance.transform.position, Vector3.up), 0.5f, 2f, (int)___lmask, (QueryTriggerInteraction)1))
                            {
                                ___playerCollider.height = 3.5f;
                                ___gc.transform.localPosition = ___groundCheckPos;
                                if (Physics.Raycast(__instance.transform.position, Vector3.up * -1f, 2.25f, (int)___lmask, (QueryTriggerInteraction)1))
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
                                    ___cc.defaultPos = ___cc.originalPos;
                                    ___standing = true;
                                }

                                if (___crouching)
                                {
                                    ___crouching = false;
                                    ___slowMode = false;
                                }
                            }
                            else
                            {
                                ___crouching = true;
                                ___slowMode = true;
                            }
                        }
                        else if (___cc.defaultPos.y != ___cc.originalPos.y)
                            ___cc.defaultPos = Vector3.MoveTowards(___cc.defaultPos, ___cc.originalPos,
                                                                   (___cc.originalPos.y - ___cc.defaultPos.y + 0.5f) * Time.deltaTime * 10f);
                        else ___standing = true;
                    }
                    else if (___rising)
                    {
                        if (___cc.defaultPos != ___cc.originalPos)
                            ___cc.defaultPos = Vector3.MoveTowards(___cc.defaultPos, ___cc.originalPos,
                                                                   ((___cc.originalPos - ___cc.defaultPos).magnitude + 0.5f) * Time.deltaTime * 10f);
                        else ___rising = false;
                    }
                }

                if (___currentSlideParticle != null) Object.Destroy(___currentSlideParticle); 

                if (___slideScrape != null) Object.Destroy(___slideScrape);
            }

            if (___rising && Vector3.Distance(___cc.defaultPos, ___cc.originalPos) > 10f)
            {
                ___rising = false;
                ___cc.defaultPos = ___cc.originalPos;
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame
                && ___activated && !___slowMode && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (((bool)___groundProperties && !___groundProperties.canDash) || ___modNoDashSlide)
                    if (___modNoDashSlide || !___groundProperties.silentDashFail)
                        Object.Instantiate(___staminaFailSound);
                else if (___boostCharge >= 100f)
                {
                    if (___sliding) __instance.StopSlide();

                    ___boostLeft = 100f;
                    ___boost = true;
                    ___dodgeDirection = ___movementDirection;
                    if (___dodgeDirection == Vector3.zero) ___dodgeDirection = __instance.transform.forward;

                    Quaternion identity = Quaternion.identity;
                    identity.SetLookRotation(___dodgeDirection * -1f);
                    Object.Instantiate(___dodgeParticle, __instance.transform.position + ___dodgeDirection * 10f, identity);
                    if (!___asscon.majorEnabled || !___asscon.infiniteStamina)
                        ___boostCharge -= 100f;

                    if (___dodgeDirection == __instance.transform.forward) ___cc.dodgeDirection = 0;
                    else if (___dodgeDirection == __instance.transform.forward * -1f) ___cc.dodgeDirection = 1;
                    else ___cc.dodgeDirection = 2;

                    ___aud.clip = ___dodgeSound;
                    ___aud.volume = 1f;
                    ___aud.pitch = 1f;
                    ___aud.Play();
                    MonoSingleton<RumbleManager>.Instance.SetVibration("rumble.dash");
                    if (___gc.heavyFall)
                    {
                        ___fallSpeed = 0f;
                        ___gc.heavyFall = false;
                        if (___currentFallParticle != null)
                                Object.Destroy(___currentFallParticle);
                    }
                }
                else Object.Instantiate(___staminaFailSound);
            }

            if (!___walking && vector.sqrMagnitude > 0f && !___sliding && ___gc.onGround)
            {
                ___walking = true;
                ___anim.SetBool("WalkF", true);
            }
            else if ((___walking && Mathf.Approximately(vector.sqrMagnitude, 0f)) || !___gc.onGround || ___sliding)
            {
                ___walking = false;
                ___anim.SetBool("WalkF", false);
            }

            if (___hurting && ___hp > 0)
            {
                ___currentColor.a -= Time.deltaTime;
                ___hurtScreen.color = ___currentColor;
                if (___currentColor.a <= 0f) ___hurting = false;
            }

            if (___boostCharge != 300f && !___sliding && !___slowMode)
            {
                float num2 = 1f;
                if (___difficulty == 1) num2 = 1.5f; 
                else if (___difficulty == 0) num2 = 2f;

                ___boostCharge = Mathf.MoveTowards(___boostCharge, 300f, 70f * Time.deltaTime * num2);
            }

            Vector3 vector3 = ___hudOriginalPos - ___cc.transform.InverseTransformDirection(___rb.velocity) / 1000f;
            float num3 = Vector3.Distance(vector3, ___screenHud.transform.localPosition);
            ___screenHud.transform.localPosition = Vector3.MoveTowards(___screenHud.transform.localPosition, vector3, Time.deltaTime * 15f * num3);
            Vector3 vector4 = Vector3.ClampMagnitude(___camOriginalPos - ___cc.transform.InverseTransformDirection(___rb.velocity) / 350f * -1f, 0.2f);
            float num4 = Vector3.Distance(vector4, ___hudCam.transform.localPosition);
            ___hudCam.transform.localPosition = Vector3.MoveTowards(___hudCam.transform.localPosition, vector4, Time.deltaTime * 25f * num4);
            int rankIndex = MonoSingleton<StyleHUD>.Instance.rankIndex;
            if (rankIndex == 7 || ___difficulty <= 1)
            {
                ___antiHp = 0f;
                ___antiHpCooldown = 0f;
            }
            else if (___antiHpCooldown > 0f)
            {
                if (rankIndex >= 4) ___antiHpCooldown = Mathf.MoveTowards(___antiHpCooldown, 0f, Time.deltaTime * (float)(rankIndex / 2));
                else ___antiHpCooldown = Mathf.MoveTowards(___antiHpCooldown, 0f, Time.deltaTime);
            }
            else if (___antiHp > 0f)
            {
                if (rankIndex >= 4) ___antiHp = Mathf.MoveTowards(___antiHp, 0f, Time.deltaTime * (float)rankIndex * 10f);
                else ___antiHp = Mathf.MoveTowards(___antiHp, 0f, Time.deltaTime * 15f);
            }

            if (!___gc.heavyFall && ___currentFallParticle != null) Object.Destroy(___currentFallParticle);

            return false;
        }
    }
}

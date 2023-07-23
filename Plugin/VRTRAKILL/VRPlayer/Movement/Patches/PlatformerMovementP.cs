using HarmonyLib;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Movement.Patches
{
    // change move vector to joystick axis, fix dash, jump, etc.
    [HarmonyPatch(typeof(PlatformerMovement))] internal static class PlatformerMovementP
    {
        // change movement vector to vr one
        [HarmonyPrefix] [HarmonyPatch(nameof(PlatformerMovement.Update))] static bool Update(PlatformerMovement __instance)
        {
            if (MonoSingleton<OptionsManager>.Instance.paused) return false;

            Vector2 vector = Vector2.zero;
            if (__instance.activated)
            {
                vector = Input.InputVars.MoveVector * Vars.Config.Game.MovementMultiplier;
                __instance.movementDirection = Vector3.ClampMagnitude(vector.x * Vector3.right + vector.y * Vector3.forward, 1f);
                __instance.movementDirection = Quaternion.Euler(0f, __instance.platformerCamera.rotation.eulerAngles.y, 0f) * __instance.movementDirection;
            }
            else
            {
                __instance.rb.velocity = new Vector3(0f, __instance.rb.velocity.y, 0f);
                __instance.movementDirection = Vector3.zero;
            }

            if (__instance.movementDirection.magnitude > 0f) __instance.anim.SetBool("Running", true);
            else __instance.anim.SetBool("Running", false);

            if (__instance.rb.velocity.y < -100f)
                __instance.rb.velocity = new Vector3(__instance.rb.velocity.x, -100f, __instance.rb.velocity.z);

            if (__instance.activated && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame
                && !__instance.falling && !__instance.jumpCooldown) __instance.Jump(false, 1f);

            if (!__instance.groundCheck.onGround)
            {
                if (__instance.fallTime < 1f)
                {
                    __instance.fallTime += Time.deltaTime * 5f;
                    if (__instance.fallTime > 1f) __instance.falling = true;
                }
                else if (__instance.rb.velocity.y < -2f) __instance.fallSpeed = __instance.rb.velocity.y;
            }
            else __instance.fallTime = 0f;

            if (__instance.groundCheck.onGround && __instance.falling && !__instance.jumpCooldown)
            {
                __instance.falling = false;
                __instance.fallSpeed = 0f;
                __instance.groundCheck.heavyFall = false;
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && __instance.groundCheck.onGround
                && __instance.activated && !__instance.sliding) __instance.StartSlide();

            RaycastHit raycastHit;
            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame
                && !__instance.groundCheck.onGround && !__instance.sliding && !__instance.jumping && __instance.activated
                && Physics.Raycast(__instance.groundCheck.transform.position + __instance.transform.up, __instance.transform.up * -1f, out raycastHit, 2f,
                LayerMaskDefaults.Get(LMD.Environment))) __instance.StartSlide();

            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame && __instance.sliding)
                __instance.StopSlide();

            if (__instance.sliding && __instance.activated)
            {
                __instance.slideLength += Time.deltaTime;
                if (__instance.currentSlideEffect != null) __instance.currentSlideEffect.transform.position = __instance.transform.position + __instance.dodgeDirection * 10f;
                if (__instance.slideSafety > 0f) __instance.slideSafety -= Time.deltaTime * 5f;
                if (__instance.groundCheck.onGround) __instance.currentSlideScrape.transform.position = __instance.transform.position + __instance.dodgeDirection;
                else __instance.currentSlideScrape.transform.position = Vector3.one * 5000f;
            }

            // Dash fix
            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame && __instance.activated)
            {
                if (__instance.groundProperties && !__instance.groundProperties.canDash)
                    if (!__instance.groundProperties.silentDashFail) Object.Instantiate<GameObject>(__instance.staminaFailSound);
                /* else */ if (__instance.boostCharge >= 100f)
                {
                    if (__instance.sliding) __instance.StopSlide();
                    __instance.boostLeft = 100f;
                    __instance.boost = true;
                    __instance.anim.Play("Dash", -1, 0f);

                    __instance.dodgeDirection = __instance.movementDirection * Vars.Config.Game.MovementMultiplier;
                    if (__instance.dodgeDirection == Vector3.zero)
                        __instance.dodgeDirection = __instance.playerModel.forward * Vars.Config.Game.MovementMultiplier;

                    Quaternion identity = Quaternion.identity;
                    identity.SetLookRotation(__instance.dodgeDirection * -1f);
                    Object.Instantiate<GameObject>
                            (__instance.dodgeParticle, __instance.transform.position + Vector3.up * 2f + __instance.dodgeDirection * 10f, identity).transform.localScale *= 2f;
                    if (!MonoSingleton<AssistController>.Instance.majorEnabled || !MonoSingleton<AssistController>.Instance.infiniteStamina)
                        __instance.boostCharge -= 100f;

                    __instance.aud.clip = __instance.dodgeSound;
                    __instance.aud.volume = 1f;
                    __instance.aud.pitch = 1f;
                    __instance.aud.Play();
                }
                else Object.Instantiate<GameObject>(__instance.staminaFailSound);
            }

            if (__instance.boostCharge != 300f && !__instance.sliding && !__instance.spinning)
            {
                float num = 1f;
                if (__instance.difficulty == 1) num = 1.5f;
                else if (__instance.difficulty == 0) num = 2f;

                __instance.boostCharge = Mathf.MoveTowards(__instance.boostCharge, 300f, 70f * Time.deltaTime * num);
            }

            if (__instance.spinCooldown > 0f) __instance.spinCooldown = Mathf.MoveTowards(__instance.spinCooldown, 0f, Time.deltaTime);

            if (__instance.activated && !__instance.spinning && __instance.spinCooldown <= 0f && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo()
                && (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame
                || MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame
                || MonoSingleton<InputManager>.Instance.InputSource.Punch.WasPerformedThisFrame)
                && !MonoSingleton<OptionsManager>.Instance.paused) __instance.Spin();

            if (__instance.spinning) __instance.playerModel.Rotate(Vector3.up, Time.deltaTime * 3600f, Space.Self);
            else if (__instance.movementDirection.magnitude != 0f || __instance.boost)
            {
                Quaternion quaternion = Quaternion.LookRotation(__instance.movementDirection);
                if (__instance.boost)
                    quaternion = Quaternion.LookRotation(__instance.dodgeDirection);
                __instance.playerModel.rotation =
                    Quaternion.RotateTowards(__instance.playerModel.rotation, quaternion,
                    (Quaternion.Angle(__instance.playerModel.rotation, quaternion) + 20f) * 35f * __instance.movementDirection.magnitude * Time.deltaTime);
            }

            if (__instance.cameraTrack)
            {
                if (!__instance.freeCamera)
                {
                    __instance.CheckCameraTarget(false);
                    __instance.platformerCamera.transform.position =
                        Vector3.MoveTowards(__instance.platformerCamera.position, __instance.transform.position + __instance.cameraTarget,
                        Time.deltaTime * 15f * (0.1f + Vector3.Distance(__instance.platformerCamera.position, __instance.cameraTarget)));

                    __instance.platformerCamera.transform.rotation =
                        Quaternion.RotateTowards(__instance.platformerCamera.transform.rotation,
                        Quaternion.Euler(__instance.cameraRotation), Time.deltaTime * 15f * (0.1f + Vector3.Distance(__instance.platformerCamera.rotation.eulerAngles,
                        __instance.cameraRotation)));
                }
                else if (!MonoSingleton<OptionsManager>.Instance.paused)
                {
                    __instance.platformerCamera.transform.position = __instance.transform.position + __instance.defaultCameraTarget;
                    __instance.platformerCamera.transform.rotation = Quaternion.Euler(__instance.defaultCameraRotation);

                    Vector2 vector2 = MonoSingleton<InputManager>.Instance.InputSource.Look.ReadValue<Vector2>();
                    if (!MonoSingleton<CameraController>.Instance.reverseY) __instance.rotationX += vector2.y * (MonoSingleton<OptionsManager>.Instance.mouseSensitivity / 10f);
                    else __instance.rotationX -= vector2.y * (MonoSingleton<OptionsManager>.Instance.mouseSensitivity / 10f);

                    if (!MonoSingleton<CameraController>.Instance.reverseX) __instance.rotationY += vector2.x * (MonoSingleton<OptionsManager>.Instance.mouseSensitivity / 10f);
                    else __instance.rotationY -= vector2.x * (MonoSingleton<OptionsManager>.Instance.mouseSensitivity / 10f);

                    if (__instance.rotationY > 180f) __instance.rotationY -= 360f;
                    else if (__instance.rotationY < -180f) __instance.rotationY += 360f;

                    __instance.rotationX = Mathf.Clamp(__instance.rotationX, -69f, 109f);

                    float num2 = 2.5f;
                    if (__instance.sliding || Physics.Raycast(__instance.transform.position + Vector3.up * 0.625f, Vector3.up, 2.5f, LayerMaskDefaults.Get(LMD.Environment)))
                        num2 = 0.625f;

                    Vector3 vector3 = __instance.transform.position + Vector3.up * num2;
                    __instance.platformerCamera.RotateAround(vector3, Vector3.left, __instance.rotationX);
                    __instance.platformerCamera.RotateAround(vector3, Vector3.up, __instance.rotationY);

                    RaycastHit raycastHit2;
                    if (Physics.SphereCast(vector3, 0.25f, __instance.platformerCamera.position - vector3, out raycastHit2,
                        Vector3.Distance(vector3, __instance.platformerCamera.position), LayerMaskDefaults.Get(LMD.Environment)))
                        __instance.platformerCamera.position = raycastHit2.point + 0.5f * raycastHit2.normal;
                }
            }

            RaycastHit raycastHit3;
            if (Physics.SphereCast(__instance.transform.position + Vector3.up, 0.5f, Vector3.down, out raycastHit3, float.PositiveInfinity,
                LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
            {
                __instance.jumpShadow.position = raycastHit3.point + Vector3.up * 0.05f;
                __instance.jumpShadow.forward = raycastHit3.normal;
            }
            else
            {
                __instance.jumpShadow.position = __instance.transform.position - Vector3.up * 1000f;
                __instance.jumpShadow.forward = Vector3.up;
            }

            if (__instance.coinTimer > 0f) __instance.coinTimer = Mathf.MoveTowards(__instance.coinTimer, 0f, Time.deltaTime);
            if (__instance.coinEffectTimer > 0f) __instance.coinEffectTimer = Mathf.MoveTowards(__instance.coinEffectTimer, 0f, Time.deltaTime);
            else if (__instance.queuedCoins > 0) __instance.CoinGetEffect();

            if (__instance.invincible && __instance.extraHits < 3)
            {
                if (__instance.blinkTimer > 0f)
                    __instance.blinkTimer = Mathf.MoveTowards(__instance.blinkTimer, 0f, Time.deltaTime);
                else
                {
                    __instance.blinkTimer = 0.05f;
                    if (__instance.playerModel.gameObject.activeSelf) __instance.playerModel.gameObject.SetActive(false);
                    else __instance.playerModel.gameObject.SetActive(true);
                }
            }

            if (__instance.superTimer > 0f)
            {
                if (!NoWeaponCooldown.NoCooldown) __instance.superTimer = Mathf.MoveTowards(__instance.superTimer, 0f, Time.deltaTime);
                if (__instance.superTimer == 0f) __instance.GetHit();
            }

            return false;
        }
    }
}

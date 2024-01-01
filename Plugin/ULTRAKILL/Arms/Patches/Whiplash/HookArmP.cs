using HarmonyLib;
using UnityEngine;
using VRBasePlugin.ULTRAKILL.VRAvatar.Armature;
using static UnityEngine.Random;
using Valve.VR.InteractionSystem;

namespace VRBasePlugin.ULTRAKILL.Arms.Patches.Whiplash
{
    // you get it? cameracontrollerpatches?? ccp????? lol!!
    [HarmonyPatch(typeof(HookArm))] internal sealed class HookArmP
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(HookArm.Start))] static void ConvertWhiplash(HookArm __instance)
        {
            Arm A = Arm.WhiplashPreset(__instance.transform);
            ArmTransformer AR = __instance.gameObject.AddComponent<ArmTransformer>();
            ArmController.DefaultArmCon AC = __instance.gameObject.AddComponent<ArmController.DefaultArmCon>();
            AR.Arm = A; AC.Arm = A;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(HookArm.Update))] static bool Update(HookArm __instance)
        {
            if (!MonoSingleton<OptionsManager>.Instance || MonoSingleton<OptionsManager>.Instance.paused) return false;
            if (!__instance.equipped || MonoSingleton<FistControl>.Instance.shopping || !MonoSingleton<FistControl>.Instance.activated)
            {
                if (__instance.state != HookState.Ready || __instance.returning) __instance.Cancel();
                __instance.model.SetActive(false);
                return false;
            }
            if (MonoSingleton<InputManager>.Instance.InputSource.Hook.WasPerformedThisFrame)
            {
                if (__instance.state == HookState.Pulling) __instance.StopThrow(0f, false); 
                else if (__instance.cooldown <= 0f)
                {
                    __instance.cooldown = 0.5f;
                    __instance.model.SetActive(true);
                    if (!__instance.forcingFistControl)
                    {
                        if (MonoSingleton<FistControl>.Instance.currentPunch) MonoSingleton<FistControl>.Instance.currentPunch.CancelAttack(); 
                        MonoSingleton<FistControl>.Instance.forceNoHold++;
                        __instance.forcingFistControl = true;
                        MonoSingleton<FistControl>.Instance.transform.localRotation = Quaternion.identity;
                    }
                    __instance.lr.enabled = true;
                    __instance.hookPoint = Vars.NonDominantHand.transform.position;
                    __instance.previousHookPoint = __instance.hookPoint;
                    if (__instance.targeter.CurrentTarget && __instance.targeter.IsAutoAimed)
                        __instance.throwDirection = (__instance.targeter.CurrentTarget.bounds.center - Vars.NonDominantHand.transform.position).normalized; 
                    else __instance.throwDirection = Vars.NonDominantHand.transform.forward; 
                    __instance.returning = false;
                    if (__instance.caughtObjects.Count > 0)
                    {
                        foreach (Rigidbody rigidbody in __instance.caughtObjects)
                            if (rigidbody) rigidbody.velocity =
                                    (MonoSingleton<NewMovement>.Instance.transform.position - rigidbody.transform.position).normalized
                                     * (100f + __instance.returnDistance / 2f);
                        __instance.caughtObjects.Clear();
                    }
                    __instance.state = HookState.Throwing;
                    __instance.lightTarget = false;
                    __instance.throwWarp = 1f;
                    __instance.anim.Play("Throw", -1, 0f);
                    __instance.inspectLr.enabled = false;
                    //__instance.hand.transform.localPosition = new Vector3(0.09f, -0.051f, 0.045f);
                    //if (MonoSingleton<CameraController>.Instance.defaultFov > 105f)
                    //{
                    //    __instance.hand.transform.localPosition += new Vector3(0.225f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f),
                    //                                                           -0.25f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f),
                    //                                                           0.05f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f));
                    //}
                    __instance.caughtPoint = Vector3.zero;
                    __instance.caughtTransform = null;
                    __instance.caughtCollider = null;
                    __instance.caughtEid = null;
                    Object.Instantiate<GameObject>(__instance.throwSound);
                    __instance.aud.clip = __instance.throwLoop;
                    __instance.aud.panStereo = 0f;
                    __instance.aud.Play();
                    __instance.aud.pitch = Random.Range(0.9f, 1.1f);
                    __instance.semiBlocked = 0f;
                    MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.WhiplashThrow, __instance.gameObject);
                }
            }
            if (__instance.cooldown != 0f) __instance.cooldown = Mathf.MoveTowards(__instance.cooldown, 0f, Time.deltaTime); 
            if (__instance.lr.enabled)
            {
                __instance.throwWarp = Mathf.MoveTowards(__instance.throwWarp, 0f, Time.deltaTime * 6.5f);
                __instance.lr.SetPosition(0, __instance.hand.position);
                for (int i = 1; i < __instance.lr.positionCount - 1; i++)
                {
                    float d = 3f; if (i % 2 == 0) d = -3f;
                    __instance.lr.SetPosition(i, Vector3.Lerp(__instance.hand.position,
                                                              __instance.hookPoint,
                                                              (float)i / (float)__instance.lr.positionCount)
                                                              + Vars.NonDominantHand.transform.up * d *
                                                              __instance.throwWarp * (1f / (float)i));
                }
                __instance.lr.SetPosition(__instance.lr.positionCount - 1, __instance.hookPoint);
            }
            if (__instance.state == HookState.Pulling && !__instance.lightTarget && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame)
            {
                if (MonoSingleton<NewMovement>.Instance.rb.velocity.y < 1f)
                    MonoSingleton<NewMovement>.Instance.rb.velocity = new Vector3(MonoSingleton<NewMovement>.Instance.rb.velocity.x,
                                                                                  1f, MonoSingleton<NewMovement>.Instance.rb.velocity.z);
                MonoSingleton<NewMovement>.Instance.rb.velocity = Vector3.ClampMagnitude(MonoSingleton<NewMovement>.Instance.rb.velocity, 30f);
                if (!MonoSingleton<NewMovement>.Instance.gc.touchingGround
                    && !Physics.Raycast(MonoSingleton<NewMovement>.Instance.gc.transform.position,
                                        Vector3.down, 1.5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
                    MonoSingleton<NewMovement>.Instance.rb.AddForce(Vector3.up * 15f, ForceMode.VelocityChange);
                else if (!MonoSingleton<NewMovement>.Instance.jumping) MonoSingleton<NewMovement>.Instance.Jump();
                __instance.StopThrow(1f, false);
            }
            if (MonoSingleton<FistControl>.Instance.currentPunch && MonoSingleton<FistControl>.Instance.currentPunch.holding && __instance.forcingFistControl)
            {
                MonoSingleton<FistControl>.Instance.currentPunch.heldItem.transform.position = __instance.hook.position + __instance.hook.up * 0.2f;
                if (__instance.state != HookState.Ready || __instance.returning)
                {
                    MonoSingleton<FistControl>.Instance.heldObject.hooked = true;
                    if (MonoSingleton<FistControl>.Instance.heldObject.gameObject.layer != 22)
                    {
                        Transform[] componentsInChildren = MonoSingleton<FistControl>.Instance.heldObject.GetComponentsInChildren<Transform>();
                        for (int j = 0; j < componentsInChildren.Length; j++) componentsInChildren[j].gameObject.layer = 22; 
                        return false;
                    }
                }
                else
                {
                    MonoSingleton<FistControl>.Instance.heldObject.hooked = false;
                    if (MonoSingleton<FistControl>.Instance.heldObject.gameObject.layer != 13)
                    {
                        Transform[] componentsInChildren = MonoSingleton<FistControl>.Instance.heldObject.GetComponentsInChildren<Transform>();
                        for (int j = 0; j < componentsInChildren.Length; j++) componentsInChildren[j].gameObject.layer = 13; 
                    }
                }
            }

            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(nameof(HookArm.StopThrow))] static bool StopThrow(HookArm __instance, float animationTime = 0f, bool sparks = false)
        {
            MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.WhiplashThrow);
            MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.WhiplashPull);
            if (animationTime == 0f)
            {
                UnityEngine.Object.Instantiate(__instance.pullSound);
                __instance.aud.clip = __instance.pullLoop;
                __instance.aud.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                __instance.aud.panStereo = -0.5f;
                __instance.aud.Play();
            }
            else
            {
                UnityEngine.Object.Instantiate(__instance.pullDoneSound);
            }

            if (__instance.forcingGroundCheck)
            {
                __instance.StopForceGroundCheck();
            }

            if (__instance.lightTarget)
            {
                if ((bool)__instance.enemyGroundCheck)
                {
                    __instance.enemyGroundCheck.StopForceOff();
                }

                __instance.lightTarget = false;
                __instance.enemyGroundCheck = null;
                __instance.enemyRigidbody = null;
            }

            if ((bool)__instance.caughtEid)
            {
                __instance.caughtEid.hooked = false;
                __instance.caughtEid = null;
            }

            if ((bool)__instance.caughtHook)
            {
                __instance.caughtHook.Unhooked();
                __instance.caughtHook = null;
            }

            if (sparks)
            {
                UnityEngine.Object.Instantiate(__instance.clinkSparks, __instance.hookPoint, Quaternion.LookRotation(__instance.transform.position - __instance.hookPoint));
            }

            __instance.state = HookState.Ready;
            __instance.anim.Play("Pull", -1, animationTime);
            //__instance.hand.transform.localPosition = new Vector3(-0.015f, 0.071f, 0.04f);
            //if (MonoSingleton<CameraController>.Instance.defaultFov > 105f)
            //{
            //    __instance.hand.transform.localPosition += new Vector3(0.25f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f), 0f, 0.05f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 60f));
            //}
            //else if (MonoSingleton<CameraController>.Instance.defaultFov < 105f)
            //{
            //    __instance.hand.transform.localPosition -= new Vector3(0.05f * ((105f - MonoSingleton<CameraController>.Instance.defaultFov) / 60f), 0.075f * ((105f - MonoSingleton<CameraController>.Instance.defaultFov) / 60f), 0.125f * ((105f - MonoSingleton<CameraController>.Instance.defaultFov) / 60f));
            //}

            __instance.returnDistance = Mathf.Max(Vector3.Distance(__instance.transform.position, __instance.hookPoint), 25f);
            __instance.returning = true;
            __instance.throwWarp = 0f;
            if ((bool)__instance.currentWoosh)
            {
                UnityEngine.Object.Destroy(__instance.currentWoosh);
            }

            return false;
        }
    }
}
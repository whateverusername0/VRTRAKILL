using HarmonyLib;
using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches.Whiplash
{
    // you get it? cameracontrollerpatches?? ccp????? lol!!
    [HarmonyPatch] internal class HookArmP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(HookArm), nameof(HookArm.Start))] static void ConvertWhiplash(HookArm __instance)
        {
            Arm A = Arm.WhiplashPreset(__instance.transform);
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>(); AR.Arm = A;
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>(); VRAC.Arm = A;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(HookArm), nameof(HookArm.Update))] static bool Update(HookArm __instance)
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
                    MonoSingleton<RumbleManager>.Instance.SetVibrationTracked("rumble.whiplash.throw", __instance.gameObject);
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
    }
}
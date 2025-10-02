using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems.Arms;
using VRTRAKILL.Systems.VRAvatar.Armature;
using VRTRAKILL.Data;

namespace VRTRAKILL.Patches.ULTRAKILL;

[HarmonyPatch(typeof(HookArm))] internal sealed class PatchHookArm
{
    [HarmonyPostfix] [HarmonyPatch(nameof(HookArm.Start))]
    static void Start(HookArm __instance)
    {
        Arm A = Arm.WhiplashPreset(__instance.transform);
        VRArmTransformer AR = __instance.gameObject.AddComponent<VRArmTransformer>();
        VRArmController AC = __instance.gameObject.AddComponent<VRArmController>();
        AR.Arm = A; AC.Arm = A;
    }

    [HarmonyPrefix] [HarmonyPatch(nameof(HookArm.Update))]
    static bool Update(HookArm __instance)
    {
        var _i = __instance;

        if (!MonoSingleton<OptionsManager>.Instance || MonoSingleton<OptionsManager>.Instance.paused) return false;
        if (!_i.equipped || MonoSingleton<FistControl>.Instance.shopping || !MonoSingleton<FistControl>.Instance.activated)
        {
            if (_i.state != HookState.Ready || _i.returning) _i.Cancel();
            _i.model.SetActive(false);
            return false;
        }
        if (MonoSingleton<InputManager>.Instance.InputSource.Hook.WasPerformedThisFrame)
        {
            if (_i.state == HookState.Pulling) _i.StopThrow(0f, false); 
            else if (_i.cooldown <= 0f)
            {
                _i.cooldown = 0.5f;
                _i.model.SetActive(true);
                if (!_i.forcingFistControl)
                {
                    if (MonoSingleton<FistControl>.Instance.currentPunch) MonoSingleton<FistControl>.Instance.currentPunch.CancelAttack(); 
                    MonoSingleton<FistControl>.Instance.forceNoHold++;
                    _i.forcingFistControl = true;
                    MonoSingleton<FistControl>.Instance.transform.localRotation = Quaternion.identity;
                }
                _i.lr.enabled = true;
                _i.hookPoint = Vars.NonDominantHand.transform.position;
                _i.previousHookPoint = _i.hookPoint;
                if (_i.targeter.CurrentTarget && _i.targeter.IsAutoAimed)
                    _i.throwDirection = (_i.targeter.CurrentTarget.bounds.center - Vars.NonDominantHand.transform.position).normalized; 
                else _i.throwDirection = Vars.NonDominantHand.transform.forward; 
                _i.returning = false;
                if (_i.caughtObjects.Count > 0)
                {
                    foreach (Rigidbody rigidbody in _i.caughtObjects)
                        if (rigidbody) rigidbody.velocity =
                                (MonoSingleton<NewMovement>.Instance.transform.position - rigidbody.transform.position).normalized
                                 * (100f + _i.returnDistance / 2f);
                    _i.caughtObjects.Clear();
                }
                _i.state = HookState.Throwing;
                _i.lightTarget = false;
                _i.throwWarp = 1f;
                _i.anim.Play("Throw", -1, 0f);
                _i.inspectLr.enabled = false;
                //_i.hand.transform.localPosition = new Vector3(0.09f, -0.051f, 0.045f);
                //if (MonoSingleton<CameraController>.Instance.defaultFov > 105f)
                //{
                //    _i.hand.transform.localPosition += new Vector3(0.225f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f),
                //                                                           -0.25f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f),
                //                                                           0.05f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f));
                //}
                _i.caughtPoint = Vector3.zero;
                _i.caughtTransform = null;
                _i.caughtCollider = null;
                _i.caughtEid = null;
                Object.Instantiate<GameObject>(_i.throwSound);
                _i.aud.clip = _i.throwLoop;
                _i.aud.panStereo = 0f;
                _i.aud.Play();
                _i.aud.pitch = Random.Range(0.9f, 1.1f);
                _i.semiBlocked = 0f;
                MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.WhiplashThrow, _i.gameObject);
            }
        }
        if (_i.cooldown != 0f) _i.cooldown = Mathf.MoveTowards(_i.cooldown, 0f, Time.deltaTime); 
        if (_i.lr.enabled)
        {
            _i.throwWarp = Mathf.MoveTowards(_i.throwWarp, 0f, Time.deltaTime * 6.5f);
            _i.lr.SetPosition(0, _i.hand.position);
            for (int i = 1; i < _i.lr.positionCount - 1; i++)
            {
                float d = 3f; if (i % 2 == 0) d = -3f;
                _i.lr.SetPosition(i, Vector3.Lerp(_i.hand.position,
                                                          _i.hookPoint,
                                                          (float)i / (float)_i.lr.positionCount)
                                                          + Vars.NonDominantHand.transform.up * d *
                                                          _i.throwWarp * (1f / (float)i));
            }
            _i.lr.SetPosition(_i.lr.positionCount - 1, _i.hookPoint);
        }
        if (_i.state == HookState.Pulling && !_i.lightTarget && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame)
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
            _i.StopThrow(1f, false);
        }
        if (MonoSingleton<FistControl>.Instance.currentPunch && MonoSingleton<FistControl>.Instance.currentPunch.holding && _i.forcingFistControl)
        {
            MonoSingleton<FistControl>.Instance.currentPunch.heldItem.transform.position = _i.hook.position + _i.hook.up * 0.2f;
            if (_i.state != HookState.Ready || _i.returning)
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

    [HarmonyPrefix] [HarmonyPatch(nameof(HookArm.StopThrow))]
    static bool StopThrow(HookArm __instance, float animationTime = 0f, bool sparks = false)
    {
        var _i = __instance;

        MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.WhiplashThrow);
        MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.WhiplashPull);
        if (animationTime == 0f)
        {
            Object.Instantiate(_i.pullSound);
            _i.aud.clip = _i.pullLoop;
            _i.aud.pitch = Random.Range(0.9f, 1.1f);
            _i.aud.panStereo = -0.5f;
            _i.aud.Play();
        }
        else
        {
            Object.Instantiate(_i.pullDoneSound);
        }

        if (_i.forcingGroundCheck)
        {
            _i.StopForceGroundCheck();
        }

        if (_i.lightTarget)
        {
            if ((bool)_i.enemyGroundCheck)
            {
                _i.enemyGroundCheck.StopForceOff();
            }

            _i.lightTarget = false;
            _i.enemyGroundCheck = null;
            _i.enemyRigidbody = null;
        }

        if ((bool)_i.caughtEid)
        {
            _i.caughtEid.hooked = false;
            _i.caughtEid = null;
        }

        if ((bool)_i.caughtHook)
        {
            _i.caughtHook.Unhooked();
            _i.caughtHook = null;
        }

        if (sparks)
        {
            Object.Instantiate(_i.clinkSparks, _i.hookPoint, Quaternion.LookRotation(_i.transform.position - _i.hookPoint));
        }

        _i.state = HookState.Ready;
        _i.anim.Play("Pull", -1, animationTime);

        _i.returnDistance = Mathf.Max(Vector3.Distance(_i.transform.position, _i.hookPoint), 25f);
        _i.returning = true;
        _i.throwWarp = 0f;
        if ((bool)_i.currentWoosh)
        {
            Object.Destroy(_i.currentWoosh);
        }

        return false;
    }
}
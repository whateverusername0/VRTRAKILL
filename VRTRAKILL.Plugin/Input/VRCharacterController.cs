using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using VRTRAKILL.Plugin.Preferences;

namespace VRTRAKILL.Plugin.Input;

[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
public class VRCharacterController : MonoSingleton<VRCharacterController>
{
    private PreferencesJson _prefs => PluginPreferencesManager.Instance.Config;

    private Transform _leftHand, _rightHand;

    public Vector3
        HeadPosition = Vector3.zero,
        LeftHandPosition = Vector3.zero,
        RightHandPosition = Vector3.zero;

    public Quaternion
        HeadRotation = Quaternion.identity,
        LeftHandRotation = Quaternion.identity,
        RightHandRotation = Quaternion.identity;

    public Transform LeftHand => _leftHand;

    public Transform RightHand => _rightHand;

    private bool IsTurning = false;

    private float SnapTurnTimer = 0f;

    public float TurnOffset = 0f;

    public void Start()
    {
        // i'm sorry
        base.StartCoroutine(this.StartCoroutine());
    }

    public void Update()
    {
        var head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        head.TryGetFeatureValue(CommonUsages.devicePosition, out HeadPosition);
        head.TryGetFeatureValue(CommonUsages.deviceRotation, out HeadRotation);

        if (_leftHand != null)
        {
            LeftHandPosition = _leftHand.position;
            LeftHandRotation = _leftHand.rotation;
        }

        if (_rightHand != null)
        {
            RightHandPosition = _rightHand.position;
            RightHandRotation = _rightHand.rotation;
        }

        var turnSpeed = _prefs.SmoothTurnSpeed;
        var snap = _prefs.SnapTurning;
        var snapAngles = _prefs.SnapAnglesPerTurn;
        var snapSpeed = _prefs.SnapTurnSpeed;
        var deadzone = _prefs.ControllerDeadzone;

        var x = SteamVRInputBridge.LookVector.x;
        var direction = x > 0 ? 1 : -1;
        var turnOffset = direction * (snap
            ? snapAngles
            : turnSpeed * Time.deltaTime);

        IsTurning = x > deadzone || x < -deadzone;

        if (IsTurning && !snap)
            TurnOffset += turnOffset;

        if (IsTurning && snap)
        {
            SnapTurnTimer -= Time.deltaTime;
            if (SnapTurnTimer <= 0)
            {
                SnapTurnTimer = snapSpeed;
                TurnOffset += turnOffset;
            }
        }

        // reset timer
        if (!IsTurning) SnapTurnTimer = 0f;
    }

    private IEnumerator StartCoroutine()
    {
        transform.position = Vector3.zero; // paranoia level god

        if (_leftHand != null) DestroyImmediate(_leftHand.gameObject);
        _leftHand = AddHand(SteamVR_Input_Sources.LeftHand, SteamVR_Actions._default.LeftPose);

        if (_rightHand != null) DestroyImmediate(_rightHand.gameObject);
        _rightHand = AddHand(SteamVR_Input_Sources.RightHand, SteamVR_Actions._default.RightPose);

        _leftHand.gameObject.SetActive(false);
        _rightHand.gameObject.SetActive(false);
        yield return null;

        _leftHand.gameObject.SetActive(true);
        _rightHand.gameObject.SetActive(true);
    }

    private Transform AddHand(SteamVR_Input_Sources source, SteamVR_Action_Pose pose)
    {
        var go = new GameObject(source.ToString());
        go.transform.SetParent(transform);
        var p = go.AddComponent<SteamVR_Behaviour_Pose>();
        p.poseAction = pose;
        p.inputSource = source;
        return go.transform;
    }
}
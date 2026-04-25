using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Valve.VR;

namespace VRTRAKILL.Systems.Input;

public static class SteamVRPlayerInput
{
    public static SteamVR_Input_ActionSet_default DefaultSet => SteamVR_Actions._default;

    public static Vector2 MoveVector, LookVector;

    private static bool _initialized = false;

    #region ULTRAKILL binds map

    // used for name comparison with PlayerInput

    public static SteamVR_Action_Vector2 Move => DefaultSet.Move;
    public static SteamVR_Action_Vector2 Look => DefaultSet.Turn;
    public static SteamVR_Action_Vector2 WheelLook => DefaultSet.Move;
    public static SteamVR_Action_Boolean Punch => DefaultSet.Punch;
    public static SteamVR_Action_Boolean Hook => DefaultSet.Hook;
    public static SteamVR_Action_Boolean Fire1 => DefaultSet.PrimaryFire;
    public static SteamVR_Action_Boolean Fire2 => DefaultSet.SecondaryFire;
    public static SteamVR_Action_Boolean Jump => DefaultSet.Jump;
    public static SteamVR_Action_Boolean Slide => DefaultSet.Slide;
    public static SteamVR_Action_Boolean Dodge => DefaultSet.Dodge;
    public static SteamVR_Action_Boolean ChangeFist => DefaultSet.ChangeFist;
    public static SteamVR_Action_Boolean NextVariation => DefaultSet.NextVariation;
    public static SteamVR_Action_Boolean PreviousVariation => DefaultSet.PreviousVariation;
    public static SteamVR_Action_Boolean NextWeapon => DefaultSet.NextWeapon;
    public static SteamVR_Action_Boolean PrevWeapon => DefaultSet.PreviousWeapon;
    public static SteamVR_Action_Boolean LastWeapon => DefaultSet.LastWeapon;
    public static SteamVR_Action_Boolean SelectVariant1 => DefaultSet.Variation1;
    public static SteamVR_Action_Boolean SelectVariant2 => DefaultSet.Variation2;
    public static SteamVR_Action_Boolean SelectVariant3 => DefaultSet.Variation3;
    public static SteamVR_Action_Boolean Pause => DefaultSet.Escape;
    public static SteamVR_Action_Boolean Stats => DefaultSet.Stats;
    public static SteamVR_Action_Boolean Slot1 => DefaultSet.Slot1;
    public static SteamVR_Action_Boolean Slot2 => DefaultSet.Slot2;
    public static SteamVR_Action_Boolean Slot3 => DefaultSet.Slot3;
    public static SteamVR_Action_Boolean Slot4 => DefaultSet.Slot4;
    public static SteamVR_Action_Boolean Slot5 => DefaultSet.Slot5;
    public static SteamVR_Action_Boolean Slot6 => DefaultSet.Slot6;

    #endregion

    public static void Initialize()
    {
        if (_initialized) return;

        SteamVR_Actions._default.Activate();

        _initialized = true;

        Move.AddOnUpdateListener(UpdateMove, SteamVR_Input_Sources.Any);
        Look.AddOnUpdateListener(UpdateLook, SteamVR_Input_Sources.Any);
        // inherits from Move
        //WheelLook.AddOnUpdateListener(UpdateVector2, SteamVR_Input_Sources.Any);
        Punch.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Hook.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Fire1.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Fire2.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Jump.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Slide.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Dodge.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        ChangeFist.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        NextVariation.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        PreviousVariation.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        NextWeapon.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        PrevWeapon.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        LastWeapon.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        SelectVariant1.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        SelectVariant2.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        SelectVariant3.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Pause.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Stats.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Slot1.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Slot2.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Slot3.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Slot4.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Slot5.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
        Slot6.AddOnUpdateListener(UpdateBool, SteamVR_Input_Sources.Any);
    }

    private static void UpdateLook(SteamVR_Action_Vector2 from, SteamVR_Input_Sources source, Vector2 axis, Vector2 delta)
    {
        ResolveInternal(from);
        MoveVector = axis;
    }

    private static void UpdateMove(SteamVR_Action_Vector2 from, SteamVR_Input_Sources source, Vector2 axis, Vector2 delta)
    {
        ResolveInternal(from);
        LookVector = axis;
    }

    public static void UpdateBool(SteamVR_Action_Boolean from, SteamVR_Input_Sources source, bool newState)
        => ResolveInternal(from);

    private static void ResolveInternal(ISteamVR_Action_Source svrAction)
    {
        var action = ResolveAction(svrAction);
        if (action == null) return;
        UpdatePerformed(action, svrAction);
    }

    public static InputAction ResolveAction(ISteamVR_Action_Source svrAction)
    {
        var inp = InputManager.Instance.InputSource;
        var fieldInfo = inp.GetType().GetFields().Where(q => q.Name == nameof(svrAction)).FirstOrDefault();
        if (fieldInfo == null) return null;

        var action = fieldInfo.GetValue(inp) as InputActionState;
        return action.Action;
    }

    public static void UpdatePerformed(InputAction action, ISteamVR_Action_Source svrAction)
    {
        if (action == null || action.controls.Count <= 0 || svrAction == null)
            return;

        var control = action.controls[0];
        //var actionType = svrAction.GetType(); // todo fix
        UpdatePerformedInternal(control, (dynamic)svrAction);
    }

    private static void UpdatePerformedInternal(InputControl control, SteamVR_Action_Boolean svrAction)
        => InputSystem.QueueDeltaStateEvent(control, svrAction.state);

    private static void UpdatePerformedInternal(InputControl control, SteamVR_Action_Vector2 svrAction)
        => InputSystem.QueueDeltaStateEvent(control, svrAction.delta);
}
using BepInEx.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
using VRTRAKILL.Plugin.Input;
using VRTRAKILL.Plugin.Preferences;
using VRTRAKILL.Plugin.Utilities;

namespace VRTRAKILL.Plugin.Systems;

[DefaultExecutionOrder(1100)]
public class VRUIInteractor : MonoSingleton<VRUIInteractor>
{
    private ManualLogSource _log => Plugin.Instance.Log;
    private VRCameraController _vrcc => VRCameraController.Instance;
    private OptionsManager _options => OptionsManager.Instance;
    private PluginPreferencesManager _prefs => PluginPreferencesManager.Instance;

    public Camera InteractorCamera { get; protected set; }

    // hand that owns the laser; the last trigger pressed takes it over
    public SteamVR_Input_Sources ActiveHand = SteamVR_Input_Sources.RightHand;

    public bool HasHit { get; protected set; }

    public Vector3 HitPoint { get; protected set; }

    // true while the laser is on a button or other item, not on a HUD graphic
    public bool OverItem { get; protected set; }

    protected const float
        DragAngle = 4f,
        NavPress = .6f,
        NavRelease = .35f,
        NavDelay = .4f,
        NavRepeat = .15f;

    private EventSystem _es;
    private PointerEventData _pointer;
    private List<RaycastResult> _results = new();

    private GameObject _hovered;
    private bool _hoverMuted;
    private SteamVR_Input_Sources _pressHand = SteamVR_Input_Sources.Any;
    private Vector3 _pressForward;

    private bool _navHeld;
    private float _nextNavTime;

    protected void Start()
    {
        if (_vrcc.UICamera == null)
        {
            _log.LogError("Cannot make a UI interactor. For some reason.");
            return;
        }

        Validate();
    }

    protected void LateUpdate()
    {
        Validate();

        if (EventSystem.current == null || InteractorCamera == null)
            return;

        if (_es != EventSystem.current || _pointer == null)
        {

        }
    }

    protected void Validate()
    {
        if (InteractorCamera == null)
        {
            var c = new GameObject("VR UI Interaction Camera").EnsureComponent<Camera>();
            c.enabled = false;
            c.depth = 6565f; // random arbitrary large number
            c.fieldOfView = 1f;
            c.nearClipPlane = .01f;
            c.cullingMask = -1; // NOTHING

            InteractorCamera = c;
        }
    }

    #region Pointer

    // Clears the old hover and press and creates a new pointer for the event system.
    protected void ResetPointer(EventSystem New_es)
    {
        if (_pointer != null)
        {
            SetHover(null);
            ClearPress();
        }

        _es = New_es;

        _pointer = new(_es)
        {
            button = PointerEventData.InputButton.Left,
            pointerId = -2
        };
    }

    // Hides the OS mouse from the game's input module,
    // which also casts it through this camera and would double every click.
    protected void HideModuleMouse()
    {
        var module = _es.currentInputModule;
        if (module == null || module.inputOverride is NoMouseInput)
            return;

        if (module.TryGetComponent<NoMouseInput>(out var nmi))
            module.inputOverride = nmi;
        else module.inputOverride = module.gameObject.AddComponent<NoMouseInput>();
    }

    // Parents the UI event camera to the gun offset of the active hand.
    protected void AttachToHand()
    {
        // TODO change it to new controller system
        VRControllersSystem hand = VRControllersSystem.Get(ActiveHand);

        if (hand == null || hand.GunOffset == null || transform.parent == hand.GunOffset.transform)
            return;

        transform.SetParent(hand.GunOffset.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // Casts the pointer from the screen center and returns the first UI object hit.
    protected GameObject Raycast()
    {
        var irc = InteractorCamera;
        _pointer.position = new Vector2(irc.pixelWidth / 2f, irc.pixelHeight / 2f);
        _pointer.delta = Vector2.zero;
        _results.Clear();
        _es.RaycastAll(_pointer, _results);

        RaycastResult first = default;
        foreach (var r in _results)
            if (r.gameObject != null) { first = r; break; }

        _results.Clear();

        _pointer.pointerCurrentRaycast = first;
        HasHit = first.gameObject != null;

        if (HasHit) HitPoint = first.worldPosition;

        return first.gameObject;
    }

    // Moves the hover to a new target, records whether it is an item and, in menus, selects it.
    protected void UpdateHover(GameObject target)
    {
        if (target == _hovered) return;
        _hoverMuted = false;
        OverItem = target != null
            && (target.GetComponentInParent<Selectable>() != null
            || target.GetComponentInParent<ControllerPointer>() != null);

        SetHover(target);

        // pointing at an item also selects it, so the stick and confirm continue from there
        if (_options.paused && target != null
        && target.GetComponentInParent<Selectable>() is Selectable s
        && CanSelect(s) && !IsTextField(s))
            Select(s);
    }

    // Sends exit and enter events to every object between the old and new target, like PointerInputModule.
    protected void SetHover(GameObject target)
    {
        var root = CommonRoot(_hovered, target);
        if (_hovered != null && !_hoverMuted)
            for (var t = _hovered.transform; t != null && t != root; t = t.parent)
                ExecuteEvents.Execute(t.gameObject, _pointer, ExecuteEvents.pointerExitHandler);

        _pointer.hovered.Clear();
        if (target != null)
        {
            for (var t = target.transform; t != null && t != root; t = t.parent)
            {
                ExecuteEvents.Execute(t.gameObject, _pointer, ExecuteEvents.pointerEnterHandler);
                _pointer.hovered.Add(t.gameObject);
            }
        }

        _hovered = target;
        _pointer.pointerEnter = target;
    }

    // stops the laser highlight once the stick takes over, until the laser moves to another item
    protected void MuteHover()
    {
        if (_hovered == null || _hoverMuted)
            return;

        for (var t = _hovered.transform; t != null; t = t.parent)
            ExecuteEvents.Execute(t.gameObject, _pointer, ExecuteEvents.pointerExitHandler);

        _hoverMuted = true;
    }

    // Returns the closest shared parent of two objects, or null.
    protected Transform CommonRoot(GameObject a, GameObject b)
    {
        if (a == null || b == null)
            return null;

        for (var t = a.transform; t != null; t = t.parent)
            if (b.transform.IsChildOf(t))
                return t;

        return null;
    }

    // Starts a press on the target with the active hand, unless the game's ControllerPointer handles it.
    protected void Press(GameObject target)
    {
        // the game's own ControllerPointer handles its screens (shop, terminals) through Fire1
        if (target == null || target.GetComponentInParent<ControllerPointer>() != null) return;

        _pressHand = ActiveHand;
        _pressForward = transform.forward;
        _pointer.eligibleForClick = true;
        _pointer.dragging = false;
        _pointer.useDragThreshold = true;
        _pointer.pressPosition = _pointer.position;
        _pointer.pointerPressRaycast = _pointer.pointerCurrentRaycast;

        var pressed = ExecuteEvents.ExecuteHierarchy(target, _pointer, ExecuteEvents.pointerDownHandler)
            ?? ExecuteEvents.GetEventHandler<IPointerClickHandler>(target);

        _pointer.pointerPress = pressed;
        _pointer.rawPointerPress = target;
        _pointer.clickTime = Time.unscaledTime;
        _pointer.clickCount = 1;

        _pointer.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(target);
        if (_pointer.pointerDrag != null)
            ExecuteEvents.Execute(_pointer.pointerDrag, _pointer, ExecuteEvents.initializePotentialDrag);
    }

    // Starts and continues a drag once the hand turns past the drag angle since the press.
    protected void Drag()
    {
        if (_pointer.pointerDrag == null) return;
        // the laser always casts from the screen center, so movement is the hand's angle since the press
        if (!_pointer.dragging && (!_pointer.useDragThreshold
        || Vector3.Angle(_pressForward, transform.forward) > DragAngle))
        {
            ExecuteEvents.Execute(_pointer.pointerDrag, _pointer, ExecuteEvents.beginDragHandler);
            _pointer.dragging = true;
            _pointer.eligibleForClick = _pointer.pointerDrag == _pointer.pointerPress && !_pointer.useDragThreshold;
        }
        if (_pointer.dragging)
            ExecuteEvents.Execute(_pointer.pointerDrag, _pointer, ExecuteEvents.dragHandler);
    }

    // Sends pointer up, then click or drop, then end drag, and clears the press.
    protected void Release(GameObject tgt)
    {
        if (_pointer.pointerPress != null)
            ExecuteEvents.Execute(_pointer.pointerPress, _pointer, ExecuteEvents.pointerUpHandler);

        var clickHandler = tgt != null ? ExecuteEvents.GetEventHandler<IPointerClickHandler>(tgt) : null;

        if (_pointer.pointerPress != null && _pointer.pointerPress == clickHandler && _pointer.eligibleForClick)
            ExecuteEvents.Execute(_pointer.pointerPress, _pointer, ExecuteEvents.pointerClickHandler);
        else if (_pointer.dragging && tgt != null)
            ExecuteEvents.ExecuteHierarchy(tgt, _pointer, ExecuteEvents.dropHandler);

        if (_pointer.dragging && _pointer.pointerDrag != null)
            ExecuteEvents.Execute(_pointer.pointerDrag, _pointer, ExecuteEvents.endDragHandler);

        ClearPress();
    }

    // Clears the press and drag state.
    protected void ClearPress()
    {
        _pressHand = SteamVR_Input_Sources.Any;
        _pointer.eligibleForClick = false;
        _pointer.dragging = false;
        _pointer.pointerPress = null;
        _pointer.rawPointerPress = null;
        _pointer.pointerDrag = null;
    }

    protected bool TriggerDown(SteamVR_Input_Sources hand)
        => SteamVR_Actions._default.PrimaryFire.GetStateDown(hand)
        || SteamVR_Actions._default.Punch.GetStateDown(hand);

    protected bool TriggerHeld(SteamVR_Input_Sources hand)
        => SteamVR_Actions._default.PrimaryFire.GetState(hand)
        || SteamVR_Actions._default.Punch.GetState(hand);

    #endregion

    #region Stick navigation

    // Moves the menu selection with the movement stick, with a delay before it repeats.
    protected void Navigate()
    {
        var axis = SteamVRInputBridge.MoveVector;
        var max = Mathf.Max(Mathf.Abs(axis.x), Mathf.Abs(axis.y));
        if (max < NavRelease) { _navHeld = false; return; }
        if (max < NavPress) return;
        if (_navHeld && Time.unscaledTime < _nextNavTime) return;
        _nextNavTime = Time.unscaledTime + (_navHeld ? NavRepeat : NavDelay);
        _navHeld = true;

        MuteHover();

        var currentSelection = CurrentSelection();
        if (currentSelection == null) { Select(InitialSelection()); return; }

        // same move event the game's input module sends for a gamepad,
        // so sliders and custom handlers react too
        var move = new AxisEventData(_es)
        {
            moveVector = axis,
            moveDir = Mathf.Abs(axis.x) > Mathf.Abs(axis.y)
                ? (axis.x > 0 ? MoveDirection.Right : MoveDirection.Left)
                : (axis.y > 0 ? MoveDirection.Up : MoveDirection.Down)
        };
        ExecuteEvents.Execute(currentSelection.gameObject, move, ExecuteEvents.moveHandler);
        if (CurrentSelection() is Selectable Next) Tint(Next);
    }

    // Submits the selected item, or selects a first item if none is selected.
    protected void Submit()
    {
        Selectable Current = CurrentSelection();
        if (Current == null) { MuteHover(); Select(InitialSelection()); return; }
        ExecuteEvents.Execute(Current.gameObject, new BaseEventData(_es), ExecuteEvents.submitHandler);
    }

    // Scrolls the hovered item with the turn stick;
    // ScrollRects with ScrollRectMouseControl already follow the simulated wheel.
    protected void Scroll()
    {
        var scroll = SteamVRInputBridge.LookVector.y;
        if (_hovered == null || Mathf.Abs(scroll) < _prefs.Config.ControllerDeadzone * 1.5f)
            return;

        var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(_hovered);
        if (scrollHandler == null || scrollHandler.GetComponent<ScrollRectMouseControl>() != null)
            return;

        _pointer.scrollDelta = new Vector2(0, Mathf.Sign(scroll));
        ExecuteEvents.Execute(scrollHandler, _pointer, ExecuteEvents.scrollHandler);
        _pointer.scrollDelta = Vector2.zero;
    }

    // Returns the selected item if it can be selected, else null.
    protected Selectable CurrentSelection()
    {
        var go = _es.currentSelectedGameObject;
        var s = go != null ? go.GetComponentInParent<Selectable>() : null;
        return CanSelect(s) ? s : null;
    }

    // Returns the first item to select: the pointed item,
    // the GamepadObjectSelector target or the top canvas item.
    protected Selectable InitialSelection()
    {
        if (_hovered != null && _hovered.GetComponentInParent<Selectable>() is Selectable pointed
        && CanSelect(pointed))
            return pointed;

        if (TopSelector() is GamepadObjectSelector top
        && top != null && top.isActiveAndEnabled && top.target != null)
        {
            if (top.firstChild)
            {
                foreach (var sc in top.target.GetComponentsInChildren<Selectable>())
                {
                    if (sc.gameObject != top.target
                    && sc.navigation.mode != Navigation.Mode.None
                    && CanSelect(sc))
                        return sc;
                }
            }
            if (top.target.GetComponent<Selectable>() is Selectable s && CanSelect(s))
                return s;
        }

        Selectable best = null;
        var bestOrder = int.MinValue;
        foreach (var s in Selectable.allSelectablesArray)
        {
            if (!CanSelect(s) || s.navigation.mode == Navigation.Mode.None)
                continue;

            var c = s.GetComponentInParent<Canvas>();
            int order = c != null && c.rootCanvas != null ? c.rootCanvas.sortingOrder : 0;
            if (best == null || order > bestOrder)
            {
                best = s;
                bestOrder = order;
            }
        }
        return best;
    }

    // s_Selectors is a Stack<> the plugin's reference set cannot resolve, so read it untyped;
    // a stack enumerates top first
    protected readonly FieldInfo _selectors = typeof(GamepadObjectSelector).GetField("s_Selectors",
        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    // Returns the top GamepadObjectSelector on the selector stack, or null.
    protected GamepadObjectSelector TopSelector()
    {
        if (_selectors?.GetValue(null) is IEnumerable Stack)
            foreach (object top in Stack) return top as GamepadObjectSelector;
        return null;
    }

    // Tints and selects the item unless it is already selected.
    protected void Select(Selectable S)
    {
        if (S == null || _es.currentSelectedGameObject == S.gameObject)
            return;

        Tint(S);

        _es.SetSelectedGameObject(S.gameObject, new BaseEventData(_es));
    }

    // Copies the hover color to the selected color, since converted canvases only recolor hover.
    protected void Tint(Selectable s)
    {
        if (s.transition != Selectable.Transition.ColorTint
        || s.colors.selectedColor == s.colors.highlightedColor)
            return;

        var block = s.colors;
        block.selectedColor = block.highlightedColor;
        s.colors = block;
    }

    // Returns true if the item is active, enabled and interactable.
    protected bool CanSelect(Selectable S)
        => S != null && S.isActiveAndEnabled && S.gameObject.activeInHierarchy && S.IsInteractable();

    // Returns true for text fields, which the laser only highlights since selecting one opens it for typing.
    protected bool IsTextField(Selectable S)
        => S is InputField || S.GetType().Name == "TMP_InputField";

    #endregion
}
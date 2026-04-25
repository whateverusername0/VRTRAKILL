using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VRTRAKILL.Systems.UI;

// borrowed and refactored from huskVR
public class VRCanvasInteraction : MonoBehaviour
{
    private InputManager _input => InputManager.Instance;

    public Camera Camera;

    // Prevents loop over the same selectable
    private Selectable ExcludedSelectable;
    private Selectable CurrentSelectable;
    private RaycastResult CurrentRaycastResult;

    private IPointerClickHandler ClickHandler;
    private IDragHandler DragHandler;

    private PointerEventData PointerEvent;

    public void Start()
    {
        if (Camera == null) Camera = GetComponent<Camera>();
        PointerEvent = new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left };
    }

    public void Update()
    {
        // Set pointer position
        PointerEvent.Reset();
        PointerEvent.position = new(Camera.pixelWidth / 2, Camera.pixelHeight / 2);

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(PointerEvent, results);

        if (results.Count == 0)
        {
            if (CurrentSelectable || ExcludedSelectable)
                Select(null, null);
            return;
        }

        foreach (var result in results)
        {
            var newSelectable = result.gameObject.GetComponentInParent<Selectable>();
            if (!newSelectable) continue;

            if (newSelectable != ExcludedSelectable && newSelectable != CurrentSelectable)
            {
                Select(newSelectable);
                CurrentRaycastResult = result;
            }
            break;
        }

        // Target is being activated
        var fire = _input != null && _input.InputSource != null ? _input.InputSource.Fire1 : null;
        var firePressed = fire != null && fire.IsPressed;
        var fireWasPressed = fire != null && fire.WasPerformedThisFrame;

        if (CurrentSelectable != null && firePressed)
        {
            if (ClickHandler != null && fireWasPressed)
            {
                ClickHandler.OnPointerClick(PointerEvent);
                Select(null, CurrentSelectable);
            }
            else if (DragHandler != null)
            {
                PointerEvent.pointerPressRaycast = CurrentRaycastResult;
                DragHandler.OnDrag(PointerEvent);
            }
        }
    }

    private void Select(Selectable selectable, Selectable exclude = null)
    {
        ExcludedSelectable = exclude;

        if (CurrentSelectable) CurrentSelectable.OnPointerExit(PointerEvent);

        CurrentSelectable = selectable;

        if (CurrentSelectable)
        {
            CurrentSelectable.OnPointerEnter(PointerEvent);
            ClickHandler = CurrentSelectable.GetComponent<IPointerClickHandler>();
            DragHandler = CurrentSelectable.GetComponent<IDragHandler>();
        }
    }
}

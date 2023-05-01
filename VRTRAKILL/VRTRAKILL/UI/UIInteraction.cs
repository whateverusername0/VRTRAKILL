using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr, ty you pretty
    internal class UIInteraction : MonoBehaviour
    {
        // Prevents loop over the same selectable
        Selectable ExcludedSelectable;
        Selectable CurrentSelectable;
        RaycastResult CurrentRaycastResult;

        IPointerClickHandler ClickHandler;
        IDragHandler DragHandler;

        PointerEventData PointerEvent;

        private void Start()
        => PointerEvent = new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left };

        void Update()
        {
            // Set pointer position
            PointerEvent.position = new Vector2(XRSettings.eyeTextureWidth / 2, XRSettings.eyeTextureHeight / 2);

            List<RaycastResult> RaycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(PointerEvent, RaycastResults);

            // Detect selectable
            if (RaycastResults.Count > 0)
            {
                foreach (RaycastResult Result in RaycastResults)
                {
                    Selectable NewSelectable = Result.gameObject.GetComponentInParent<Selectable>();

                    if (NewSelectable)
                    {
                        if (NewSelectable != ExcludedSelectable && NewSelectable != CurrentSelectable)
                        {
                            Select(NewSelectable);
                            CurrentRaycastResult = Result;
                        }
                        break;
                    }
                }
            }
            else
            {
                if (CurrentSelectable || ExcludedSelectable)
                {
                    Select(null, null);
                }
            }

            // Target is being activated
            if (CurrentSelectable != null && InputManager.Instance.InputSource.Fire1.IsPressed)
            {
                if (ClickHandler != null && (InputManager.Instance?.InputSource.Fire1.WasPerformedThisFrame ?? false))
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

        void Select(Selectable S, Selectable Exclude = null)
        {
            ExcludedSelectable = Exclude;

            if (CurrentSelectable) CurrentSelectable.OnPointerExit(PointerEvent);

            CurrentSelectable = S;

            if (CurrentSelectable)
            {
                CurrentSelectable.OnPointerEnter(PointerEvent);
                ClickHandler = CurrentSelectable.GetComponent<IPointerClickHandler>();
                DragHandler = CurrentSelectable.GetComponent<IDragHandler>();
            }
        }
    }
}

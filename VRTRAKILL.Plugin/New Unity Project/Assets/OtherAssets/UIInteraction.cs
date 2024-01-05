﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr, ty you pretty
    internal class UIInteraction : MonoBehaviour
    {
        public Camera Cam;

        // Prevents loop over the same selectable
        Selectable ExcludedSelectable;
        Selectable CurrentSelectable;
        RaycastResult CurrentRaycastResult;

        IPointerClickHandler ClickHandler;
        IDragHandler DragHandler;

        PointerEventData PointerEvent;

        public LineRenderer LR;

        public void Start()
        {
            if (Cam == null) Cam = GetComponent<Camera>();
            PointerEvent = new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left };
        }

        public void Update()
        {
            

            // Set pointer position
            PointerEvent.position = new Vector2(Cam.pixelWidth / 2, Cam.pixelHeight / 2);

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
            if (CurrentSelectable != null)
            {
                if (ClickHandler != null)
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

        private void Select(Selectable S, Selectable Exclude = null)
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

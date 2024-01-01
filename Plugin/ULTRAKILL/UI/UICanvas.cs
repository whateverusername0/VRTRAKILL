﻿using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.UI
{
    // "borrowed" from huskvr
    internal sealed class UICanvas : MonoBehaviour
    {
        private Vector3 LastCamFwd = Vector3.zero;

        private const float Distance = 72f;
        private static float Scale => Vars.Config.UIInteraction.UISize;

        private void UpdatePos()
        {
            LastCamFwd = UIConverter.UICamera.transform.forward * Distance;
            transform.rotation = UIConverter.UICamera.transform.rotation;
        }
        private void ResetPos()
        {
            LastCamFwd = new Vector3(LastCamFwd.x, 0f, LastCamFwd.z);
            transform.LookAt(UIConverter.UICamera.transform);
            transform.forward = new Vector3(-transform.forward.x, 0f, -transform.forward.z);
        }

        public void Start()
        {
            transform.localScale = Vector3.one * Scale;
            LastCamFwd = Vector3.back * Distance;
            UpdatePos();
        }
        public void Update()
        {
            if (!Vars.IsPlayerFrozen) UpdatePos(); else ResetPos();
            transform.position = UIConverter.UICamera.transform.position + LastCamFwd;
        }
    }
}

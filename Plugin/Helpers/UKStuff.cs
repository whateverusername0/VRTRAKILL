using System;
using UnityEngine;

namespace Plugin.Helpers
{
    internal class UKStuff
    {
        private static Camera _MainCamera; public static Camera MainCamera
        {
            get
            {
                if (_MainCamera == null) _MainCamera = MonoSingleton<CameraController>.Instance.cam;
                return _MainCamera;
            }
        }
        private static Camera _HUDCamera; public static Camera HUDCamera
        {
            get
            {
                if (_HUDCamera == null)
                    _HUDCamera = MonoSingleton<CameraController>.Instance.gameObject
                                 .transform.GetChild(1).GetComponent<Camera>();
                return _HUDCamera;
            }
        }
    }
}

using UnityEngine;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr
    internal class UICanvas : MonoBehaviour
    {
        private Vector3 LastCamFwd = Vector3.zero;

        private const float Distance = 72f;
        private static float Scale = Vars.Config.VRSettings.UISize;

        public void UpdatePos()
        {
            LastCamFwd = UIConverter.UICamera.transform.forward * Distance;
            transform.rotation = UIConverter.UICamera.transform.rotation;
        }
        public void ResetPos()
        {
            LastCamFwd = new Vector3(LastCamFwd.x, 0f, LastCamFwd.z);
            transform.LookAt(UIConverter.UICamera.transform);
            transform.forward = new Vector3(-transform.forward.x, 0f, -transform.forward.z);
        }

        private void Start()
        {
            transform.localScale = Vector3.one * Scale;
            LastCamFwd = Vector3.back * Distance;
        }
        private void Update()
        {
            if (!Vars.IsAMenu) UpdatePos(); else ResetPos();
            transform.position = UIConverter.UICamera.transform.position + LastCamFwd;
        }
    }
}

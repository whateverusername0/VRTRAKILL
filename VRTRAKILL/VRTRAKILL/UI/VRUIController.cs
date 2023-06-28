using UnityEngine;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr
    internal class VRUIController : MonoBehaviour
    {
        private Vector3 LastCamFwd = Vector3.zero;

        private const float Distance = 72f;
        private static float Scale => Vars.Config.View.VRUI.UISize;

        private void UpdatePos()
        {
            LastCamFwd = SceneWorker.UICamera.transform.forward * Distance;
            transform.rotation = SceneWorker.UICamera.transform.rotation;
        }
        private void ResetPos()
        {
            LastCamFwd = new Vector3(LastCamFwd.x, 0f, LastCamFwd.z);
            transform.LookAt(SceneWorker.UICamera.transform);
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
            if (!Vars.IsAMenu && !Vars.IsWeaponWheelPresent) UpdatePos(); else ResetPos();
            transform.position = SceneWorker.UICamera.transform.position + LastCamFwd;
        }
    }
}

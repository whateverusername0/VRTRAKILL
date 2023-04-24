using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr
    internal class UICanvas : MonoBehaviour
    {
        private Vector3 LastCamFwd = Vector3.zero;

        private const float Distance = 72f;
        private static float Scale = 0.0625f;

        public void UpdatePos()
        {
            LastCamFwd = VRUIController.UICamera.transform.forward * Distance;
            transform.rotation = VRUIController.UICamera.transform.rotation;
        }
        public void ResetPos()
        {
            LastCamFwd = new Vector3(LastCamFwd.x, 0f, LastCamFwd.z);
            transform.LookAt(VRUIController.UICamera.transform);
            transform.forward = new Vector3(-transform.forward.x, 0f, -transform.forward.z);
        }

        private void Start()
        {
            transform.localScale = Vector3.one * Scale;
            LastCamFwd = Vector3.back * Distance;
        }
        private void Update()
        {
            if (Vars.NotAMenu) UpdatePos(); else ResetPos();
            transform.position = VRUIController.UICamera.transform.position + LastCamFwd;
        }

        public static void ConvertCanvas(Canvas C)
        {
            if (C.renderMode != RenderMode.ScreenSpaceOverlay) return;
            C.worldCamera = VRUIController.UICamera;
            C.renderMode = RenderMode.WorldSpace;
            C.gameObject.layer = 5; // ui
            C.gameObject.AddComponent<UICanvas>();

            foreach (Transform Child in C.transform) ConvertElement(Child);
        }

        private static void ConvertElement(Transform Element)
        {
            if (Element.GetComponent<Selectable>() is Selectable Button)
            {
                ColorBlock block = Button.colors;
                block.highlightedColor = Color.red;
                Button.colors = block;
            }

            foreach (Transform Child in Element) ConvertElement(Child);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr
    internal class UICanvas : MonoBehaviour
    {
        public static bool ShouldUpdatePos =>
            !SceneManager.GetActiveScene().name.StartsWith("Main Menu") &&
            !(OptionsManager.Instance != null && OptionsManager.Instance.paused) &&
            !(SpawnMenu.Instance != null && SpawnMenu.Instance.gameObject.activeInHierarchy);

        private Vector3 LastCamFwd = Vector3.zero;

        private const float Distance = 69f;
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
            if (ShouldUpdatePos) UpdatePos(); else ResetPos();
            transform.position = VRUIController.UICamera.transform.position + LastCamFwd;
        }

        public static void ConvertCanvas(Canvas C)
        {
            if (C.renderMode != RenderMode.ScreenSpaceOverlay || C.renderMode != RenderMode.ScreenSpaceCamera) return;
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

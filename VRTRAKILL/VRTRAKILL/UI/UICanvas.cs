using UnityEngine;
using UnityEngine.UI;

namespace Plugin.VRTRAKILL.UI
{
    // mostly "borrowed" from huskvr
    internal class UICanvas : MonoBehaviour
    {
        private Vector3 LastCamFwd = Vector3.zero;

        private const float Distance = 72f;
        private static float Scale = 0.0625f;

        public void UpdatePos()
        {
            LastCamFwd = VRUIConverter.UICamera.transform.forward * Distance;
            transform.rotation = VRUIConverter.UICamera.transform.rotation;
        }
        public void ResetPos()
        {
            LastCamFwd = new Vector3(LastCamFwd.x, 0f, LastCamFwd.z);
            transform.LookAt(VRUIConverter.UICamera.transform);
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
            transform.position = VRUIConverter.UICamera.transform.position + LastCamFwd;
        }

        public static void RecursiveConvertCanvas(GameObject GO = null)
        {
            if (GO != null)
            {
                try { ConvertCanvas(GO.GetComponent<Canvas>()); } catch {}

                if (GO.transform.childCount > 0)
                    for (int i = 0; i < GO.transform.childCount; i++)
                        RecursiveConvertCanvas(GO.transform.GetChild(i).gameObject);
            }
            else
            {
                foreach (Canvas C in Object.FindObjectsOfType<Canvas>())
                    if (!Helpers.Misc.HasComponent<UICanvas>(C.gameObject))
                        ConvertCanvas(C);
            }
        }

        private static void ConvertCanvas(Canvas C)
        {
            if (C.renderMode != RenderMode.ScreenSpaceOverlay) return;

            C.worldCamera = VRUIConverter.UICamera;
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

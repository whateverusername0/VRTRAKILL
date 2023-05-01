using Plugin.Helpers;
using Plugin.VRTRAKILL.VRPlayer.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Plugin.VRTRAKILL.UI
{
    // mostly "borrowed" from huskvr
    internal class UIConverter
    {
        public static Camera UICamera { get; private set; }

        public static void Init()
        => SceneManager.activeSceneChanged += (x, y) => SceneChanged(y);

        private static void SceneChanged(Scene S)
        {
            UICamera = new GameObject("VRUI Camera").AddComponent<Camera>();
            UICamera.cullingMask = LayerMask.GetMask(new string[] { "UI" });
            UICamera.clearFlags = CameraClearFlags.Depth; UICamera.depth = 1f;
            UICamera.gameObject.AddComponent<GazeUIInteraction>();

            foreach (Canvas C in Object.FindObjectsOfType<Canvas>())
                if (!Helpers.Misc.HasComponent<UICanvas>(C.gameObject))
                    RecursiveConvertCanvas();
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
                        try { ConvertCanvas(C); } catch {}
            }
        }
        public static void ConvertCanvas(Canvas C, bool Force = false, bool DontAddComponent = false)
        {
            if (!Force)
            {
                if (C.renderMode != RenderMode.ScreenSpaceOverlay) return;
            }

            C.worldCamera = UIConverter.UICamera;
            C.renderMode = RenderMode.WorldSpace;
            C.gameObject.layer = 5; // ui
            if (!DontAddComponent) C.gameObject.AddComponent<UICanvas>();

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

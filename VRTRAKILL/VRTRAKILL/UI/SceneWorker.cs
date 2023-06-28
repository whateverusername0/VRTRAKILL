using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.UI
{
    // mostly "borrowed" from huskvr
    internal class SceneWorker
    {
        public static Camera UICamera { get; private set; }

        public static void Init() => SceneManager.activeSceneChanged += (x, y) => SceneChanged(y);

        private static void SceneChanged(Scene S)
        {
            UICamera = new GameObject("UI Camera").AddComponent<Camera>();
            UICamera.transform.parent = Vars.VRCameraContainer.transform;
            UICamera.cullingMask = (int)Vars.Layers.CustomUI;
            UICamera.clearFlags = CameraClearFlags.Depth; UICamera.depth = 1f;

            if (!Vars.Config.Controllers.UseControllerUIInteraction)
                UICamera.gameObject.AddComponent<UIInteraction>();

            Vars.UICamera = UICamera;

            if (!MonoSingleton<CanvasController>.Instance.gameObject.HasComponent<VRUIController>())
                MonoSingleton<CanvasController>.Instance.gameObject.AddComponent<VRUIController>();
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
                    if (C.gameObject.layer != (int)Vars.Layers.CustomUI)
                        try { ConvertCanvas(C); } catch {}
            }
        }
        public static void ConvertCanvas(Canvas C)
        {
            C.worldCamera = UICamera;
            C.renderMode = RenderMode.WorldSpace;
            C.gameObject.layer = (int)Vars.Layers.CustomUI;

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

using Plugin.VRTRAKILL.VRPlayer.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Plugin.VRTRAKILL.UI
{
    // mostly "borrowed" from huskvr
    internal class UIConverter
    {
        public static Camera UICamera { get; private set; }
        public static Camera UIEventCamera { get; private set; }

        public static void ConvertAllCanvases()
        {
            UICamera = new GameObject("UI Camera").AddComponent<Camera>();
            UICamera.cullingMask = 1 << (int)Layers.UI;
            UICamera.clearFlags = CameraClearFlags.Depth; UICamera.depth = 1;

            if (Vars.Config.UIInteraction.ControllerBased)
            {
                // make this camera almost useless because it's only use is to give me the middle POINT.
                UIEventCamera = new GameObject("UI Event Camera").AddComponent<Camera>();
                UIEventCamera.enabled = false;
                UIEventCamera.stereoTargetEye = StereoTargetEyeMask.None;
                UIEventCamera.clearFlags = CameraClearFlags.Nothing;
                UIEventCamera.depth = 99;
                UIEventCamera.fieldOfView = 1;
                UIEventCamera.nearClipPlane = .01f;
                UIEventCamera.cullingMask = -1;

                UIEventCamera.transform.parent = Vars.NonDominantHand.transform;
            }
            else UIEventCamera = UICamera;
            UIEventCamera.gameObject.AddComponent<UIInteraction>();

            foreach (Canvas C in Object.FindObjectsOfType<Canvas>())
                if (!Util.Misc.HasComponent<UICanvas>(C.gameObject))
                    RecursiveConvertCanvas();
        }

        private static void RecursiveConvertCanvas(GameObject GO = null)
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
                    if (!Util.Misc.HasComponent<UICanvas>(C.gameObject))
                        try { ConvertCanvas(C); } catch {}
            }
        }
        public static void ConvertCanvas(Canvas C, bool Force = false, bool DontAddComponent = false)
        {
            if (!Force && C.renderMode != RenderMode.ScreenSpaceOverlay) return;

            C.worldCamera = UIEventCamera;
            C.renderMode = RenderMode.WorldSpace;
            C.gameObject.layer = (int)Layers.UI;
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

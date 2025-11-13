using VRTRAKILL.Data;
using UnityEngine;
using UnityEngine.UI;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Systems.UI;

internal static class VRUIConverter
{
    public static Camera UICamera { get; private set; }
    public static Camera UIEventCamera { get; private set; }

    public static void ConvertAllCanvases()
    {
        UICamera = new GameObject("UI Camera").AddComponent<Camera>();
        UICamera.cullingMask = 1 << (int)Layers.UI;
        UICamera.clearFlags = CameraClearFlags.Depth; UICamera.depth = 1;

        // make this camera almost useless because it's only use is to give me the middle POINT.
        UIEventCamera = new GameObject("UI Event Camera").AddComponent<Camera>();
        UIEventCamera.enabled = false;

        UIEventCamera.gameObject.AddComponent<VRUIInteraction>();

        foreach (Canvas C in Resources.FindObjectsOfTypeAll<Canvas>())
            if (!C.gameObject.HasComponent<VRUICanvas>())
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
                if (!C.gameObject.HasComponent<VRUICanvas>())
                    try { ConvertCanvas(C); } catch {}
        }
    }

    // turns all screen/camera space canvases to world space
    public static void ConvertCanvas(Canvas c, bool force = false, bool addComponent = true)
    {
        c.worldCamera = UIEventCamera;
        if (!force && c.renderMode != RenderMode.ScreenSpaceOverlay) return;
        c.renderMode = RenderMode.WorldSpace;
        c.gameObject.layer = (int)Layers.UI;
        if (addComponent) c.gameObject.AddComponent<VRUICanvas>();

        foreach (Transform Child in c.transform) ConvertElement(Child);
    }

    // paints the buttons a different color
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

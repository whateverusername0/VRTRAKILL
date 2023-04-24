using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr
    internal class VRUIConverter
    {
        public static Camera UICamera { get; private set; }

        public static void Init()
        => SceneManager.activeSceneChanged += (x, y) => SceneChanged(y);

        private static void SceneChanged(Scene S)
        {
            UICamera = new GameObject("UI Camera").AddComponent<Camera>();
            UICamera.cullingMask = LayerMask.GetMask(new string[] { "UI" });
            UICamera.clearFlags = CameraClearFlags.Depth;
            UICamera.depth = 1f;
            UICamera.gameObject.AddComponent<Helpers.GazeUIInteraction>();

            foreach (Canvas C in Object.FindObjectsOfType<Canvas>())
                if (!Helpers.Misc.HasComponent<UICanvas>(C.gameObject))
                    UICanvas.RecursiveConvertCanvas();
        }
    }
}

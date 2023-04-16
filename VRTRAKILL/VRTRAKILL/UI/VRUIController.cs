using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr
    internal class VRUIController
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
            UICamera.gameObject.AddComponent<GazeUIInteraction>();

            foreach (Canvas C in Object.FindObjectsOfType<Canvas>())
                UICanvas.ConvertCanvas(C);
        }
    }
}

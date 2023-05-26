using UnityEngine;
using Plugin.Helpers;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    // lol the name
    internal class ControllerController : MonoBehaviour
    {
        public GameObject Offset = new GameObject("Offset");
        private GameObject Pointer;
        LineRenderer LR; Vector3 EndPosition;
        public float DefaultLength => Vars.Config.VRSettings.VRUI.CrosshairDistance;

        private void SetupControllerPointer()
        {
            Pointer = new GameObject("Canvas Pointer");
            Pointer.layer = (int)Vars.Layers.IgnoreRaycast;
            Pointer.transform.parent = Offset.transform;
            Pointer.AddComponent<UI.UIInteraction>();

            Camera PointerCamera = Pointer.AddComponent<Camera>();
            PointerCamera.stereoTargetEye = StereoTargetEyeMask.None;
            PointerCamera.clearFlags = CameraClearFlags.Nothing;
            PointerCamera.cullingMask = -1; // Nothing
            PointerCamera.nearClipPlane = .01f;
            PointerCamera.fieldOfView = 1; // haha, ha, 1!
            PointerCamera.enabled = false;
        }
        private void SetupControllerLines()
        {
            LR = Pointer.AddComponent<LineRenderer>();
            LR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            LR.receiveShadows = false;
            LR.allowOcclusionWhenDynamic = false;
            LR.useWorldSpace = true;
            LR.material = new Material(Shader.Find("GUI/Text Shader"));

            Color C1 = new Color(1, 1, 1, Vars.Config.VRSettings.CL.LInitTransparency),
                  C2 = new Color(1, 1, 1, Vars.Config.VRSettings.CL.LEndTransparency);

            LR.startWidth = 0.02f; LR.endWidth = 0.001f;
            LR.startColor = C1; LR.endColor = C2;
        }

        private void DrawControllerLines()
        {
            // It's broken AGAIN!! :(
            if (Vars.IsAMenu) LR.enabled = true;
            else LR.enabled = false;

            if (LR.enabled)
            {
                LR.SetPosition(0, transform.position);
                LR.SetPosition(1, EndPosition);
            }
        }

        public void Start()
        {
            Offset.layer = (int)Vars.Layers.IgnoreRaycast;
            Offset.transform.parent = this.transform;
            Offset.transform.localPosition = Vector3.zero;
            Offset.transform.localRotation = Quaternion.Euler(45, 0, 0);

            if (Vars.Config.VRSettings.CL.DrawControllerLines)
            {
                SetupControllerPointer();
                SetupControllerLines();
            }
        }
        public void Update()
        {
            RaycastHit Hit = transform.ForwardRaycast(DefaultLength);
            EndPosition = transform.position + (Offset.transform.forward * DefaultLength);
            if (Hit.collider != null) EndPosition = Hit.point;

            if (Vars.Config.VRSettings.CL.DrawControllerLines)
                DrawControllerLines();
        }

        public static Vector3 ControllerOffset = new Vector3(0, 2.85f, 0);
        public static void onTransformUpdatedH(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
        => fromAction.transform.position += ControllerOffset;
    }
}

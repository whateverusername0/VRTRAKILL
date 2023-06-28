using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    // lol the name
    internal class ControllerController : MonoBehaviour
    {
        private SteamVR_RenderModel[] SVRRM;
        public GameObject Offset = new GameObject("Offset");
        private GameObject Pointer;
        LineRenderer LR; Vector3 EndPosition;
        public float DefaultLength => Vars.Config.View.VRUI.CrosshairDistance;

        private void SetupControllerPointer()
        {
            GameObject ControllerClone = Instantiate(gameObject);
            // prevent zipbombing the entire thing
            Object.Destroy(ControllerClone.GetComponent<ControllerController>());
            Object.Destroy(ControllerClone.GetComponentInChildren<SteamVR_RenderModel>());

            // Create a real pointer with the camera for ui interaction
            GameObject RealPointer = new GameObject("Real Canvas Pointer") { layer = (int)Vars.Layers.CustomUI };
            RealPointer.transform.parent = ControllerClone.transform.GetChild(1);

            Camera PointerCamera = RealPointer.AddComponent<Camera>();
            PointerCamera.stereoTargetEye = StereoTargetEyeMask.None;
            PointerCamera.clearFlags = CameraClearFlags.Nothing;
            PointerCamera.cullingMask = -1; // Nothing
            PointerCamera.nearClipPlane = .01f;
            PointerCamera.fieldOfView = 1; // haha, ha, 1!
            PointerCamera.enabled = false;

            RealPointer.AddComponent<UI.UIInteraction>();
        }
        private void SetupControllerLines()
        {
            // Then create a fake pointer w/ the line renderer
            Pointer = new GameObject("Canvas Pointer") { layer = (int)Vars.Layers.Default };
            Pointer.transform.parent = Offset.transform;

            LR = Pointer.AddComponent<LineRenderer>();
            LR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            LR.receiveShadows = false;
            LR.allowOcclusionWhenDynamic = false;
            LR.useWorldSpace = true;
            LR.material = new Material(Shader.Find("GUI/Text Shader"));

            Color C1 = new Color(1, 1, 1, Vars.Config.Controllers.CLines.LInitTransparency),
                  C2 = new Color(1, 1, 1, Vars.Config.Controllers.CLines.LEndTransparency);

            LR.startWidth = 0.02f; LR.endWidth = 0.001f;
            LR.startColor = C1; LR.endColor = C2;
        }

        private void DrawControllerLines()
        {
            if (Vars.IsAMenu || Vars.IsPlayerUsingShop) LR.enabled = true;
            else LR.enabled = false;

            if (LR.enabled)
            {
                LR.SetPosition(0, Offset.transform.position);
                LR.SetPosition(1, EndPosition);
            }
        }

        public void Start()
        {
            SVRRM = GetComponentsInChildren<SteamVR_RenderModel>();
            Offset.layer = (int)Vars.Layers.IgnoreRaycast;
            Offset.transform.parent = this.transform;
            Offset.transform.localPosition = Vector3.zero;
            Offset.transform.localRotation = Quaternion.Euler(45, 0, 0);

            if (Vars.Config.Controllers.UseControllerUIInteraction) SetupControllerPointer();
            if (Vars.Config.Controllers.CLines.DrawControllerLines) SetupControllerLines();
        }
        public void Update()
        {
            if (Vars.IsMainMenu || Vars.IsIntro || Vars.IsRankingScreenPresent)
                foreach (SteamVR_RenderModel SVRRRM in SVRRM) try { SVRRRM.gameObject.SetActive(true); } catch {}
            else foreach (SteamVR_RenderModel SVRRRM in SVRRM) try { SVRRRM.gameObject.SetActive(false); } catch {}

            bool Raycast = Physics.Raycast(Offset.transform.position, Offset.transform.forward,
                                           out RaycastHit Hit, float.PositiveInfinity, (int)Vars.Layers.CustomUI);
            EndPosition = Offset.transform.position + (Offset.transform.forward * DefaultLength);
            if (Raycast) EndPosition = Hit.point;

            if (Vars.Config.Controllers.CLines.DrawControllerLines) DrawControllerLines();
        }

        public static Vector3 ControllerOffset = new Vector3(0, 2.85f, 0);
        public static void onTransformUpdatedH(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
        => fromAction.transform.position += ControllerOffset;
    }
}

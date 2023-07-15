using UnityEngine;
using UnityEngine.Animations;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    // lol the name
    internal class ControllerController : MonoBehaviour
    {
        private SteamVR_RenderModel[] SVRRM;
        public GameObject GunOffset = new GameObject("Gun Offset") { layer = (int)Vars.Layers.IgnoreRaycast };
        public GameObject ArmOffset = new GameObject("Arm Offset") { layer = (int)Vars.Layers.IgnoreRaycast };

        GameObject Pointer;
        LineRenderer LR; Vector3 EndPosition;
        public float DefaultLength => Vars.Config.View.VRUI.CrosshairDistance;

        private PositionConstraint PC;

        private void SetupOffsets()
        {
            GunOffset.transform.parent = transform;
            GunOffset.transform.localPosition = Vector3.zero;
            GunOffset.transform.localRotation = Quaternion.Euler(45, 0, 0);

            ArmOffset.transform.parent = transform;
        }

        private void SetupControllerPointer()
        {
            // Create a real pointer with the camera for ui interaction
            Pointer = new GameObject("Canvas Pointer") { layer = (int)Vars.Layers.UI };
            Pointer.transform.parent = GunOffset.transform;

            Camera PointerCamera = Pointer.AddComponent<Camera>();
            PointerCamera.stereoTargetEye = StereoTargetEyeMask.None;
            PointerCamera.clearFlags = CameraClearFlags.Nothing;
            PointerCamera.cullingMask = -1; // Nothing
            PointerCamera.nearClipPlane = .01f;
            PointerCamera.fieldOfView = 1; // haha, ha, 1!
            PointerCamera.enabled = false;

            Pointer.AddComponent<UI.UIInteraction>();
        }
        private void SetupControllerLines()
        {
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
            if (Vars.IsAMenu || Vars.IsPaused || Vars.IsPlayerUsingShop) LR.enabled = true;
            else LR.enabled = false;

            if (LR.enabled)
            {
                LR.SetPosition(0, GunOffset.transform.position);
                LR.SetPosition(1, EndPosition);
            }
        }

        public void Start()
        {
            SVRRM = GetComponentsInChildren<SteamVR_RenderModel>();

            SetupOffsets();

            if (Vars.Config.Controllers.UseControllerUIInteraction) SetupControllerPointer();
            if (Vars.Config.Controllers.CLines.DrawControllerLines) SetupControllerLines();
        }
        public void Update()
        {
            if ((Vars.IsMainMenu || Vars.IsIntro || Vars.IsRankingScreenPresent)
            && !Vars.Config.Game.VRB.EnableVRIK)
                foreach (SteamVR_RenderModel SVRRRM in SVRRM) try { SVRRRM.gameObject.SetActive(true); } catch {}
            else foreach (SteamVR_RenderModel SVRRRM in SVRRM) try { SVRRRM.gameObject.SetActive(false); } catch {}

            bool Raycast = Physics.Raycast(GunOffset.transform.position, GunOffset.transform.forward,
                                           out RaycastHit Hit, float.PositiveInfinity, (int)Vars.Layers.UI);
            EndPosition = GunOffset.transform.position + (GunOffset.transform.forward * DefaultLength);
            if (Raycast) EndPosition = Hit.point;

            if (Vars.Config.Controllers.CLines.DrawControllerLines) DrawControllerLines();
        }

        public static Vector3 ControllerOffset = new Vector3(0, 2.85f, 0);
        public static void onTransformUpdatedH(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
        => fromAction.transform.position += ControllerOffset;
    }
}

using System.Linq;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    public enum SCMode
    {
        Follow = 0,
        RotateAround = 1,
        Fixed = 2
    }
    internal class SpectatorCamera : MonoSingleton<SpectatorCamera>
    {
        public SCMode Mode = SCMode.Follow;

        public Transform FollowTarget;
        public Camera SPCam;

        public Vector3 OffsetPos = new Vector3(0, 1, -3);
        public Vector3 RotAngles = new Vector3(0, 180, 0);
        public readonly float RAMRotationSpeed = .2f, FMDuration = 2;
        public float MoveRotateSpeed = .5f;

        private Vector3 PrevPos;
        private readonly float BackupCap = .2f;

        public override void OnEnable()
        {
            base.OnEnable();
            SPCam = GetComponentInChildren<Camera>();
            SPCam.transform.localPosition = OffsetPos;
            SPCam.transform.eulerAngles = RotAngles;
        }

        public void Update()
        {
            transform.position = FollowTarget.position;
            SPCam.transform.localPosition = OffsetPos;
            Util.Misc.CopyCameraValues(SPCam, Vars.DesktopCamera);

            // Pushback (from https://metaanomie.blogspot.com/2020/04/unity-vr-head-blocking-steam-vr-v2.html)
            if (Util.Misc.DetectCollisions(SPCam.transform.position, 2, (int)Layers.Environment) > 0)
            {
                Vector3 Difference = SPCam.transform.position - PrevPos;
                if (Mathf.Abs(Difference.x) > BackupCap)
                {
                    if (Difference.x > 0) Difference.x = BackupCap;
                    else Difference.x = BackupCap * -1;
                }
                if (Mathf.Abs(Difference.z) > BackupCap)
                {
                    if (Difference.z > 0) Difference.z = BackupCap;
                    else Difference.z = BackupCap * -1;
                }
                Vector3 AdjustedHeadPos = new Vector3(SPCam.transform.position.x - Difference.x,
                                                      SPCam.transform.position.y,
                                                      SPCam.transform.position.z - Difference.z);
                SPCam.transform.SetPositionAndRotation(AdjustedHeadPos, SPCam.transform.rotation);
            }
            else
            {
                PrevPos = SPCam.transform.position;
                switch (Mode)
                {
                    case SCMode.Follow: Follow(); break;
                    case SCMode.RotateAround: RotateAround(); break;
                    case SCMode.Fixed: transform.eulerAngles = RotAngles; break;
                    default: break;
                }

                if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamLeft))  MoveOrRotate(Vector2.left);
                if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamUp))    MoveOrRotate(Vector2.up);
                if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamRight)) MoveOrRotate(Vector2.right);
                if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamDown))  MoveOrRotate(Vector2.down);
            }
        }

        public void MoveOrRotate(Vector2 V)
        {
            if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamHoldMoveMode))
            {
                if (V == Vector2.left)       OffsetPos += new Vector3(MoveRotateSpeed, 0, 0);
                else if (V == Vector2.up)    OffsetPos += new Vector3(0, 0, MoveRotateSpeed);
                else if (V == Vector2.right) OffsetPos += new Vector3(-MoveRotateSpeed, 0, 0);
                else if (V == Vector2.down)  OffsetPos += new Vector3(0, 0, -MoveRotateSpeed);
            }
            else
            {
                if (V == Vector2.left)       RotAngles += new Vector3(0, 0, MoveRotateSpeed);
                else if (V == Vector2.up)    RotAngles += new Vector3(0, MoveRotateSpeed, 0);
                else if (V == Vector2.right) RotAngles += new Vector3(0, 0, -MoveRotateSpeed);
                else if (V == Vector2.down)  RotAngles += new Vector3(0, -MoveRotateSpeed, 0);
            }
        }

        private void Follow()
        { transform.forward = Vector3.Lerp(transform.forward, FollowTarget.forward, Time.deltaTime * FMDuration); }
        private void RotateAround()
        { transform.Rotate(new Vector3(0, RAMRotationSpeed, 0)); }
        
        public void EnumSCMode() // ugly
        {
            transform.eulerAngles = RotAngles; Mode++;
            if ((int)Mode > System.Enum.GetValues(typeof(SCMode)).Cast<int>().Max()) Mode = 0;
        }
    }
}

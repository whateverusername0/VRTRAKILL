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
    internal class ThirdPersonCamera : MonoSingleton<ThirdPersonCamera>
    {
        public SCMode Mode = SCMode.Follow;

        public Transform FollowTarget;
        public Transform Offset;
        public Rigidbody RB; public Camera SPCam;

        public Vector3 OffsetPos = new Vector3(0, 1, -3);
        public Vector3 RotAngles = new Vector3(0, 0, 0);
        public readonly float RAMRotationSpeed = .2f, FMDuration = 2;
        public float MoveRotateSpeed = .5f;

        public override void OnEnable()
        {
            base.OnEnable();
            SPCam = GetComponentInChildren<Camera>();
            Offset.localPosition = OffsetPos;
            SPCam.transform.rotation = Quaternion.Euler(RotAngles);
        }

        public void Update()
        {
            Util.Unity.CopyCameraValues(SPCam, Vars.DesktopCamera);
            transform.position = FollowTarget.position;

            switch (Mode)
            {
                case SCMode.Follow: Follow(); break;
                case SCMode.RotateAround: RotateAround(); break;
                case SCMode.Fixed: transform.eulerAngles = RotAngles; break;
                default: break;
            }

            //if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamLeft))  MoveOrRotate(Vector2.left);
            //if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamUp))    MoveOrRotate(Vector2.up);
            //if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamRight)) MoveOrRotate(Vector2.right);
            //if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.SpecCamDown))  MoveOrRotate(Vector2.down);
        }
        public void LateUpdate()
        {
            RB.velocity = Vector3.zero; RB.angularVelocity = Vector3.zero;
            RB.position = Vector3.Lerp(RB.position, Offset.TransformPoint(Offset.localPosition), Time.deltaTime);
        }

        public void MoveOrRotate(Vector2 V)
        {
            if (UnityEngine.Input.GetKey((KeyCode)Config.ConfigMaster.SpecCamHoldMoveMode))
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

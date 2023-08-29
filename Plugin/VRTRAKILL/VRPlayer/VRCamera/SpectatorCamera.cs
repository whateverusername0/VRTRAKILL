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

        public Camera SPCam;

        public Vector3 OffsetPos = new Vector3(0, 1, -3);
        public float RAMRotationSpeed = .2f, FMDuration = 2;
        public Vector3 RotAngles = new Vector3(0, 180, 0);

        private Vector3 PrevPos;
        private readonly float BackupCap = .2f;

        private int DetectHit(Vector3 Pos)
        {
            int Hits = 0;
            Collider[] Things = Physics.OverlapSphere(Pos, BackupCap, 1 << (int)Layers.Environment, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < Things.Length; i++) Hits++;
            return Hits;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            SPCam = GetComponentInChildren<Camera>();
            SPCam.transform.localPosition = OffsetPos;
            SPCam.transform.eulerAngles = RotAngles;
        }

        public void Update()
        {
            transform.position = Vars.MainCamera.transform.position;
            Util.Misc.CopyCameraValues(SPCam, Vars.DesktopCamera);

            switch (Mode)
            {
                case SCMode.Follow: Follow(); break;
                case SCMode.RotateAround: RotateAround(); break;
                case SCMode.Fixed: transform.eulerAngles = Vector3.zero; break;
                default: break;
            }

            // Pushback (from https://metaanomie.blogspot.com/2020/04/unity-vr-head-blocking-steam-vr-v2.html)
            int Hits = DetectHit(transform.position);
            if (Hits == 0) PrevPos = transform.position;
            else
            {
                Vector3 Difference = transform.position - PrevPos;
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
        }

        private void Follow()
        { transform.forward = Vector3.Lerp(transform.forward, Vars.MainCamera.transform.forward, Time.deltaTime * FMDuration); }
        private void RotateAround()
        { transform.Rotate(new Vector3(0, RAMRotationSpeed, 0)); }
        
        public void EnumSCMode() // ugly
        {
            transform.eulerAngles = RotAngles; Mode++;
            if ((int)Mode > System.Enum.GetValues(typeof(SCMode)).Cast<int>().Max()) Mode = 0;
        }
    }
}

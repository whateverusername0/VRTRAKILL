using System.Linq;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera
{
    internal class SpectatorCamera : MonoSingleton<SpectatorCamera>
    {
        public enum SCMode
        {
            Follow = 0,
            RotateAround = 1,
            Fixed = 2
        };
        public SCMode Mode = SCMode.Follow;

        public Vector3 OffsetPos = new Vector3(0, 0, -1);
        public float RAMRotationSpeed = 2;
        public Vector3 RotAngles = new Vector3(20, 0, 0);

        public void Update()
        {
            GetComponentInChildren<Camera>().transform.localPosition = OffsetPos;
            Helpers.Misc.CopyCameraValues(GetComponentInChildren<Camera>(), Vars.DesktopCamera);

            switch (Mode)
            {
                case SCMode.Follow: Follow(); break;
                case SCMode.RotateAround: RotateAround(); break;
                case SCMode.Fixed: transform.eulerAngles = RotAngles; break;
                default: break;
            }
        }

        private void Follow()
        {
            transform.forward =
                Vector3.Lerp(transform.forward,
                             new Vector3(Vars.MainCamera.transform.forward.x, Vars.MainCamera.transform.forward.y, 0),
                             Time.deltaTime);
        }
        private void RotateAround()
        { transform.eulerAngles = RotAngles + new Vector3(0, RAMRotationSpeed * Time.deltaTime, 0); }
        
        public void EnumSCMode() // ugly
        {
            Mode++;
            if ((int)Mode > System.Enum.GetValues(typeof(SCMode)).Cast<int>().Max())
                Mode = 0;
        }
    }
}

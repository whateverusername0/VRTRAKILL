using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    internal class VRIKController : MonoSingleton<VRIKController>
    {
        public MetaRig Rig;

        public Vector3 BodyPosOffset = new Vector3(0, 0, 0),
                       HeadPosOffset = new Vector3(0, 0, 0);

        public Vector3 BodyRotOffset = new Vector3(0, 0, 0),
                       HeadRotOffset = new Vector3(0, 0, 0);

        public void Start()
        {
            // For now remove unnecessary stuff
            Rig.LeftArm.GameObjectT.localScale = Vector3.zero;
            Rig.RightArm.GameObjectT.localScale = Vector3.zero;
            Rig.LeftLeg.GameObjectT.localScale = Vector3.zero;
            Rig.RightLeg.GameObjectT.localScale = Vector3.zero;
        }

        public void LateUpdate()
        {
            if (Rig == null) return;

            // body thingamajiggery
            
            if ((Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Body.rotation.eulerAngles.y) >= Quaternion.Euler(0, 85, 0).y)
                Rig.Body.rotation *= Quaternion.Euler(0, 1, 0);
            else if ((Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Body.rotation.eulerAngles.y) <= Quaternion.Euler(0, -85, 0).y)
                Rig.Body.rotation *= Quaternion.Euler(0, -1, 0);


            // other thingamajiggery
            Rig.Head.rotation = Vars.MainCamera.transform.rotation * Quaternion.Euler(HeadRotOffset);
        }
    }
}

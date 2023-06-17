using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns
{
    internal class VRGunsController : MonoBehaviour
    {
        public void Update()
        {
            transform.position = Vars.DominantHand.transform.position;
            transform.rotation = Vars.DominantHand.transform.rotation;
        }
    }
}

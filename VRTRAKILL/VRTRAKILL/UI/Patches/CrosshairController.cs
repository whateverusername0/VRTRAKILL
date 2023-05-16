using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    internal class CrosshairController : MonoBehaviour
    {
        public int Hand;
        // 0 - left, 1 - right
        public void Update()
        {
            if (Hand == 0)
            {
                transform.position = Vars.LeftController.transform.position
                                   + (Vars.LeftController.transform.forward * Vars.Config.VRSettings.VRUI.CrosshairDistance);
                transform.rotation = Vars.LeftController.transform.rotation;
            }
            if (Hand == 1)
            {
                transform.position = Vars.RightController.transform.position
                                   + (Vars.RightController.transform.forward * Vars.Config.VRSettings.VRUI.CrosshairDistance);
                transform.rotation = Vars.RightController.transform.rotation;
            }
        }
    }
}

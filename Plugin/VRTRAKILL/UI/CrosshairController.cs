using UnityEngine;
using Plugin.Util;

namespace Plugin.VRTRAKILL.UI
{
    internal class CrosshairController : MonoBehaviour
    {
        public float Length => Vars.Config.CBS.CrosshairDistance;

        Vector3 Offset = new Vector3(-.2f, -2.75f, 0);
        Transform Target;

        public void LateUpdate()
        {
            if ((Vars.IsPlayerFrozen || Vars.IsPlayerUsingShop) && !Vars.Config.UIInteraction.ControllerBased)
                transform.position = Vars.MainCamera.transform.position + (Vars.MainCamera.transform.forward * Length * .25f) + Offset;
            else
            {
                if (GunControl.Instance != null && GunControl.Instance.currentWeapon.HasComponent<RocketLauncher>())
                    Target = GunControl.Instance.currentWeapon.transform;
                else Target = Vars.DominantHand.transform;

                transform.position = Target.position + (Target.forward * Length) + Offset;
                transform.rotation = Target.rotation;
            }
        }
    }
}

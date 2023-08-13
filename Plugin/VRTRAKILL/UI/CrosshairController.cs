using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.UI
{
    internal class CrosshairController : MonoBehaviour
    {
        public float Length => Vars.Config.CBS.CrosshairDistance;

        Vector3 Offset = new Vector3(-.2f, -2.75f, 0);

        public void LateUpdate()
        {
            if (Vars.IsPlayerFrozen || Vars.IsPlayerUsingShop)
                transform.position = Vars.MainCamera.transform.forward * Length * .5f;
            else
            {
                if (GunControl.Instance != null)
                    transform.position = GunControl.Instance.currentWeapon.transform.position
                                         + (GunControl.Instance.currentWeapon.transform.forward * Length);
                else transform.position = transform.parent.position + (transform.parent.forward * Length) + Offset;
            }
        }
    }
}

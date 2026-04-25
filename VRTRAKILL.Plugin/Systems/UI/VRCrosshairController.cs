using UnityEngine;
using VRTRAKILL.Data;

namespace VRTRAKILL.Systems.UI;

public class VRCrosshairController : MonoSingleton<VRCrosshairController>
{
    // Distance between the object and the crosshair
    public float Length => GlobalVars.Config.Controllers.CrosshairDistance;

    // a magic number which gives us the most accurate center, presumably.
    private Vector3 _offset = new(-.2f, -2.75f, 0);
    // where crosshair will point from
    private Transform _origin;

    public void LateUpdate()
    {
        if (GlobalVars.IsPlayerFrozen || GlobalVars.IsPlayerUsingShop)
            transform.position = GlobalVars.MainCamera.position + (GlobalVars.MainCamera.forward * Length * .25f) + _offset;
        else
        {
            if (GunControl.Instance != null)
            {
                // if it's an arm cannon then use it's own forward instead of the controller's
                if (GunControl.Instance.currentWeapon?.GetComponent<RocketLauncher>()
                || GunControl.Instance.currentWeapon?.GetComponent<ShotgunHammer>())
                    _origin = GunControl.Instance.currentWeapon.transform;
                else _origin = GunControl.Instance.transform;
            }
            else _origin = GlobalVars.DominantHand;

            transform.position = _origin.position + (_origin.forward * Length) + _offset;
            transform.rotation = _origin.rotation;
        }
    }
}

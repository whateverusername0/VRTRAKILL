using UnityEngine;
using Plugin.Util;

namespace Plugin.VRTRAKILL.UI
{
    /// <summary> Controls the behavior of player's crosshair. </summary>
    internal class CrosshairController : MonoBehaviour
    {
        // Distance between the object and the crosshair
        public float Length => Vars.Config.CBS.CrosshairDistance;

        // A magic number which gives us the most accurate center
        Vector3 Offset = new Vector3(-.2f, -2.75f, 0);
        Transform Target; // Where crosshair will point from

        public void LateUpdate()
        {
            // Doesn't work. (for some reason)
            if ((Vars.IsPlayerFrozen || Vars.IsPlayerUsingShop) && !Vars.Config.UIInteraction.ControllerBased)
                transform.position = Vars.MainCamera.transform.position + (Vars.MainCamera.transform.forward * Length * .25f) + Offset;
            else
            {
                // This otoh works.
                if (GunControl.Instance != null) Target = GunControl.Instance.currentWeapon.transform;
                else Target = Vars.DominantHand.transform;

                transform.position = Target.position + (Target.forward * Length) + Offset;
                transform.rotation = Target.rotation;
            }
        }
    }
}

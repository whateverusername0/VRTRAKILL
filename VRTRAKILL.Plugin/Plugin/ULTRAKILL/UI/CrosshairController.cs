﻿using UnityEngine;
using VRTRAKILL.Utilities;

namespace VRBasePlugin.ULTRAKILL.UI
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

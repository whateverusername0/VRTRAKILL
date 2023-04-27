using UnityEngine;

namespace Plugin.Helpers.InverseKinematics
{
    internal class Joint : MonoBehaviour
    {
        public Joint Child; public Joint GetChild() { return Child; }

        public void Rotate(float Angle)
        {
            transform.Rotate(Vector3.up * Angle);
        } 
    }
}

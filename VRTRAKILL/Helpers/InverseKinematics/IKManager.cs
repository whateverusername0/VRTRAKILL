using UnityEngine;

namespace Plugin.Helpers.InverseKinematics
{
    internal class IKManager : MonoBehaviour
    {
        public Joint Root, End;
        public GameObject Target;

        public float Threshold = 0.05f;

        float CalculateSlope(Joint J)
        {
            float DeltaTheta = 0.1f;

            float Distance1 = GetDistance(End.transform.position, Target.transform.position);
            J.Rotate(DeltaTheta);

            float Distance2 = GetDistance(End.transform.position, Target.transform.position);
            J.Rotate(-DeltaTheta);

            return (Distance2 - Distance1) / DeltaTheta;
        }

        float GetDistance(Vector3 P1, Vector3 P2) { return Vector3.Distance(P1, P2); }

        void Update()
        {
            if (GetDistance(End.transform.position, Target.transform.position) > Threshold)
            {
                float Slope = CalculateSlope(Root);
                Root.Rotate(-Slope);
            }
        }
    }
}

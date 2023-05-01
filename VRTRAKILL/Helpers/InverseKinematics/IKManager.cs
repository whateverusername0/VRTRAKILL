using UnityEngine;

namespace Plugin.Helpers.InverseKinematics
{
    internal class IKManager : MonoBehaviour
    {
        public Joint Root, End;
        public GameObject Target;

        public float Threshold = 0.05f;
        public float Rate = 20;

        float CalculateSlope(Joint J)
        {
            float DeltaTheta = 0.1f;

            float Distance1 = Vector3.Distance(End.transform.position, Target.transform.position);
            J.Rotate(DeltaTheta);

            float Distance2 = Vector3.Distance(End.transform.position, Target.transform.position);
            J.Rotate(-DeltaTheta);

            return (Distance2 - Distance1) / DeltaTheta;
        }

        void Update()
        {
            if (Vector3.Distance(End.transform.position, Target.transform.position) > Threshold)
            {
                Joint Current = Root;
                while (Current != null)
                {
                    float Slope = CalculateSlope(Root);
                    Root.Rotate(-Slope * Rate);
                    Current = Current.GetChild();
                }
            }
        }
    }
}

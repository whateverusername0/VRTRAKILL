using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.VRPlayer.VRAvatar
{
    internal class IKFoot : MonoBehaviour
    {
        // code from https://www.youtube.com/watch?v=1Xr3jB8ik1g

        public Animator Anim;
        public Transform Body;
        public IKFoot OtherFoot;
        public float Speed = 5, StepDistance = .3f, StepLength = .3f, StepHeight = .3f;

        public float? FootSpacing;
        private float Lerp;
        private Vector3 OldPos, CurrentPos, NewPos;

        public int DetectionLayerMask = 1 << (int)Layers.Environment,
                   RaycastDistance = 10;

        public bool IsMoving => Lerp < 1;

        Vector3 LastFootPos = default;

        public void Start()
        {
            DetectionLayerMask |= 1 << (int)Layers.Outdoors;
            if (FootSpacing == null) FootSpacing = Body.InverseTransformPoint(transform.position).x;
            CurrentPos = OldPos = NewPos = transform.position;
            Lerp = 1;
            LastFootPos = transform.localPosition;
        }

        public void Update()
        {
            if (Anim?.GetBool("Jumping") == true || Anim?.GetBool("Sliding") == true) { transform.localPosition = LastFootPos; return; }

            if (Input.InputVars.MoveVector.magnitude > 0)
            {
                Speed = ((Input.InputVars.MoveVector.magnitude * 7.5f) - Speed) > 0 ? Input.InputVars.MoveVector.magnitude * 7.5f : 5;
                StepDistance = Input.InputVars.MoveVector.magnitude * 2.5f;
                StepLength = Input.InputVars.MoveVector.magnitude * 5f;
                StepHeight = Input.InputVars.MoveVector.magnitude * 1.5f;
            }
            else
            {
                StepDistance = .5f;
                StepLength = .3f;
                StepHeight = .25f;
            }

            transform.position = CurrentPos;

            Ray R = new Ray(Body.position + (Body.right * (float)FootSpacing) + (Vector3.up * 2), Vector3.down);
            if (Physics.Raycast(R, out RaycastHit Hit, RaycastDistance, DetectionLayerMask))
            {
                if (Vector3.Distance(NewPos, Hit.point) > StepDistance && !OtherFoot.IsMoving && !IsMoving)
                {
                    Lerp = 0;
                    int Direction = Body.InverseTransformPoint(Hit.point).z > Body.InverseTransformPoint(NewPos).z ? 1 : -1;
                    NewPos = Hit.point + (Body.forward * Direction * StepLength);
                }

                if (IsMoving)
                {
                    Vector3 TempPos = Vector3.Lerp(OldPos, NewPos, Lerp);
                    TempPos.y += Mathf.Sin(Lerp * Mathf.PI) * StepHeight;
                    CurrentPos = TempPos;
                    Lerp += Time.deltaTime * Speed;
                }
                else OldPos = NewPos;
            }
        }
    }
}

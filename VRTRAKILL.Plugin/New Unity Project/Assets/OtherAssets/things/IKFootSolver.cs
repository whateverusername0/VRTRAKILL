using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    // https://www.youtube.com/watch?v=1Xr3jB8ik1g
    // LEFT FootSpacing = .23f;
    // RIGHT FootSpacing = 0;

    public Animator Anim;
    public Transform Body;
    public IKFootSolver OtherFoot;
    public float Speed = 5, StepDistance = .3f, StepLength = .3f, StepHeight = .3f;
    public Vector3 FootPosOffset, FootRotOffset;

    [SerializeField] private float FootSpacing, Lerp;
    private Vector3
        InitPos, OldPos, CurrentPos, NewPos,
        OldNorm, CurrentNorm, NewNorm;

    public bool IsMoving => Lerp < 1;

    private void Start()
    {
        if (FootSpacing == null) FootSpacing = Body.InverseTransformPoint(transform.position).x;
        CurrentPos = OldPos = NewPos = InitPos = transform.position;
        CurrentNorm = OldNorm = NewNorm = transform.up;
        Lerp = 1;
    }

    
    private void Update()
    {
        if (Anim.GetBool("Jumping") == true || Anim.GetBool("Sliding") == true) return;

        transform.position = CurrentPos;

        Ray R = new Ray(Body.position + (Body.right * FootSpacing) + (Vector3.up * 2), Vector3.down);
        Debug.DrawRay(Body.position + (Body.right * FootSpacing) + (Vector3.up * 2), Vector3.down, Color.black, .1f);
        if (Physics.Raycast(R, out RaycastHit Hit, 10, 1 << 0))
        {
            if (Vector3.Distance(NewPos, Hit.point) > StepDistance && !OtherFoot.IsMoving && !IsMoving)
            {
                Lerp = 0;
                int Direction = Body.InverseTransformPoint(Hit.point).z > Body.InverseTransformPoint(NewPos).z ? 1 : -1;
                NewPos = Hit.point + (Body.forward * Direction * StepLength) + FootPosOffset;
                NewNorm = Hit.normal + FootRotOffset;
            }

            if (IsMoving)
            {
                Vector3 TempPos = Vector3.Lerp(OldPos, NewPos, Lerp);
                TempPos.y += Mathf.Sin(Lerp * Mathf.PI) * StepHeight;
                CurrentPos = TempPos;
                CurrentNorm = Vector3.Lerp(OldNorm, NewNorm, Lerp);
                Lerp += Time.deltaTime * Speed;
            }
            else
            {
                OldPos = NewPos;
                OldNorm = NewNorm;
            }
        }
    }
}

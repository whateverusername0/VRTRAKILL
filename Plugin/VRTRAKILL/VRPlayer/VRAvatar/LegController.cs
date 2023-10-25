using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
{
    internal class LegController : MonoBehaviour
    {
        public IKChain Chain;
        public Transform Target, Origin;

        public void Start()
        {
            Chain = GetComponent<IKChain>();
            Chain.enabled = false;
        }
        private Vector3 PrevPos = default;
        public Vector3 Diff;
        public void LateUpdate()
        {
            transform.position = Target.position; transform.rotation = Target.rotation;
            Vector3 InitPos = transform.position; Quaternion InitRot = transform.rotation;

            Diff = Target.position - PrevPos;
            if (Mathf.Abs(Diff.y) > .2f)
            {
                if (Diff.y > 0) Diff.y = .2f;
                else Diff.y = -.2f;
            }

            if (Physics.Raycast(new Ray(Target.parent.position, Vector3.down),
                                out RaycastHit Info, 10, 1 << 0))
            {
                Debug.DrawLine(Target.position, Info.point, Color.black, .1f);
                if (Target.position.y <= Info.point.y)
                {
                    Target.position = new Vector3(Target.position.x, Target.position.y - Diff.y, Target.position.z);
                }
                else { PrevPos = Target.position; }
            }

            InitPos = transform.position; InitRot = transform.rotation;

            Chain.LateUpdate();

            transform.position = InitPos; transform.rotation = InitRot;
        }
    }
}

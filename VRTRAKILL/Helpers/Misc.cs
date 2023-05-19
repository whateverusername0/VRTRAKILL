using UnityEngine;

namespace Plugin.Helpers
{
    static class Misc
    {
        public static bool HasComponent<T>(this GameObject GM) where T : Component
        { return GM.GetComponent<T>() != null; }

        public static void RecursiveChangeLayer(this GameObject GM, LayerMask LM)
        {
            GM.layer = LM;
            if (GM.transform.childCount > 0)
                for (int i = 0; i < GM.transform.childCount; i++)
                    try { RecursiveChangeLayer(GM.transform.GetChild(i).gameObject, LM); } catch {}
        }

        public static RaycastHit ForwardRaycast(this Transform T, float Length, int? Layer = null)
        {
            if (Layer != null) { Physics.Raycast(new Ray(T.position, T.forward), out RaycastHit Hit, Length, (int)Layer); return Hit; }
            else { Physics.Raycast(new Ray(T.position, T.forward), out RaycastHit Hit, Length); return Hit; }
        }
    }
}

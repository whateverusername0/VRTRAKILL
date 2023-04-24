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
    }
}

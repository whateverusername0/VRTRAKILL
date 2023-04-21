using UnityEngine;

namespace Plugin.Helpers
{
    static class Misc
    {
        public static bool HasComponent<T>(this GameObject Flag) where T : Component
        { return Flag.GetComponent<T>() != null; }
    }
}

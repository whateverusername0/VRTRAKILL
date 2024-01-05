using UnityEngine;

namespace VRTRAKILL.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Checks if a <c>Component</c> is present in GM
        /// </summary>
        /// <typeparam name="T"> The <c>Component</c> class (e.g. <c>NewMovement</c>) </typeparam>
        /// <param name="GM"> An instance of a <c>GameObject</c> </param>
        /// <returns></returns>
        public static bool HasComponent<T>(this GameObject GM) where T : Component
        { return GM.GetComponent<T>() != null; }
    }
}

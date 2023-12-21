using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns
{
    /// <summary>
    ///     This right here exists with a sole purpose of now letting the revolver reset it's fucking position
    ///     when alt firing. Fuck you, Hakita.
    /// </summary>
    internal class FuckRevolver : MonoBehaviour
    {
        public Vector3 FuckPosition, FuckRotation, FuckScale;
        public void Update()
        {
            LateUpdate();
        }
        public void LateUpdate()
        {
            transform.localPosition = FuckPosition;
            transform.localRotation = Quaternion.Euler(FuckRotation);
            transform.localScale = FuckScale;
        }
    }
}

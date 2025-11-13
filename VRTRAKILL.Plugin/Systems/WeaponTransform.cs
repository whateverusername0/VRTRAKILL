using UnityEngine;

namespace VRTRAKILL.Systems;

internal static class WeaponTransform
{
    public static void ApplyTransform(ref WeaponPos wPos, Vector3 position = new(), Vector3 euler = new(), Vector3 scale = new())
    {
        if (wPos == null) return;
        if (position != new Vector3())
        {
            wPos.defaultPos = position; wPos.middlePos = position; wPos.currentDefault = position;
            wPos.transform.localPosition = position;
        }
        if (euler != new Vector3())
        {
            wPos.defaultRot = euler; wPos.middleRot = euler;
            wPos.transform.localRotation = Quaternion.Euler(euler);
        }
        if (scale != new Vector3())
        {
            wPos.defaultScale = scale;
            wPos.transform.localScale = scale;
        }
    }

    public static void ApplyTransform(WeaponPos wPos, Vector3 position = new(), Vector3 euler = new(), Vector3 scale = new())
    {
        if (wPos == null) return;
        if (position != new Vector3())
        {
            wPos.defaultPos = position; wPos.middlePos = position;
            wPos.transform.localPosition = position;
        }
        if (euler != new Vector3())
        {
            wPos.defaultRot = euler; wPos.middleRot = euler;
            wPos.transform.localRotation = Quaternion.Euler(euler);
        }
        if (scale != new Vector3())
        {
            wPos.defaultScale = scale;
            wPos.transform.localScale = scale;
        }
    }
}
using UnityEngine;

public static class VectorHelpers
{
	public static float axisAngle(Vector3 v, Vector3 forward, Vector3 axis)
	{
		Vector3 right = Vector3.Cross(axis, forward);
		forward = Vector3.Cross(right, axis);
		return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
	}

	public static float getAngleBetween(Vector3 a, Vector3 b, Vector3 forward, Vector3 axis)
	{
		float angleA = axisAngle(a, forward, axis);
		float angleB = axisAngle(b, forward, axis);

		return Mathf.DeltaAngle(angleA, angleB);
	}
}

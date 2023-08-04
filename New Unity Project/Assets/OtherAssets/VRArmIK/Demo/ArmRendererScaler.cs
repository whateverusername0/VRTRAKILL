using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRendererScaler : MonoBehaviour
{
	public Transform target;

	void Update()
	{
		var collider = GetComponent<Collider>();
		float meshLength = 1.0f;
		if (collider is CapsuleCollider)
		{
			meshLength = (collider as CapsuleCollider).height;
		}

		var scale = transform.localScale;
		var boneLength = (transform.parent.position - target.position).magnitude;
		scale.y = boneLength / meshLength;

		var pos = transform.localPosition;
		pos.x = Mathf.Sign(pos.x) * boneLength * 0.5f;

		transform.localScale = scale;
		transform.localPosition = pos;
	}
}

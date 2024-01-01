using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

namespace VRArmIK
{
	public class LocalVrTrackingInput : MonoBehaviour
	{
		public UnityEngine.XR.XRNode node;

		void Update()
		{
			if (UnityEngine.XR.InputTracking.GetLocalPosition(node).magnitude < 0.000001f)
				return;

			transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(node);
			transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(node);
		}
	}
}
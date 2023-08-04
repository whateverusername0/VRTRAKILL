using UnityEngine;

namespace VRArmIK
{
	public class ArmTransforms : MonoBehaviour
	{
		public Transform upperArm, lowerArm, wrist1, wrist2, hand;

		public float upperArmLength => distance(upperArm, lowerArm);
		public float lowerArmLength => distance(lowerArm, hand);
		public float armLength => upperArmLength + lowerArmLength;

		public bool armLengthByScale = false;
		public Vector3 scaleAxis = Vector3.one;
		public float scaleHandFactor = .7f;

		float distance(Transform a, Transform b) => (a.position - b.position).magnitude;

		void Start()
		{
			PoseManager.Instance.onCalibrate += updateArmLengths;
			updateArmLengths();
		}

		void updateArmLengths()
		{
			var shoulderWidth = (upperArm.position - lowerArm.position).magnitude;
			var _armLength = (PoseManager.Instance.playerWidthWrist - shoulderWidth) / 2f;
			setArmLength(_armLength);
		}

		public void setUpperArmLength(float length)
		{
			if (armLengthByScale)
			{
				float oldLowerArmLength = distance(lowerArm, hand);

				Vector3 newScale = upperArm.localScale - Vector3.Scale(upperArm.localScale, scaleAxis).magnitude * scaleAxis;
				float scaleFactor = Vector3.Scale(upperArm.localScale, scaleAxis).magnitude / upperArmLength * length;
				newScale += scaleAxis * scaleFactor;
				upperArm.localScale = newScale;

				setLowerArmLength(oldLowerArmLength);
			}
			else
			{
				Vector3 pos = lowerArm.localPosition;
				pos.x = Mathf.Sign(pos.x) * length;
				lowerArm.localPosition = pos;
			}
		}

		public void setLowerArmLength(float length)
		{
			if (armLengthByScale)
			{
			}
			else
			{
				Vector3 pos = hand.localPosition;
				pos.x = Mathf.Sign(pos.x) * length;
				hand.localPosition = pos;
			}
		}

		public void setArmLength(float length)
		{
			float upperArmFactor = .48f;
			if (armLengthByScale)
			{
				upperArm.localScale = upperArm.localScale / armLength * length;
				hand.localScale = Vector3.one / (1f - (1f - scaleHandFactor) * (1f - upperArm.localScale.x));
			}
			else
			{
				setUpperArmLength(length * upperArmFactor);
				setLowerArmLength(length * (1f - upperArmFactor));
			}
		}
	}
}
using UnityEngine;

namespace VRArmIK
{
	public class ShoulderTransforms : MonoBehaviour
	{
		public Transform leftShoulder, rightShoulder;
		public Transform leftShoulderRenderer, rightShoulderRenderer;
		public Transform leftShoulderAnchor, rightShoulderAnchor;
		public ArmTransforms leftArmDummy, rightArmDummy;
		public ArmTransforms leftArm, rightArm;

		void Awake()
		{
			if (leftArm == null)
			{
				leftArm = Instantiate(leftArmDummy, leftShoulderAnchor.position, leftShoulderAnchor.rotation, leftShoulderAnchor);
				var armIk = leftArm.GetComponentInChildren<VRArmIK>();
				armIk.shoulder = this;
				armIk.shoulderPoser = GetComponent<ShoulderPoser>();
				armIk.target = armIk.shoulderPoser.avatarTrackingReferences.leftHand.transform;
			}
			if (rightArm == null)
			{
				rightArm = Instantiate(rightArmDummy, leftShoulderAnchor.position, rightShoulderAnchor.rotation, rightShoulderAnchor);
				var armIk = rightArm.GetComponentInChildren<VRArmIK>();
				armIk.shoulder = this;
				armIk.shoulderPoser = GetComponent<ShoulderPoser>();
				armIk.target = armIk.shoulderPoser.avatarTrackingReferences.rightHand.transform;
			}
		}

		void Start()
		{
			setShoulderWidth(PoseManager.Instance.playerWidthShoulders);
		}

		public void setShoulderWidth(float width)
		{
			Vector3 localScale = new Vector3(width * .5f, .05f, .05f);
			Vector3 localPosition = new Vector3(width * .25f, 0f, 0f);

			leftShoulderRenderer.localScale = localScale;
			leftShoulderRenderer.localPosition = -localPosition;

			rightShoulderRenderer.localScale = localScale;
			rightShoulderRenderer.localPosition = localPosition;
		}
	}

}
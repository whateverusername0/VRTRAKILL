using UnityEngine;

namespace VRArmIK
{
	public class ShoulderPoser : MonoBehaviour
	{
		public ShoulderTransforms shoulder;
		public VRTrackingReferences vrTrackingReferences;
		public AvatarVRTrackingReferences avatarTrackingReferences;

		public float headNeckDistance = 0.03f;
		public Vector3 neckShoulderDistance = new Vector3(0f, -.1f, -0.02f);

		public float maxDeltaHeadRotation = 80f;

		[LabelOverride("max forward rotation")]
		public float distinctShoulderRotationLimitForward = 33f;

		[LabelOverride("max backward rotation")]
		public float distinctShoulderRotationLimitBackward = 0f;

		[LabelOverride("max upward rotation")] public float distinctShoulderRotationLimitUpward = 33f;
		public float distinctShoulderRotationMultiplier = 30;

		public float rightRotationStartHeight = 0f;
		public float rightRotationHeightFactor = 142f;
		public float rightRotationHeadRotationFactor = 0.3f;
		public float rightRotationHeadRotationOffset = -20f;

		public Vector3 headNeckDirectionVector = new Vector3(0f, -1f, -.05f);
		public float startShoulderDislocationBefore = 0.005f;

		[Header("Features")] public bool ignoreYPos = true;
		public bool autoDetectHandsBehindHead = true;
		public bool clampRotationToHead = true;
		public bool enableDistinctShoulderRotation = true;
		public bool enableShoulderDislocation = true;


		[Header("Debug")]
		[DisplayOnly]
		[SerializeField]
		bool handsBehindHead = false;

		[DisplayOnly] [SerializeField] bool clampingHeadRotation = false;
#pragma warning disable 414
		[DisplayOnly] [SerializeField] bool shoulderDislocated = false;
#pragma warning restore 414
		public float shoulderRightRotation;


		Vector3 lastAngle = Vector3.zero;


		Vector3 leftShoulderAnkerStartLocalPosition, rightShoulderAnkerStartLocalPosition;

		void Start()
		{
			if (vrTrackingReferences == null)
				vrTrackingReferences = PoseManager.Instance.vrTransforms;

			leftShoulderAnkerStartLocalPosition = shoulder.transform.InverseTransformPoint(shoulder.leftShoulderAnchor.position);
			rightShoulderAnkerStartLocalPosition =
				shoulder.transform.InverseTransformPoint(shoulder.rightShoulderAnchor.position);
		}

		void onCalibrate()
		{
			shoulder.leftArm.setArmLength((avatarTrackingReferences.leftHand.transform.position - shoulder.leftShoulderAnchor.position)
				.magnitude);
			shoulder.rightArm.setArmLength((avatarTrackingReferences.rightHand.transform.position - shoulder.rightShoulderAnchor.position)
				.magnitude);
		}

		protected virtual void Update()
		{
			shoulder.transform.rotation = Quaternion.identity;
			positionShoulder();
			rotateShoulderUp();
			rotateShoulderRight();

			if (enableDistinctShoulderRotation)
			{
				rotateLeftShoulder();
				rotateRightShoulder();
			}

			if (enableShoulderDislocation)
			{
				clampShoulderHandDistance();
			}
			else
			{
				shoulder.leftArm.transform.localPosition = Vector3.zero;
				shoulder.rightArm.transform.localPosition = Vector3.zero;
			}

			Debug.DrawRay(shoulder.transform.position, shoulder.transform.forward);
		}

		protected virtual void rotateLeftShoulder()
		{
			rotateShoulderUp(shoulder.leftShoulder, shoulder.leftArm, avatarTrackingReferences.leftHand.transform,
				leftShoulderAnkerStartLocalPosition, 1f);

		}

		protected virtual void rotateRightShoulder()
		{
			rotateShoulderUp(shoulder.rightShoulder, shoulder.rightArm, avatarTrackingReferences.rightHand.transform,
				rightShoulderAnkerStartLocalPosition, -1f);
		}

		void rotateShoulderUp(Transform shoulderSide, ArmTransforms arm, Transform targetHand,
			Vector3 initialShoulderLocalPos, float angleSign)
		{
			Vector3 initialShoulderPos = shoulder.transform.TransformPoint(initialShoulderLocalPos);
			Vector3 handShoulderOffset = targetHand.position - initialShoulderPos;
			float armLength = arm.armLength;

			Vector3 targetAngle = Vector3.zero;

			float forwardDistanceRatio = Vector3.Dot(handShoulderOffset, shoulder.transform.forward) / armLength;
			float upwardDistanceRatio = Vector3.Dot(handShoulderOffset, shoulder.transform.up) / armLength;
			if (forwardDistanceRatio > 0f)
			{
				targetAngle.y = Mathf.Clamp((forwardDistanceRatio - 0.5f) * distinctShoulderRotationMultiplier, 0f,
					distinctShoulderRotationLimitForward);
			}
			else
			{
				targetAngle.y = Mathf.Clamp(-(forwardDistanceRatio + 0.08f) * distinctShoulderRotationMultiplier * 10f,
					-distinctShoulderRotationLimitBackward, 0f);
			}

			targetAngle.z = Mathf.Clamp(-(upwardDistanceRatio - 0.5f) * distinctShoulderRotationMultiplier,
				-distinctShoulderRotationLimitUpward, 0f);

			shoulderSide.localEulerAngles = targetAngle * angleSign;
		}


		void positionShoulder()
		{
			Vector3 headNeckOffset = avatarTrackingReferences.hmd.transform.rotation * headNeckDirectionVector;
			Vector3 targetPosition = avatarTrackingReferences.head.transform.position + headNeckOffset * headNeckDistance;
			shoulder.transform.localPosition =
				shoulder.transform.parent.InverseTransformPoint(targetPosition) + neckShoulderDistance;
		}

		protected virtual void rotateShoulderUp()
		{
			float angle = getCombinedDirectionAngleUp();

			Vector3 targetRotation = new Vector3(0f, angle, 0f);

			if (autoDetectHandsBehindHead)
			{
				detectHandsBehindHead(ref targetRotation);
			}

			if (clampRotationToHead)
			{
				clampHeadRotationDeltaUp(ref targetRotation);
			}

			shoulder.transform.eulerAngles = targetRotation;
		}

		protected virtual void rotateShoulderRight()
		{
			float heightDiff = vrTrackingReferences.hmd.transform.position.y - PoseManager.Instance.vrSystemOffsetHeight;
			float relativeHeightDiff = -heightDiff / PoseManager.Instance.playerHeightHmd;

			float headRightRotation = VectorHelpers.getAngleBetween(shoulder.transform.forward,
										  avatarTrackingReferences.hmd.transform.forward,
										  Vector3.up, shoulder.transform.right) + rightRotationHeadRotationOffset;
			float heightFactor = Mathf.Clamp(relativeHeightDiff - rightRotationStartHeight, 0f, 1f);
			shoulderRightRotation = heightFactor * rightRotationHeightFactor;
			shoulderRightRotation += Mathf.Clamp(headRightRotation * rightRotationHeadRotationFactor * heightFactor, 0f, 50f);

            shoulderRightRotation = Mathf.Clamp(shoulderRightRotation, 0f, 50f);

			Quaternion deltaRot = Quaternion.AngleAxis(shoulderRightRotation, shoulder.transform.right);


			shoulder.transform.rotation = deltaRot * shoulder.transform.rotation;
			positionShoulderRelative();
		}

		protected void positionShoulderRelative()
		{
			Quaternion deltaRot = Quaternion.AngleAxis(shoulderRightRotation, shoulder.transform.right);
			Vector3 shoulderHeadDiff = shoulder.transform.position - avatarTrackingReferences.head.transform.position;
			shoulder.transform.position = deltaRot * shoulderHeadDiff + avatarTrackingReferences.head.transform.position;
		}

		float getCombinedDirectionAngleUp()
		{
			Transform leftHand = avatarTrackingReferences.leftHand.transform, rightHand = avatarTrackingReferences.rightHand.transform;

			Vector3 distanceLeftHand = leftHand.position - shoulder.transform.position,
				distanceRightHand = rightHand.position - shoulder.transform.position;

			if (ignoreYPos)
			{
				distanceLeftHand.y = 0;
				distanceRightHand.y = 0;
			}

			Vector3 directionLeftHand = distanceLeftHand.normalized,
				directionRightHand = distanceRightHand.normalized;

			Vector3 combinedDirection = directionLeftHand + directionRightHand;

			return Mathf.Atan2(combinedDirection.x, combinedDirection.z) * 180f / Mathf.PI;
		}

		void detectHandsBehindHead(ref Vector3 targetRotation)
		{
			float delta = Mathf.Abs(targetRotation.y - lastAngle.y + 360f) % 360f;
			if (delta > 150f && delta < 210f && lastAngle.magnitude > 0.000001f && !clampingHeadRotation)
			{
				handsBehindHead = !handsBehindHead;
			}

			lastAngle = targetRotation;

			if (handsBehindHead)
			{
				targetRotation.y += 180f;
			}
		}

		void clampHeadRotationDeltaUp(ref Vector3 targetRotation)
		{
			float headUpRotation = (avatarTrackingReferences.head.transform.eulerAngles.y + 360f) % 360f;
			float targetUpRotation = (targetRotation.y + 360f) % 360f;

			float delta = headUpRotation - targetUpRotation;

			if (delta > maxDeltaHeadRotation && delta < 180f || delta < -180f && delta >= -360f + maxDeltaHeadRotation)
			{
				targetRotation.y = headUpRotation - maxDeltaHeadRotation;
				clampingHeadRotation = true;
			}
			else if (delta < -maxDeltaHeadRotation && delta > -180 || delta > 180f && delta < 360f - maxDeltaHeadRotation)
			{
				targetRotation.y = headUpRotation + maxDeltaHeadRotation;
				clampingHeadRotation = true;
			}
			else
			{
				clampingHeadRotation = false;
			}
		}

		void clampShoulderHandDistance()
		{
			Vector3 leftHandVector = avatarTrackingReferences.leftHand.transform.position - shoulder.leftShoulderAnchor.position;
			Vector3 rightHandVector = avatarTrackingReferences.rightHand.transform.position - shoulder.rightShoulderAnchor.position;
			float leftShoulderHandDistance = leftHandVector.magnitude, rightShoulderHandDistance = rightHandVector.magnitude;
			shoulderDislocated = false;

			float startBeforeFactor = (1f - startShoulderDislocationBefore);

			if (leftShoulderHandDistance > shoulder.leftArm.armLength * startBeforeFactor)
			{
				shoulderDislocated = true;
				shoulder.leftArm.transform.position = shoulder.leftShoulderAnchor.position +
													  leftHandVector.normalized *
													  (leftShoulderHandDistance - shoulder.leftArm.armLength * startBeforeFactor);
			}
			else
			{
				shoulder.leftArm.transform.localPosition = Vector3.zero;
			}

			if (rightShoulderHandDistance > shoulder.rightArm.armLength * startBeforeFactor)
			{
				shoulderDislocated = true;
				shoulder.rightArm.transform.position = shoulder.rightShoulderAnchor.position +
													   rightHandVector.normalized *
													   (rightShoulderHandDistance -
														shoulder.rightArm.armLength * startBeforeFactor);
			}
			else
			{
				shoulder.rightArm.transform.localPosition = Vector3.zero;
			}
		}
	}
}
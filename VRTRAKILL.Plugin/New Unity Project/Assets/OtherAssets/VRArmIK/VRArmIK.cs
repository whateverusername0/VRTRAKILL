using UnityEngine;
using UnityEngine.UI;

namespace VRArmIK
{

	public class VRArmIK : MonoBehaviour
	{
		[System.Serializable]
		public class ArmIKElbowSettings
		{
			public bool calcElbowAngle = true;
			public bool clampElbowAngle = true;
			public bool softClampElbowAngle = true;
			public float maxAngle = 175f, minAngle = 13f, softClampRange = 10f;
			public float offsetAngle = 135f;
			public float yWeight = -60f;
			public float zWeightTop = 260, zWeightBottom = -100, zBorderY = -.25f, zDistanceStart = .6f;
			public float xWeight = -50f, xDistanceStart = .1f;
		}

		[System.Serializable]
		public class BeforePositioningSettings
		{
			public bool correctElbowOutside = true;
			public float weight = -0.5f;
			public float startBelowZ = .4f;
			public float startAboveY = 0.1f;
		}

		[System.Serializable]
		public class ElbowCorrectionSettings
		{
			public bool useFixedElbowWhenNearShoulder = true;
			public float startBelowDistance = .5f;
			public float startBelowY = 0.1f;
			public float weight = 2f;
			public Vector3 localElbowPos = new Vector3(0.3f, -1f, -2f);
		}

		[System.Serializable]
		public class HandSettings
		{
			public bool useWristRotation = true;
			public bool rotateElbowWithHandRight = true;
			public bool rotateElbowWithHandForward = true;
			public float handDeltaPow = 1.5f, handDeltaFactor = -.3f, handDeltaOffset = 45f;
			// todo fix rotateElbowWithHandForward with factor != 1 -> horrible jumps. good value would be between [0.4, 0.6]
			public float handDeltaForwardPow = 2f, handDeltaForwardFactor = 1f, handDeltaForwardOffset = 0f, handDeltaForwardDeadzone = .3f;
			public float rotateElbowWithHandDelay = .08f;
		}


		public ArmTransforms arm;
		public ShoulderTransforms shoulder;
		public ShoulderPoser shoulderPoser;
		public Transform target;
		public bool left = true;

		public ArmIKElbowSettings elbowSettings;
		public BeforePositioningSettings beforePositioningSettings;
		public ElbowCorrectionSettings elbowCorrectionSettings;
		public HandSettings handSettings;

		Vector3 nextLowerArmAngle;

		Quaternion upperArmStartRotation, lowerArmStartRotation, wristStartRotation, handStartRotation;

		float interpolatedDeltaElbow;
		float interpolatedDeltaElbowForward;

		void Awake()
		{
			upperArmStartRotation = arm.upperArm.rotation;
			lowerArmStartRotation = arm.lowerArm.rotation;
			wristStartRotation = Quaternion.identity;
			if (arm.wrist1 != null)
				wristStartRotation = arm.wrist1.rotation;
			handStartRotation = arm.hand.rotation;
		}

		void OnEnable()
		{
			setUpperArmRotation(Quaternion.identity);
			setLowerArmRotation(Quaternion.identity);
			setHandRotation(Quaternion.identity);
		}

		void LateUpdate()
		{
			updateUpperArmPosition();
			calcElbowInnerAngle();
			rotateShoulder();
			correctElbowRotation();
			if (elbowSettings.calcElbowAngle)
			{
				positionElbow();
				if (elbowCorrectionSettings.useFixedElbowWhenNearShoulder)
					correctElbowAfterPositioning();
				if (handSettings.rotateElbowWithHandRight)
					rotateElbowWithHandRight();
				if (handSettings.rotateElbowWithHandForward)
					rotateElbowWithHandFoward();
				rotateHand();
			}
		}

		public void updateArmAndTurnElbowUp()
		{
			updateUpperArmPosition();
			calcElbowInnerAngle();
			rotateShoulder();
			correctElbowRotation();
		}

		void updateUpperArmPosition()
		{
			//arm.upperArm.position = shoulderAnker.transform.position;
		}

		void calcElbowInnerAngle()
		{
			Vector3 eulerAngles = new Vector3();
			float targetShoulderDistance = (target.position - upperArmPos).magnitude;
			float innerAngle;

			if (targetShoulderDistance > arm.armLength)
			{
				innerAngle = 0f;
			}
			else
			{
				innerAngle = Mathf.Acos(Mathf.Clamp((Mathf.Pow(arm.upperArmLength, 2f) + Mathf.Pow(arm.lowerArmLength, 2f) -
												Mathf.Pow(targetShoulderDistance, 2f)) / (2f * arm.upperArmLength * arm.lowerArmLength), -1f, 1f)) * Mathf.Rad2Deg;
				if (left)
					innerAngle = 180f - innerAngle;
				else
					innerAngle = 180f + innerAngle;
				if (float.IsNaN(innerAngle))
				{
					innerAngle = 180f;
				}
			}

			eulerAngles.y = innerAngle;
			nextLowerArmAngle = eulerAngles;
		}

		//source: https://github.com/NickHardeman/ofxIKArm/blob/master/src/ofxIKArm.cpp
		void rotateShoulder()
		{
			Vector3 eulerAngles = new Vector3();
			Vector3 targetShoulderDirection = (target.position - upperArmPos).normalized;
			float targetShoulderDistance = (target.position - upperArmPos).magnitude;

			eulerAngles.y = (left ? -1f : 1f) *
				Mathf.Acos(Mathf.Clamp((Mathf.Pow(targetShoulderDistance, 2f) + Mathf.Pow(arm.upperArmLength, 2f) -
							Mathf.Pow(arm.lowerArmLength, 2f)) / (2f * targetShoulderDistance * arm.upperArmLength), -1f, 1f)) * Mathf.Rad2Deg;
			if (float.IsNaN(eulerAngles.y))
				eulerAngles.y = 0f;


			Quaternion shoulderRightRotation = Quaternion.FromToRotation(armDirection, targetShoulderDirection);
			setUpperArmRotation(shoulderRightRotation);
			arm.upperArm.rotation = Quaternion.AngleAxis(eulerAngles.y, lowerArmRotation * Vector3.up) * arm.upperArm.rotation;
			setLowerArmLocalRotation(Quaternion.Euler(nextLowerArmAngle));
		}

		float getElbowTargetAngle()
		{
			Vector3 localHandPosNormalized = shoulderAnker.InverseTransformPoint(handPos) / arm.armLength;

			// angle from Y
			var angle = elbowSettings.yWeight * localHandPosNormalized.y + elbowSettings.offsetAngle;

			// angle from Z
			/*angle += Mathf.Lerp(elbowSettings.zWeightBottom, elbowSettings.zWeightTop, Mathf.Clamp01((localHandPosNormalized.y + 1f) - elbowSettings.zBorderY)) *
					 (Mathf.Max(elbowSettings.zDistanceStart - localHandPosNormalized.z, 0f));*/
			if (localHandPosNormalized.y > 0)
				angle += elbowSettings.zWeightTop * (Mathf.Max(elbowSettings.zDistanceStart - localHandPosNormalized.z, 0f) * Mathf.Max(localHandPosNormalized.y, 0f));
			else
				angle += elbowSettings.zWeightBottom * (Mathf.Max(elbowSettings.zDistanceStart - localHandPosNormalized.z, 0f) * Mathf.Max(-localHandPosNormalized.y, 0f));


			// angle from X
			angle += elbowSettings.xWeight * Mathf.Max(localHandPosNormalized.x * (left ? 1.0f : -1.0f) + elbowSettings.xDistanceStart, 0f);

			if (elbowSettings.clampElbowAngle)
			{
				if (elbowSettings.softClampElbowAngle)
				{
					if (angle < elbowSettings.minAngle + elbowSettings.softClampRange)
					{
						float a = elbowSettings.minAngle + elbowSettings.softClampRange - angle;
						angle = elbowSettings.minAngle + elbowSettings.softClampRange * (1f - Mathf.Log(1f + a) * 3f);
					}
				}
				else
				{
					angle = Mathf.Clamp(angle, elbowSettings.minAngle, elbowSettings.maxAngle);
				}
			}

			if (left)
				angle *= -1f;

			return angle;
		}

		void correctElbowRotation()
		{
			var s = beforePositioningSettings;

			Vector3 localTargetPos = shoulderAnker.InverseTransformPoint(target.position) / arm.armLength;
			float elbowOutsideFactor = Mathf.Clamp01(
									 Mathf.Clamp01((s.startBelowZ - localTargetPos.z) /
												   Mathf.Abs(s.startBelowZ) * .5f) *
									 Mathf.Clamp01((localTargetPos.y - s.startAboveY) /
												   Mathf.Abs(s.startAboveY)) *
									 Mathf.Clamp01(1f - localTargetPos.x * (left ? -1f : 1f))
								 ) * s.weight;

			Vector3 shoulderHandDirection = (upperArmPos - handPos).normalized;
			Vector3 targetDir = shoulder.transform.rotation * (Vector3.up + (s.correctElbowOutside ? (armDirection + Vector3.forward * -.2f) * elbowOutsideFactor : Vector3.zero));
			Vector3 cross = Vector3.Cross(shoulderHandDirection, targetDir * 1000f);

			Vector3 upperArmUp = upperArmRotation * Vector3.up;

			float elbowTargetUp = Vector3.Dot(upperArmUp, targetDir);
			float elbowAngle = Vector3.Angle(cross, upperArmUp) + (left ? 0f : 180f);
			Quaternion rotation = Quaternion.AngleAxis(elbowAngle * Mathf.Sign(elbowTargetUp), shoulderHandDirection);
			arm.upperArm.rotation = rotation * arm.upperArm.rotation;
		}

		/// <summary>
		/// reduces calculation problems when hand is moving around shoulder XZ coordinates -> forces elbow to be outside of body
		/// </summary>
		void correctElbowAfterPositioning()
		{
			var s = elbowCorrectionSettings;
			Vector3 localTargetPos = shoulderAnker.InverseTransformPoint(target.position) / arm.armLength;
			Vector3 shoulderHandDirection = (upperArmPos - handPos).normalized;
			Vector3 elbowPos = s.localElbowPos;

			if (left)
				elbowPos.x *= -1f;

			Vector3 targetDir = shoulder.transform.rotation * elbowPos.normalized;
			Vector3 cross = Vector3.Cross(shoulderHandDirection, targetDir);

			Vector3 upperArmUp = upperArmRotation * Vector3.up;


			Vector3 distance = target.position - upperArmPos;
			distance = distance.magnitude * shoulder.transform.InverseTransformDirection(distance / distance.magnitude);

			float weight = Mathf.Clamp01(Mathf.Clamp01((s.startBelowDistance - distance.xz().magnitude / arm.armLength) /
						   s.startBelowDistance) * s.weight + Mathf.Clamp01((-distance.z + .1f) * 3)) *
						   Mathf.Clamp01((s.startBelowY - localTargetPos.y) /
										 s.startBelowY);

			float elbowTargetUp = Vector3.Dot(upperArmUp, targetDir);
			float elbowAngle2 = Vector3.Angle(cross, upperArmUp) + (left ? 0f : 180f);
			Quaternion rotation = Quaternion.AngleAxis((elbowAngle2 * Mathf.Sign(elbowTargetUp)).toSignedEulerAngle() * Mathf.Clamp(weight, 0, 1f), shoulderHandDirection);
			arm.upperArm.rotation = rotation * arm.upperArm.rotation;
		}

		public void rotateElbow(float angle)
		{
			Vector3 shoulderHandDirection = (upperArmPos - handPos).normalized;

			Quaternion rotation = Quaternion.AngleAxis(angle, shoulderHandDirection);
			setUpperArmRotation(rotation * upperArmRotation);
		}

		//source: https://github.com/NickHardeman/ofxIKArm/blob/master/src/ofxIKArm.cpp
		void positionElbow()
		{
			float targetElbowAngle = getElbowTargetAngle();
			rotateElbow(targetElbowAngle);
		}


		void rotateElbowWithHandRight()
		{
			var s = handSettings;
			Vector3 handUpVec = target.rotation * Vector3.up;
			float forwardAngle = VectorHelpers.getAngleBetween(lowerArmRotation * Vector3.right, target.rotation * Vector3.right,
				lowerArmRotation * Vector3.up, lowerArmRotation * Vector3.forward);

			// todo reduce influence if hand local forward rotation is high (hand tilted inside)
			Quaternion handForwardRotation = Quaternion.AngleAxis(-forwardAngle, lowerArmRotation * Vector3.forward);
			handUpVec = handForwardRotation * handUpVec;

			float elbowTargetAngle = VectorHelpers.getAngleBetween(lowerArmRotation * Vector3.up, handUpVec,
				lowerArmRotation * Vector3.forward, lowerArmRotation * armDirection);

			float deltaElbow = (elbowTargetAngle + (left ? -s.handDeltaOffset : s.handDeltaOffset)) / 180f;

			deltaElbow = Mathf.Sign(deltaElbow) * Mathf.Pow(Mathf.Abs(deltaElbow), s.handDeltaPow) * 180f * s.handDeltaFactor;
			interpolatedDeltaElbow =
				Mathf.LerpAngle(interpolatedDeltaElbow, deltaElbow, Time.deltaTime / s.rotateElbowWithHandDelay);
			rotateElbow(interpolatedDeltaElbow);
		}

		void rotateElbowWithHandFoward()
		{
			var s = handSettings;
			Vector3 handRightVec = target.rotation * armDirection;

			float elbowTargetAngleForward = VectorHelpers.getAngleBetween(lowerArmRotation * armDirection, handRightVec,
				lowerArmRotation * Vector3.up, lowerArmRotation * Vector3.forward);

			float deltaElbowForward = (elbowTargetAngleForward + (left ? -s.handDeltaForwardOffset : s.handDeltaForwardOffset)) / 180f;

			if (Mathf.Abs(deltaElbowForward) < s.handDeltaForwardDeadzone)
				deltaElbowForward = 0f;
			else
			{
				deltaElbowForward = (deltaElbowForward - Mathf.Sign(deltaElbowForward) * s.handDeltaForwardDeadzone) / (1f - s.handDeltaForwardDeadzone);
			}

			deltaElbowForward = Mathf.Sign(deltaElbowForward) * Mathf.Pow(Mathf.Abs(deltaElbowForward), s.handDeltaForwardPow) * 180f;
			interpolatedDeltaElbowForward = Mathf.LerpAngle(interpolatedDeltaElbowForward, deltaElbowForward, Time.deltaTime / s.rotateElbowWithHandDelay);

			float signedInterpolated = interpolatedDeltaElbowForward.toSignedEulerAngle();
			rotateElbow(signedInterpolated * s.handDeltaForwardFactor);
		}

		public void rotateHand()
		{
			if (handSettings.useWristRotation)
			{
				Vector3 handUpVec = target.rotation * Vector3.up;
				float forwardAngle = VectorHelpers.getAngleBetween(lowerArmRotation * Vector3.right, target.rotation * Vector3.right,
					lowerArmRotation * Vector3.up, lowerArmRotation * Vector3.forward);

				// todo reduce influence if hand local forward rotation is high (hand tilted inside)
				Quaternion handForwardRotation = Quaternion.AngleAxis(-forwardAngle, lowerArmRotation * Vector3.forward);
				handUpVec = handForwardRotation * handUpVec;

				float elbowTargetAngle = VectorHelpers.getAngleBetween(lowerArmRotation * Vector3.up, handUpVec,
					lowerArmRotation * Vector3.forward, lowerArmRotation * armDirection);

				elbowTargetAngle = Mathf.Clamp(elbowTargetAngle, -90f, 90f);
				if (arm.wrist1 != null)
					setWrist1Rotation(Quaternion.AngleAxis(elbowTargetAngle * .3f, lowerArmRotation * armDirection) * lowerArmRotation);
				if (arm.wrist2 != null)
					setWrist2Rotation(Quaternion.AngleAxis(elbowTargetAngle * .8f, lowerArmRotation * armDirection) * lowerArmRotation);
			}
			setHandRotation(target.rotation);
		}

		Vector3 removeShoulderRightRotation(Vector3 direction) => Quaternion.AngleAxis(-shoulderPoser.shoulderRightRotation, shoulder.transform.right) * direction;

		Vector3 armDirection => left ? Vector3.left : Vector3.right;
		Vector3 upperArmPos => arm.upperArm.position;
		Vector3 lowerArmPos => arm.lowerArm.position;
		Vector3 handPos => arm.hand.position;
		Transform shoulderAnker => left ? shoulder.leftShoulderAnchor : shoulder.rightShoulderAnchor;

		Quaternion upperArmRotation => arm.upperArm.rotation * Quaternion.Inverse(upperArmStartRotation);
		Quaternion lowerArmRotation => arm.lowerArm.rotation * Quaternion.Inverse(lowerArmStartRotation);
		Quaternion handRotation => arm.hand.rotation * Quaternion.Inverse(handStartRotation);

		void setUpperArmRotation(Quaternion rotation) => arm.upperArm.rotation = rotation * upperArmStartRotation;
		void setLowerArmRotation(Quaternion rotation) => arm.lowerArm.rotation = rotation * lowerArmStartRotation;
		void setLowerArmLocalRotation(Quaternion rotation) => arm.lowerArm.rotation = upperArmRotation * rotation * lowerArmStartRotation;
		void setWrist1Rotation(Quaternion rotation) => arm.wrist1.rotation = rotation * wristStartRotation;
		void setWrist2Rotation(Quaternion rotation) => arm.wrist2.rotation = rotation * wristStartRotation;
		void setWristLocalRotation(Quaternion rotation) => arm.wrist1.rotation = arm.lowerArm.rotation * rotation * wristStartRotation;

		void setHandRotation(Quaternion rotation) =>
			arm.hand.rotation = arm.hand.rotation = rotation * handStartRotation;
	}



}
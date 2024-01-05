using UnityEngine;

namespace VRArmIK
{
	public class StaticOffsetTransform : MonoBehaviour
	{
		public enum EulerOrder
		{
			XYZ,
			XZY,
			YXZ,
			YZX,
			ZXY,
			ZYX
		};

		public Transform reference = null;
		public Vector3 offsetPosition;
		public Vector3 offsetRotation;
		public Vector3 orientationalOffset;
		public Vector3 referenceRotationMultiplicator = Vector3.one;

		public EulerOrder axisOrder;

		public bool referenceLocalPosition = false, referenceLocalRotation = false;
		public bool applyLocalPosition = false, applyLocalRotation = false;
		public bool applyPosition = true, applyRotation = true;
		public bool applyForwardOffsetAfterRotationOffset = false;


		public static Vector3 switchAxis(Vector3 r, EulerOrder order)
		{
			switch (order)
			{
				case EulerOrder.XYZ:
					return new Vector3(r.x, r.y, r.z);
				case EulerOrder.XZY:
					return new Vector3(r.x, r.z, r.y);
				case EulerOrder.YXZ:
					return new Vector3(r.y, r.x, r.z);
				case EulerOrder.YZX:
					return new Vector3(r.y, r.z, r.x);
				case EulerOrder.ZXY:
					return new Vector3(r.z, r.x, r.y);
				case EulerOrder.ZYX:
					return new Vector3(r.z, r.y, r.x);

				default:
					return r;
			}
		}

		void Awake()
		{
			updatePosition();
		}

		void Update()
		{
			updatePosition();
		}

		void updatePosition()
		{
			if (reference == null)
				return;

			Vector3 rot = switchAxis(referenceLocalRotation ? reference.localEulerAngles : reference.eulerAngles, axisOrder) +
			              offsetRotation;
			rot.Scale(referenceRotationMultiplicator);

			Vector3 pos = referenceLocalPosition ? reference.localPosition : reference.position;


			if (applyForwardOffsetAfterRotationOffset)
			{
				pos += Quaternion.Euler(rot) * Vector3.right * orientationalOffset.x;
				pos += Quaternion.Euler(rot) * Vector3.up * orientationalOffset.y;
				pos += Quaternion.Euler(rot) * Vector3.forward * orientationalOffset.z;
			}
			else
			{
				pos += reference.right * orientationalOffset.x;
				pos += reference.up * orientationalOffset.y;
				pos += reference.forward * orientationalOffset.z;
			}

			pos += offsetPosition;

			if (applyPosition)
			{
				if (applyLocalPosition)
				{
					transform.localPosition = pos;
				}
				else
				{
					transform.position = pos;
				}
			}


			if (applyRotation)
			{
				if (applyLocalRotation)
				{
					transform.localEulerAngles = rot;
				}
				else
				{
					transform.eulerAngles = rot;
				}
			}
		}
	}
}
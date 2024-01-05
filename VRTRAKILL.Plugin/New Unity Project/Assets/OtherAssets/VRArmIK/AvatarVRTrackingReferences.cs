using UnityEngine;

namespace VRArmIK
{
	public class AvatarVRTrackingReferences : MonoBehaviour
	{
		public StaticOffsetTransform head, hmd, leftHand, rightHand;

		void Start()
		{
			initTransforms();
		}

		[ContextMenu("init transforms")]
		public void initTransforms()
		{
			createTransforms();
			connectTransforms();
		}

		void setStaticOffsetSettings(StaticOffsetTransform s)
		{
			s.referenceLocalPosition = false;
			s.referenceLocalRotation = false;
			s.applyLocalPosition = true;
			s.applyLocalRotation = true;
			s.applyPosition = true;
			s.applyRotation = true;
			s.applyForwardOffsetAfterRotationOffset = false;
		}


		void createTransform(ref StaticOffsetTransform t, string name)
		{
			if (t == null)
			{
				t = new GameObject(name).AddComponent<StaticOffsetTransform>();
				t.transform.parent = transform;
				setStaticOffsetSettings(t);
			}
		}

		void createHandTransform(ref Transform t, string name, Transform parent)
		{
			if (t == null)
			{
				t = new GameObject(name).transform;
				t.transform.localPosition = Vector3.zero;
				t.transform.parent = parent;
			}
		}

		void createTransforms()
		{
			createTransform(ref head, nameof(head));
			createTransform(ref leftHand, nameof(leftHand));
			createTransform(ref rightHand, nameof(rightHand));
			createTransform(ref hmd, nameof(hmd));
		}

		void connectTransforms()
		{
			StaticOffsetTransform sot = this.GetOrAddComponent<StaticOffsetTransform>();
			if (sot.reference == null)
			{
				sot.reference = transform.parent;
			}

			head.reference = head.reference != null ? head.reference : PoseManager.Instance.vrTransforms.head;
			hmd.reference = hmd.reference != null ? hmd.reference : PoseManager.Instance.vrTransforms.hmd;
			leftHand.reference = leftHand.reference != null
				? leftHand.reference
				: PoseManager.Instance.vrTransforms.leftHand;
			rightHand.reference = rightHand.reference != null
				? rightHand.reference
				: PoseManager.Instance.vrTransforms.rightHand;
		}
	}
}
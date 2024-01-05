using UnityEngine;
using UnityEngine.XR;

namespace VRArmIK
{
	[ExecuteInEditMode]
	public class PoseManager : MonoBehaviour
	{
		public static PoseManager Instance = null;
		public VRTrackingReferences vrTransforms;

		public delegate void OnCalibrateListener();

		public event OnCalibrateListener onCalibrate;

        // Oculus uses a different reference position -> 0 is the reference head position if the user is standing in the middle of the room. 
        // In OpenVR, the 0 position is the ground position and the user is then at (0, playerHeightHmd, 0) if he is in the middle of the room, so I need to correct this for shoulder calculation 
        public float vrSystemOffsetHeight = 0.0f;

		public const float referencePlayerHeightHmd = 1.7f;
		public const float referencePlayerWidthWrist = 1.39f;
		public float playerHeightHmd = 1.70f;
		public float playerWidthWrist = 1.39f;
		public float playerWidthShoulders = 0.31f;
        public bool loadPlayerSizeOnAwake = false;

		void OnEnable()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else if (Instance != null)
			{
				Debug.LogError("Multiple Instances of PoseManager in Scene");
			}
		}

		void Awake()
		{
            if (loadPlayerSizeOnAwake)
            {
                loadPlayerSize();
            }
        }

		void Start()
		{
			onCalibrate += OnCalibrate;
		}

		[ContextMenu("calibrate")]
		void OnCalibrate()
		{
			playerHeightHmd = Camera.main.transform.position.y;
		}

		void loadPlayerWidthShoulders()
		{
			playerWidthShoulders = PlayerPrefs.GetFloat("VRArmIK_PlayerWidthShoulders", 0.31f);
		}

		public void savePlayerWidthShoulders(float width)
		{
			PlayerPrefs.SetFloat("VRArmIK_PlayerWidthShoulders", width);
		}

		[ContextMenu("setArmLength")]
		public void calibrateIK()
		{
			playerWidthWrist = (vrTransforms.leftHand.position - vrTransforms.rightHand.position).magnitude;
			playerHeightHmd = vrTransforms.hmd.position.y;
			savePlayerSize(playerHeightHmd, playerWidthWrist);
		}

		public void savePlayerSize(float heightHmd, float widthWrist)
		{
			PlayerPrefs.SetFloat("VRArmIK_PlayerHeightHmd", heightHmd);
			PlayerPrefs.SetFloat("VRArmIK_PlayerWidthWrist", widthWrist);
			loadPlayerSize();
			onCalibrate?.Invoke();
		}

		public void loadPlayerSize()
		{
			playerHeightHmd = PlayerPrefs.GetFloat("VRArmIK_PlayerHeightHmd", referencePlayerHeightHmd);
			playerWidthWrist = PlayerPrefs.GetFloat("VRArmIK_PlayerWidthWrist", referencePlayerWidthWrist);
		}
	}
}

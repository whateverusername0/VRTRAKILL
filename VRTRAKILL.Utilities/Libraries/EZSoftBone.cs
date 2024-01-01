using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRBasePlugin.Util.Libraries.EZhex1991.EZSoftBone
{
    public class EZSoftBone : MonoBehaviour
    {
        public static readonly float DeltaTime_Min = 1e-6f;

        public enum UnificationMode
        {
            None,
            Rooted,
            Unified,
        }

        public enum DeltaTimeMode
        {
            DeltaTime,
            UnscaledDeltaTime,
            Constant,
        }

        private class Bone
        {
            public Bone parentBone;
            public Vector3 localPosition;
            public Quaternion localRotation;

            public Bone leftBone;
            public Vector3 leftPosition;
            public Bone rightBone;
            public Vector3 rightPosition;

            public List<Bone> childBones = new List<Bone>();

            public Transform transform;
            public Vector3 worldPosition;

            public Transform systemSpace;
            public Vector3 systemPosition;

            public int depth;
            public float boneLength;
            public float treeLength;
            public float normalizedLength;

            public float radius;
            public float damping;
            public float stiffness;
            public float resistance;
            public float slackness;

            public Vector3 speed;

            public Bone(Transform systemSpace, Transform transform, IEnumerable<Transform> endBones, int startDepth, int depth, float nodeLength, float boneLength)
            {
                this.transform = transform;
                this.systemSpace = systemSpace;
                worldPosition = transform.position;
                systemPosition = systemSpace == null ? worldPosition : systemSpace.InverseTransformPoint(worldPosition);
                localPosition = transform.localPosition;
                localRotation = transform.localRotation;
                this.depth = depth;
                if (depth > startDepth)
                {
                    this.boneLength = boneLength + nodeLength;
                }
                treeLength = Mathf.Max(treeLength, this.boneLength);
                if (transform.childCount > 0 && !endBones.Contains(transform))
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Transform child = transform.GetChild(i);
                        if (!child.gameObject.activeSelf) continue;
                        Bone childBone = new Bone(systemSpace, child, endBones, startDepth, depth + 1, Vector3.Distance(child.position, transform.position), this.boneLength);
                        childBone.parentBone = this;
                        childBones.Add(childBone);
                        treeLength = Mathf.Max(treeLength, childBone.treeLength);
                    }
                }
                normalizedLength = treeLength == 0 ? 0 : (this.boneLength / treeLength);
            }

            public void SetTreeLength()
            {
                SetTreeLength(treeLength);
            }
            public void SetTreeLength(float treeLength)
            {
                this.treeLength = treeLength;
                normalizedLength = treeLength == 0 ? 0 : (boneLength / treeLength);
                for (int i = 0; i < childBones.Count; i++)
                {
                    childBones[i].SetTreeLength(treeLength);
                }
            }

            public void SetLeftSibling(Bone left)
            {
                if (left == this || left == rightBone) return;
                leftBone = left;
                leftPosition = transform.InverseTransformPoint(left.worldPosition);
            }
            public void SetRightSibling(Bone right)
            {
                if (right == this || right == leftBone) return;
                rightBone = right;
                rightPosition = transform.InverseTransformPoint(right.worldPosition);
            }

            public void Inflate(float baseRadius, AnimationCurve radiusCurve)
            {
                radius = radiusCurve.Evaluate(normalizedLength) * baseRadius;
                for (int i = 0; i < childBones.Count; i++)
                {
                    childBones[i].Inflate(baseRadius, radiusCurve);
                }
            }
            public void Inflate(float baseRadius, AnimationCurve radiusCurve, EZSoftBoneMaterial material)
            {
                radius = radiusCurve.Evaluate(normalizedLength) * baseRadius;
                damping = material.GetDamping(normalizedLength);
                stiffness = material.GetStiffness(normalizedLength);
                resistance = material.GetResistance(normalizedLength);
                slackness = material.GetSlackness(normalizedLength);
                for (int i = 0; i < childBones.Count; i++)
                {
                    childBones[i].Inflate(baseRadius, radiusCurve, material);
                }
            }

            public void RevertTransforms(int startDepth)
            {
                if (depth > startDepth)
                {
                    transform.localPosition = localPosition;
                    transform.localRotation = localRotation;
                }
                for (int i = 0; i < childBones.Count; i++)
                {
                    childBones[i].RevertTransforms(startDepth);
                }
            }
            public void UpdateTransform(bool siblingRotationConstraints, int startDepth)
            {
                if (depth > startDepth)
                {
                    if (childBones.Count == 1)
                    {
                        Bone childBone = childBones[0];
                        transform.rotation *= Quaternion.FromToRotation(childBone.localPosition,
                                                                        transform.InverseTransformVector(childBone.worldPosition - worldPosition));

                        if (siblingRotationConstraints)
                        {
                            if (leftBone != null && rightBone != null)
                            {
                                Vector3 directionLeft0 = leftPosition;
                                Vector3 directionLeft1 = transform.InverseTransformVector(leftBone.worldPosition - worldPosition);
                                Quaternion rotationLeft = Quaternion.FromToRotation(directionLeft0, directionLeft1);

                                Vector3 directionRight0 = rightPosition;
                                Vector3 directionRight1 = transform.InverseTransformVector(rightBone.worldPosition - worldPosition);
                                Quaternion rotationRight = Quaternion.FromToRotation(directionRight0, directionRight1);

                                transform.rotation *= Quaternion.Lerp(rotationLeft, rotationRight, 0.5f);
                            }
                            else if (leftBone != null)
                            {
                                Vector3 directionLeft0 = leftPosition;
                                Vector3 directionLeft1 = transform.InverseTransformVector(leftBone.worldPosition - worldPosition);
                                Quaternion rotationLeft = Quaternion.FromToRotation(directionLeft0, directionLeft1);
                                transform.rotation *= rotationLeft;
                            }
                            else if (rightBone != null)
                            {
                                Vector3 directionRight0 = rightPosition;
                                Vector3 directionRight1 = transform.InverseTransformVector(rightBone.worldPosition - worldPosition);
                                Quaternion rotationRight = Quaternion.FromToRotation(directionRight0, directionRight1);
                                transform.rotation *= rotationRight;
                            }
                        }
                    }
                    transform.position = worldPosition;
                }

                if (systemSpace != null) systemPosition = systemSpace.InverseTransformPoint(worldPosition);
                for (int i = 0; i < childBones.Count; i++)
                {
                    childBones[i].UpdateTransform(siblingRotationConstraints, startDepth);
                }
            }

            public void SetRestState()
            {
                worldPosition = transform.position;
                systemPosition = systemSpace == null ? worldPosition : systemSpace.InverseTransformPoint(worldPosition);
                speed = Vector3.zero;
                for (int i = 0; i < childBones.Count; i++)
                {
                    childBones[i].SetRestState();
                }
            }
            public void UpdateSpace()
            {
                if (systemSpace == null) return;
                worldPosition = systemSpace.TransformPoint(systemPosition);
                for (int i = 0; i < childBones.Count; i++)
                {
                    childBones[i].UpdateSpace();
                }
            }
        }

        [SerializeField]
        public List<Transform> m_RootBones = new List<Transform>();
        public List<Transform> rootBones { get { return m_RootBones; } }
        [SerializeField]
        public List<Transform> m_EndBones = new List<Transform>();
        public List<Transform> endBones { get { return m_EndBones; } }

        [SerializeField]
        private EZSoftBoneMaterial m_Material;
        private EZSoftBoneMaterial m_InstanceMaterial;
        public EZSoftBoneMaterial sharedMaterial
        {
            get
            {
                if (m_Material == null)
                    m_Material = EZSoftBoneMaterial.defaultMaterial;
                return m_Material;
            }
            set
            {
                m_Material = value;
            }
        }
        public EZSoftBoneMaterial material
        {
            get
            {
                if (m_InstanceMaterial == null)
                {
                    m_InstanceMaterial = m_Material = Instantiate(sharedMaterial);
                }
                return m_InstanceMaterial;
            }
            set
            {
                m_InstanceMaterial = m_Material = value;
            }
        }

        #region Structure
        [SerializeField]
        private int m_StartDepth;
        public int startDepth { get { return m_StartDepth; } set { m_StartDepth = value; } }

        [SerializeField]
        private UnificationMode m_SiblingConstraints = UnificationMode.None;
        public UnificationMode siblingConstraints { get { return m_SiblingConstraints; } set { m_SiblingConstraints = value; } }
        [SerializeField]
        private bool m_ClosedSiblings = false;
        public bool closedSiblings { get { return m_ClosedSiblings; } set { m_ClosedSiblings = value; } }
        [SerializeField]
        private bool m_SiblingRotationConstraints = true;
        public bool siblingRotationConstraints { get { return m_SiblingRotationConstraints; } set { m_SiblingRotationConstraints = value; } }
        [SerializeField]
        private UnificationMode m_LengthUnification = UnificationMode.None;
        public UnificationMode lengthUnification { get { return m_LengthUnification; } set { m_LengthUnification = value; } }
        #endregion

        #region Collision
        [SerializeField]
        private LayerMask m_CollisionLayers = 1;
        public LayerMask collisionLayers { get { return m_CollisionLayers; } set { m_CollisionLayers = value; } }
        [SerializeField]
        private List<Collider> m_ExtraColliders = new List<Collider>();
        public List<Collider> extraColliders { get { return m_ExtraColliders; } }
        [SerializeField]
        private float m_Radius = 0;
        public float radius { get { return m_Radius; } set { m_Radius = value; } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_RadiusCurve = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve radiusCurve { get { return m_RadiusCurve; } }
        #endregion

        #region Performance
        [SerializeField]
        private DeltaTimeMode m_DeltaTimeMode = DeltaTimeMode.DeltaTime;
        public DeltaTimeMode deltaTimeMode { get { return m_DeltaTimeMode; } set { m_DeltaTimeMode = value; } }
        [SerializeField]
        private float m_ConstantDeltaTime = 0.03f;
        public float constantDeltaTime { get { return m_ConstantDeltaTime; } set { m_ConstantDeltaTime = value; } }

        [SerializeField, Range(1, 10)]
        private int m_Iterations = 1;
        public int iterations { get { return m_Iterations; } set { m_Iterations = value; } }

        [SerializeField]
        private float m_SleepThreshold = 0.005f;
        public float sleepThreshold { get { return m_SleepThreshold; } set { m_SleepThreshold = Mathf.Max(0, value); } }
        #endregion

        #region Gravity
        [SerializeField]
        private Transform m_GravityAligner;
        public Transform gravityAligner { get { return m_GravityAligner; } set { m_GravityAligner = value; } }
        [SerializeField]
        private Vector3 m_Gravity;
        public Vector3 gravity { get { return m_Gravity; } set { m_Gravity = value; } }
        #endregion

        #region Force
        [SerializeField]
        private EZSoftBoneForceField m_ForceModule;
        public EZSoftBoneForceField forceModule { get { return m_ForceModule; } set { m_ForceModule = value; } }
        [SerializeField]
        private float m_ForceScale = 1;
        public float forceScale { get { return m_ForceScale; } set { m_ForceScale = value; } }
        #endregion

        #region References
        [SerializeField]
        private Transform m_SimulateSpace;
        public Transform simulateSpace { get { return m_SimulateSpace; } set { m_SimulateSpace = value; } }
        #endregion

        public float globalRadius { get; private set; }
        public Vector3 globalForce { get; private set; }

        private List<Bone> m_Structures = new List<Bone>();

        public void Awake()
        {
            InitStructures();
        }
        public void OnEnable()
        {
            SetRestState();
        }
        public void Update()
        {
            RevertTransforms(startDepth);
        }
        public void LateUpdate()
        {
            switch (deltaTimeMode)
            {
                case DeltaTimeMode.DeltaTime:
                    UpdateStructures(Time.deltaTime);
                    break;
                case DeltaTimeMode.UnscaledDeltaTime:
                    UpdateStructures(Time.unscaledDeltaTime);
                    break;
                case DeltaTimeMode.Constant:
                    UpdateStructures(constantDeltaTime);
                    break;
            }
            UpdateTransforms();
        }
        public void OnDisable()
        {
            RevertTransforms(startDepth);
        }

#if UNITY_EDITOR
                private void OnValidate()
                {
                    m_StartDepth = Mathf.Max(0, m_StartDepth);
                    m_ConstantDeltaTime = Mathf.Max(DeltaTime_Min, m_ConstantDeltaTime);
                    m_Iterations = Mathf.Max(1, m_Iterations);
                    m_SleepThreshold = Mathf.Max(0, m_SleepThreshold);
                    m_Radius = Mathf.Max(0, m_Radius);
                }
                private void OnDrawGizmosSelected()
                {
                    if (!enabled) return;

                    if (!Application.isPlaying)
                    {
                        InitStructures();
                    }

                    for (int i = 0; i < m_Structures.Count; i++)
                    {
                        DrawBoneGizmos(m_Structures[i]);
                    }

                    if (forceModule != null)
                    {
                        forceModule.DrawGizmos();
                    }
                }
                private void DrawBoneGizmos(Bone bone)
                {
                    for (int i = 0; i < bone.childBones.Count; i++)
                    {
                        DrawBoneGizmos(bone.childBones[i]);
                    }

                    Gizmos.color = Color.Lerp(Color.white, Color.red, bone.normalizedLength);
                    if (bone.parentBone != null)
                        Gizmos.DrawLine(bone.worldPosition, bone.parentBone.worldPosition);
                    if (bone.depth > startDepth)
                        Gizmos.DrawWireSphere(bone.worldPosition, bone.radius);
                    if (siblingConstraints != UnificationMode.None)
                    {
                        if (bone.leftBone != null)
                            Gizmos.DrawLine(bone.leftBone.worldPosition, bone.worldPosition);
                        if (bone.rightBone != null)
                            Gizmos.DrawLine(bone.rightBone.worldPosition, bone.worldPosition);
                    }
                }
#endif

        public void RevertTransforms()
        {
            RevertTransforms(startDepth);
        }
        public void RevertTransforms(int startDepth)
        {
            for (int i = 0; i < m_Structures.Count; i++)
            {
                m_Structures[i].RevertTransforms(startDepth);
            }
        }
        public void InitStructures()
        {
            CreateBones();
            SetSiblings();
            SetTreeLength();
            RefreshRadius();
        }
        public void SetRestState()
        {
            for (int i = 0; i < m_Structures.Count; i++)
            {
                m_Structures[i].SetRestState();
            }
        }

        private void CreateBones()
        {
            m_Structures.Clear();
            if (rootBones == null || rootBones.Count == 0) return;
            for (int i = 0; i < rootBones.Count; i++)
            {
                if (rootBones[i] == null) continue;
                Bone bone = new Bone(simulateSpace, rootBones[i], endBones, startDepth, 0, 0, 0);
                m_Structures.Add(bone);
            }
        }
        private void SetSiblings()
        {
            if (siblingConstraints == UnificationMode.Rooted)
            {
                for (int i = 0; i < m_Structures.Count; i++)
                {
                    Queue<Bone> bones = new Queue<Bone>();
                    bones.Enqueue(m_Structures[i]);
                    SetSiblingsByDepth(bones, closedSiblings);
                }
            }
            else if (siblingConstraints == UnificationMode.Unified)
            {
                Queue<Bone> bones = new Queue<Bone>();
                for (int i = 0; i < m_Structures.Count; i++)
                {
                    bones.Enqueue(m_Structures[i]);
                }
                if (bones.Count > 0) SetSiblingsByDepth(bones, closedSiblings);
            }
        }
        private void SetSiblingsByDepth(Queue<Bone> bones, bool closed)
        {
            Bone first = bones.Dequeue();
            for (int i = 0; i < first.childBones.Count; i++)
            {
                bones.Enqueue(first.childBones[i]);
            }
            Bone left = first;
            Bone right = null;
            while (bones.Count > 0)
            {
                right = bones.Dequeue();
                for (int i = 0; i < right.childBones.Count; i++)
                {
                    bones.Enqueue(right.childBones[i]);
                }
                if (left.depth == right.depth)
                {
                    // same depth
                    left.SetRightSibling(right);
                    right.SetLeftSibling(left);
                }
                else
                {
                    // connect the last node to the first of this tier
                    if (closed)
                    {
                        left.SetRightSibling(first);
                        first.SetLeftSibling(left);
                    }
                    // next depth
                    first = right;
                }
                left = right;
            }
            // connect the last node to the first of the last tier
            if (right != null && closed)
            {
                first.SetLeftSibling(right);
                right.SetRightSibling(first);
            }
        }
        private void SetTreeLength()
        {
            if (lengthUnification == UnificationMode.Rooted)
            {
                for (int i = 0; i < m_Structures.Count; i++)
                {
                    m_Structures[i].SetTreeLength();
                }
            }
            else if (lengthUnification == UnificationMode.Unified)
            {
                float maxLength = 0;
                for (int i = 0; i < m_Structures.Count; i++)
                {
                    maxLength = Mathf.Max(maxLength, m_Structures[i].treeLength);
                }
                for (int i = 0; i < m_Structures.Count; i++)
                {
                    m_Structures[i].SetTreeLength(maxLength);
                }
            }
        }
        public void RefreshRadius()
        {
            globalRadius = transform.lossyScale.Abs().Max() * radius;
            for (int i = 0; i < m_Structures.Count; i++)
            {
                m_Structures[i].Inflate(globalRadius, radiusCurve);
            }
        }

        private void UpdateStructures(float deltaTime)
        {
            if (deltaTime <= DeltaTime_Min) return;

            // radius
            globalRadius = transform.lossyScale.Abs().Max() * radius;

            // parameters
            for (int j = 0; j < m_Structures.Count; j++)
            {
                m_Structures[j].Inflate(globalRadius, radiusCurve, sharedMaterial);
                if (simulateSpace != null) m_Structures[j].UpdateSpace();
            }

            // force
            globalForce = gravity;
            if (gravityAligner != null)
            {
                Vector3 alignedDir = gravityAligner.TransformDirection(gravity).normalized;
                Vector3 globalDir = gravity.normalized;
                float attenuation = Mathf.Acos(Vector3.Dot(alignedDir, globalDir)) / Mathf.PI;
                globalForce *= attenuation;
            }

            deltaTime /= iterations;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < m_Structures.Count; j++)
                {
                    UpdateBones(m_Structures[j], deltaTime);
                }
            }
        }
        private void UpdateBones(Bone bone, float deltaTime)
        {
            if (bone.depth > startDepth)
            {
                Vector3 oldWorldPosition, newWorldPosition, expectedPosition;
                oldWorldPosition = newWorldPosition = bone.worldPosition;

                // Resistance (force resistance)
                Vector3 force = globalForce;
                if (forceModule != null && forceModule.isActiveAndEnabled)
                {
                    force += forceModule.GetForce(bone.normalizedLength) * forceScale;
                }
                force.x *= transform.localScale.x;
                force.y *= transform.localScale.y;
                force.z *= transform.localScale.z;
                bone.speed += force * (1 - bone.resistance) / iterations;

                // Damping (inertia attenuation)
                bone.speed *= 1 - bone.damping;
                if (bone.speed.sqrMagnitude > sleepThreshold)
                {
                    newWorldPosition += bone.speed * deltaTime;
                }

                // Stiffness (shape keeper)
                Vector3 parentMovement = bone.parentBone.worldPosition - bone.parentBone.transform.position;
                expectedPosition = bone.parentBone.transform.TransformPoint(bone.localPosition) + parentMovement;
                newWorldPosition = Vector3.Lerp(newWorldPosition, expectedPosition, bone.stiffness / iterations);

                // Slackness (length keeper)
                // Length needs to be calculated with TransformVector to match runtime scaling
                Vector3 dirToParent = (newWorldPosition - bone.parentBone.worldPosition).normalized;
                float lengthToParent = bone.parentBone.transform.TransformVector(bone.localPosition).magnitude;
                expectedPosition = bone.parentBone.worldPosition + dirToParent * lengthToParent;
                int lengthConstraints = 1;
                // Sibling constraints
                if (siblingConstraints != UnificationMode.None)
                {
                    if (bone.leftBone != null)
                    {
                        Vector3 dirToLeft = (newWorldPosition - bone.leftBone.worldPosition).normalized;
                        float lengthToLeft = bone.transform.TransformVector(bone.leftPosition).magnitude;
                        expectedPosition += bone.leftBone.worldPosition + dirToLeft * lengthToLeft;
                        lengthConstraints++;
                    }
                    if (bone.rightBone != null)
                    {
                        Vector3 dirToRight = (newWorldPosition - bone.rightBone.worldPosition).normalized;
                        float lengthToRight = bone.transform.TransformVector(bone.rightPosition).magnitude;
                        expectedPosition += bone.rightBone.worldPosition + dirToRight * lengthToRight;
                        lengthConstraints++;
                    }
                }
                expectedPosition /= lengthConstraints;
                newWorldPosition = Vector3.Lerp(expectedPosition, newWorldPosition, bone.slackness / iterations);

                // Collision
                if (bone.radius > 0)
                {
                    foreach (EZSoftBoneColliderBase collider in EZSoftBoneColliderBase.EnabledColliders)
                    {
                        if (bone.transform != collider.transform && collisionLayers.Contains(collider.gameObject.layer))
                            collider.Collide(ref newWorldPosition, bone.radius);
                    }
                    foreach (Collider collider in extraColliders)
                    {
                        if (bone.transform != collider.transform && collider.enabled)
                            EZSoftBoneUtility.PointOutsideCollider(ref newWorldPosition, collider, bone.radius);
                    }
                }

                bone.speed = (bone.speed + (newWorldPosition - oldWorldPosition) / deltaTime) * 0.5f;
                bone.worldPosition = newWorldPosition;
            }
            else
            {
                bone.worldPosition = bone.transform.position;
            }

            for (int i = 0; i < bone.childBones.Count; i++)
            {
                UpdateBones(bone.childBones[i], deltaTime);
            }
        }
        private void UpdateTransforms()
        {
            for (int i = 0; i < m_Structures.Count; i++)
            {
                m_Structures[i].UpdateTransform(siblingRotationConstraints, startDepth);
            }
        }
    }
    [RequireComponent(typeof(Collider))] public class EZSoftBoneCollider : EZSoftBoneColliderBase
    {
        [SerializeField]
        private Collider m_ReferenceCollider;
        public Collider referenceCollider
        {
            get
            {
                if (m_ReferenceCollider == null)
                    m_ReferenceCollider = GetComponent<Collider>();
                return m_ReferenceCollider;
            }
        }

        [SerializeField]
        private float m_Margin;
        public float margin { get { return m_Margin; } set { m_Margin = value; } }

        [SerializeField]
        private bool m_InsideMode;
        public bool insideMode { get { return m_InsideMode; } set { m_InsideMode = value; } }

        public override void Collide(ref Vector3 position, float spacing)
        {
            if (referenceCollider is SphereCollider)
            {
                SphereCollider collider = referenceCollider as SphereCollider;
                if (insideMode) EZSoftBoneUtility.PointInsideSphere(ref position, collider, spacing + margin);
                else EZSoftBoneUtility.PointOutsideSphere(ref position, collider, spacing + margin);
            }
            else if (referenceCollider is CapsuleCollider)
            {
                CapsuleCollider collider = referenceCollider as CapsuleCollider;
                if (insideMode) EZSoftBoneUtility.PointInsideCapsule(ref position, collider, spacing + margin);
                else EZSoftBoneUtility.PointOutsideCapsule(ref position, collider, spacing + margin);
            }
            else if (referenceCollider is BoxCollider)
            {
                BoxCollider collider = referenceCollider as BoxCollider;
                if (insideMode) EZSoftBoneUtility.PointInsideBox(ref position, collider, spacing + margin);
                else EZSoftBoneUtility.PointOutsideBox(ref position, collider, spacing + margin);
            }
            else if (referenceCollider is MeshCollider)
            {
                if (!CheckConvex(referenceCollider as MeshCollider))
                {
                    Debug.LogError("Non-Convex Mesh Collider is not supported", this);
                    enabled = false;
                    return;
                }
                if (insideMode)
                {
                    Debug.LogError("Inside Mode On Mesh Collider is not supported", this);
                    insideMode = false;
                    return;
                }
                EZSoftBoneUtility.PointOutsideCollider(ref position, referenceCollider, spacing + margin);
            }
        }

        private bool CheckConvex(MeshCollider meshCollider)
        {
            return meshCollider.sharedMesh != null && meshCollider.convex;
        }

        private void Reset()
        {
            m_ReferenceCollider = GetComponent<Collider>();
        }
    }
    public abstract class EZSoftBoneColliderBase : MonoBehaviour
    {
        public static HashSet<EZSoftBoneColliderBase> EnabledColliders = new HashSet<EZSoftBoneColliderBase>();

        protected void OnEnable()
        {
            EnabledColliders.Add(this);
        }
        protected void OnDisable()
        {
            EnabledColliders.Remove(this);
        }

        public abstract void Collide(ref Vector3 position, float spacing);
    }
    public class EZSoftBoneColliderCylinder : EZSoftBoneColliderBase
    {
        [SerializeField]
        private float m_Margin;
        public float margin { get { return m_Margin; } set { m_Margin = value; } }

        [SerializeField]
        private bool m_InsideMode;
        public bool insideMode { get { return m_InsideMode; } set { m_InsideMode = value; } }

        public override void Collide(ref Vector3 position, float spacing)
        {
            if (insideMode) EZSoftBoneUtility.PointInsideCylinder(ref position, transform, spacing + margin);
            else EZSoftBoneUtility.PointOutsideCylinder(ref position, transform, spacing + margin);
        }

        #if UNITY_EDITOR
                private void OnDrawGizmosSelected()
                {
                    Vector3 center, direction;
                    float radius, height;
                    EZSoftBoneUtility.GetCylinderParams(transform, out center, out direction, out radius, out height);
                    UnityEditor.Handles.color = Color.red;
                    UnityEditor.Handles.matrix = Matrix4x4.identity;
                    Vector3 p0 = center + direction * height;
                    Vector3 p1 = center - direction * height;
                    UnityEditor.Handles.DrawWireDisc(p0, transform.up, radius);
                    UnityEditor.Handles.DrawWireDisc(p1, transform.up, radius);
                    UnityEditor.Handles.matrix *= Matrix4x4.Translate(transform.forward * radius);
                    UnityEditor.Handles.DrawLine(p0, p1);
                    UnityEditor.Handles.matrix *= Matrix4x4.Translate(-transform.forward * 2 * radius);
                    UnityEditor.Handles.DrawLine(p0, p1);
                    UnityEditor.Handles.matrix *= Matrix4x4.Translate((transform.right + transform.forward) * radius);
                    UnityEditor.Handles.DrawLine(p0, p1);
                    UnityEditor.Handles.matrix *= Matrix4x4.Translate(-transform.right * 2 * radius);
                    UnityEditor.Handles.DrawLine(p0, p1);
                }
        #endif
    }
    [CreateAssetMenu(fileName = "SBForce", menuName = "EZSoftBone/SBForce")] public class EZSoftBoneForce : ScriptableObject
    {
        [SerializeField]
        private float m_Force = 1;
        public float force { get { return m_Force; } set { m_Force = value; } }

        public enum TurbulenceMode
        {
            Curve,
            Perlin,
        }

        [SerializeField]
        private Vector3 m_Turbulence = new Vector3(1f, 0.5f, 2f);
        public Vector3 turbulence { get { return m_Turbulence; } set { m_Turbulence = value; } }

        [SerializeField]
        private TurbulenceMode m_TurbulenceMode = TurbulenceMode.Perlin;
        public TurbulenceMode turbulenceMode { get { return m_TurbulenceMode; } set { m_TurbulenceMode = value; } }

        #region Perlin
        [SerializeField]
        private Vector3 m_Frequency = new Vector3(1f, 1f, 1.5f);
        public Vector3 frequency { get { return m_Frequency; } set { m_Frequency = value; } }
        #endregion

        #region Curve
        [SerializeField]
        private float m_TimeCycle = 2f;
        public float timeCycle { get { return m_TimeCycle; } set { m_TimeCycle = Mathf.Max(0, value); } }

        [SerializeField, EZCurveRect(0, -1, 1, 2)]
        private AnimationCurve m_CurveX = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField, EZCurveRect(0, -1, 1, 2)]
        private AnimationCurve m_CurveY = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField, EZCurveRect(0, -1, 1, 2)]
        private AnimationCurve m_CurveZ = AnimationCurve.EaseInOut(0, 1, 1, 0);
        #endregion

        public Vector3 GetForce(float time)
        {
            Vector3 tbl = turbulence;
            switch (turbulenceMode)
            {
                case TurbulenceMode.Curve:
                    time = Mathf.Repeat(time, m_TimeCycle) / m_TimeCycle;
                    tbl.x *= Curve(m_CurveX, time);
                    tbl.y *= Curve(m_CurveY, time);
                    tbl.z *= Curve(m_CurveZ, time);
                    break;
                case TurbulenceMode.Perlin:
                    tbl.x *= Perlin(time * frequency.x, 0);
                    tbl.y *= Perlin(time * frequency.y, 0.5f);
                    tbl.z *= Perlin(time * frequency.z, 1.0f);
                    break;
            }
            return new Vector3(0, 0, force) + tbl;
        }

        private float Perlin(float x, float y)
        {
            return Mathf.PerlinNoise(x, y) * 2 - 1;
        }
        private float Curve(AnimationCurve curve, float time)
        {
            return curve.Evaluate(time);
        }
    }
    public class EZSoftBoneForceField : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        private float m_Conductivity = 0.15f;
        public float conductivity { get { return m_Conductivity; } set { m_Conductivity = value; } }

        [SerializeField, EZNestedEditor]
        private EZSoftBoneForce m_Force;
        public EZSoftBoneForce force { get { return m_Force; } set { m_Force = value; } }

        public float time { get; set; }

        private void OnEnable()
        {
            time = 0;
        }
        private void Update()
        {
            time += Time.deltaTime;
        }

        public Vector3 GetForce(float normalizedLength)
        {
            return transform.TransformDirection(force.GetForce(time - conductivity * normalizedLength));
        }

        #if UNITY_EDITOR
                private void OnDrawGizmosSelected()
                {
                    DrawGizmos();
                }
                public void DrawGizmos()
                {
                    if (force == null || !isActiveAndEnabled) return;
                    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
                    if (Application.isPlaying)
                    {
                        Vector3 forceVector = force.GetForce(Time.time);
                        float width = forceVector.magnitude * 0.2f;
                        EZSoftBoneUtility.DrawGizmosArrow(Vector3.zero, forceVector, width, Vector3.up);
                        EZSoftBoneUtility.DrawGizmosArrow(Vector3.zero, forceVector, width, Vector3.right);
                        Gizmos.DrawRay(Vector3.zero, forceVector);
                    }
                    else
                    {
                        Vector3 forceVector = new Vector3(0, 0, force.force);
                        float width = force.force * 0.2f;
                        EZSoftBoneUtility.DrawGizmosArrow(Vector3.zero, forceVector, width, Vector3.up);
                        EZSoftBoneUtility.DrawGizmosArrow(Vector3.zero, forceVector, width, Vector3.right);
                        Gizmos.DrawRay(Vector3.zero, forceVector);
                    }
                    Gizmos.DrawWireCube(new Vector3(0, 0, force.force), force.turbulence * 2);
                }
        #endif
    }
    [CreateAssetMenu(fileName = "SBMat", menuName = "EZSoftBone/SBMaterial")]
    public class EZSoftBoneMaterial : ScriptableObject
    {
        [SerializeField, Range(0, 1)]
        private float m_Damping = 0.2f;
        public float damping { get { return m_Damping; } set { m_Damping = Mathf.Clamp01(value); } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_DampingCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 1);
        public AnimationCurve dampingCurve { get { return m_DampingCurve; } }

        [SerializeField, Range(0, 1)]
        private float m_Stiffness = 0.1f;
        public float stiffness { get { return m_Stiffness; } set { m_Stiffness = Mathf.Clamp01(value); } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_StiffnessCurve = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve stiffnessCurve { get { return m_StiffnessCurve; } }

        [SerializeField, Range(0, 1)]
        private float m_Resistance = 0.9f;
        public float resistance { get { return m_Resistance; } set { m_Resistance = Mathf.Clamp01(value); } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_ResistanceCurve = AnimationCurve.Linear(0, 1, 1, 0);
        public AnimationCurve resistanceCurve { get { return m_ResistanceCurve; } }

        [SerializeField, Range(0, 1)]
        private float m_Slackness = 0.1f;
        public float slackness { get { return m_Slackness; } set { m_Slackness = Mathf.Clamp01(value); } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_SlacknessCurve = AnimationCurve.Linear(0, 1, 1, 0.8f);
        public AnimationCurve slacknessCurve { get { return m_SlacknessCurve; } }

        private static EZSoftBoneMaterial m_DefaultMaterial;
        public static EZSoftBoneMaterial defaultMaterial
        {
            get
            {
                if (m_DefaultMaterial == null)
                    m_DefaultMaterial = CreateInstance<EZSoftBoneMaterial>();
                m_DefaultMaterial.name = "SBMat_Default";
                return m_DefaultMaterial;
            }
        }

        public float GetDamping(float t)
        {
            return damping * dampingCurve.Evaluate(t);
        }
        public float GetStiffness(float t)
        {
            return stiffness * stiffnessCurve.Evaluate(t);
        }
        public float GetResistance(float t)
        {
            return resistance * resistanceCurve.Evaluate(t);
        }
        public float GetSlackness(float t)
        {
            return slackness * slacknessCurve.Evaluate(t);
        }
    }
    public static class EZSoftBoneUtility
    {
        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
        public static float Max(this Vector3 v)
        {
            return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask | (1 << layer)) == mask;
        }

        public static void GetCapsuleParams(CapsuleCollider collider, out Vector3 center0, out Vector3 center1, out float radius)
        {
            Vector3 scale = collider.transform.lossyScale.Abs();
            radius = collider.radius;
            center0 = center1 = collider.center;
            float height = collider.height * 0.5f;
            switch (collider.direction)
            {
                case 0:
                    radius *= Mathf.Max(scale.y, scale.z);
                    height = Mathf.Max(0, height - radius / scale.x);
                    center0.x -= height;
                    center1.x += height;
                    break;
                case 1:
                    radius *= Mathf.Max(scale.x, scale.z);
                    height = Mathf.Max(0, height - radius / scale.y);
                    center0.y -= height;
                    center1.y += height;
                    break;
                case 2:
                    radius *= Mathf.Max(scale.x, scale.y);
                    height = Mathf.Max(0, height - radius / scale.z);
                    center0.z -= height;
                    center1.z += height;
                    break;
            }
            center0 = collider.transform.TransformPoint(center0);
            center1 = collider.transform.TransformPoint(center1);
        }
        public static void GetCylinderParams(Transform transform, out Vector3 center, out Vector3 direction, out float radius, out float height)
        {
            Vector3 size = transform.lossyScale.Abs();
            center = transform.position;
            direction = transform.up;
            radius = Mathf.Max(size.x, size.z) * 0.5f;
            height = size.y;
        }

        public static void PointOutsideSphere(ref Vector3 position, SphereCollider collider, float spacing)
        {
            Vector3 scale = collider.transform.lossyScale.Abs();
            float radius = collider.radius * scale.Max();
            PointOutsideSphere(ref position, collider.transform.TransformPoint(collider.center), radius + spacing);
        }
        private static void PointOutsideSphere(ref Vector3 position, Vector3 spherePosition, float radius)
        {
            Vector3 bounceDir = position - spherePosition;
            if (bounceDir.magnitude < radius)
            {
                position = spherePosition + bounceDir.normalized * radius;
            }
        }

        public static void PointInsideSphere(ref Vector3 position, SphereCollider collider, float spacing)
        {
            PointInsideSphere(ref position, collider.transform.TransformPoint(collider.center), collider.radius - spacing);
        }
        private static void PointInsideSphere(ref Vector3 position, Vector3 spherePosition, float radius)
        {
            Vector3 bounceDir = position - spherePosition;
            if (bounceDir.magnitude > radius)
            {
                position = spherePosition + bounceDir.normalized * radius;
            }
        }

        public static void PointOutsideCapsule(ref Vector3 position, CapsuleCollider collider, float spacing)
        {
            Vector3 center0, center1;
            float radius;
            GetCapsuleParams(collider, out center0, out center1, out radius);
            PointOutsideCapsule(ref position, center0, center1, radius + spacing);
        }
        private static void PointOutsideCapsule(ref Vector3 position, Vector3 center0, Vector3 center1, float radius)
        {
            Vector3 capsuleDir = center1 - center0;
            Vector3 pointDir = position - center0;

            float dot = Vector3.Dot(capsuleDir, pointDir);
            if (dot <= 0)
            {
                PointOutsideSphere(ref position, center0, radius);
            }
            else if (dot >= capsuleDir.sqrMagnitude)
            {
                PointOutsideSphere(ref position, center1, radius);
            }
            else
            {
                Vector3 bounceDir = pointDir - Vector3.Project(pointDir, capsuleDir);
                float bounceDis = radius - bounceDir.magnitude;
                if (bounceDis > 0)
                {
                    position += bounceDir.normalized * bounceDis;
                }
            }
        }

        public static void PointInsideCapsule(ref Vector3 position, CapsuleCollider collider, float spacing)
        {
            Vector3 center0, center1;
            float radius;
            GetCapsuleParams(collider, out center0, out center1, out radius);
            PointInsideCapsule(ref position, center0, center1, radius - spacing);
        }
        private static void PointInsideCapsule(ref Vector3 position, Vector3 center0, Vector3 center1, float radius)
        {
            Vector3 capsuleDir = center1 - center0;
            Vector3 pointDir = position - center0;

            float dot = Vector3.Dot(capsuleDir, pointDir);
            if (dot <= 0)
            {
                PointInsideSphere(ref position, center0, radius);
            }
            else if (dot >= capsuleDir.sqrMagnitude)
            {
                PointInsideSphere(ref position, center1, radius);
            }
            else
            {
                Vector3 bounceDir = pointDir - Vector3.Project(pointDir, capsuleDir);
                float bounceDis = radius - bounceDir.magnitude;
                if (bounceDis < 0)
                {
                    position += bounceDir.normalized * bounceDis;
                }
            }
        }

        public static void PointOutsideCylinder(ref Vector3 position, Transform transform, float spacing)
        {
            Vector3 center, direction;
            float radius, height;
            GetCylinderParams(transform, out center, out direction, out radius, out height);
            PointOutsideCylinder(ref position, center, direction, radius + spacing, height + spacing);
        }
        private static void PointOutsideCylinder(ref Vector3 position, Vector3 center, Vector3 direction, float radius, float height)
        {
            Vector3 pointDir = position - center;
            Vector3 directionAlong = Vector3.Project(pointDir, direction);
            float distanceAlong = height - directionAlong.magnitude;
            if (distanceAlong > 0)
            {
                Vector3 directionSide = pointDir - directionAlong;
                float distanceSide = radius - directionSide.magnitude;
                if (distanceSide > 0)
                {
                    if (distanceSide < distanceAlong)
                    {
                        position += directionSide.normalized * distanceSide;
                    }
                    else
                    {
                        position += directionAlong.normalized * distanceAlong;
                    }
                }
            }
        }

        public static void PointInsideCylinder(ref Vector3 position, Transform transform, float spacing)
        {
            GetCylinderParams(transform, out Vector3 center, out Vector3 direction, out float radius, out float height);
            PointInsideCylinder(ref position, center, direction, radius - spacing, height - spacing);
        }
        private static void PointInsideCylinder(ref Vector3 position, Vector3 center, Vector3 direction, float radius, float height)
        {
            Vector3 pointDir = position - center;
            Vector3 directionAlong = Vector3.Project(pointDir, direction);
            float distanceAlong = height - directionAlong.magnitude;
            Vector3 directionSide = pointDir - directionAlong;
            float distanceSide = radius - directionSide.magnitude;
            if (distanceAlong < 0 || distanceSide < 0)
            {
                if (distanceSide < distanceAlong)
                {
                    position += directionSide.normalized * distanceSide;
                }
                else
                {
                    position += directionAlong.normalized * distanceAlong;
                }
            }
        }

        public static void PointOutsideBox(ref Vector3 position, BoxCollider collider, float spacing)
        {
            Vector3 positionToCollider = collider.transform.InverseTransformPoint(position) - collider.center;
            PointOutsideBox(ref positionToCollider, collider.size.Abs() / 2 + collider.transform.InverseTransformVector(Vector3.one * spacing).Abs());
            position = collider.transform.TransformPoint(collider.center + positionToCollider);
        }
        private static void PointOutsideBox(ref Vector3 position, Vector3 boxSize)
        {
            Vector3 distanceToCenter = position.Abs();
            if (distanceToCenter.x < boxSize.x && distanceToCenter.y < boxSize.y && distanceToCenter.z < boxSize.z)
            {
                Vector3 distance = (distanceToCenter - boxSize).Abs();
                if (distance.x < distance.y)
                {
                    if (distance.x < distance.z)
                    {
                        position.x = Mathf.Sign(position.x) * boxSize.x;
                    }
                    else
                    {
                        position.z = Mathf.Sign(position.z) * boxSize.z;
                    }
                }
                else
                {
                    if (distance.y < distance.z)
                    {
                        position.y = Mathf.Sign(position.y) * boxSize.y;
                    }
                    else
                    {
                        position.z = Mathf.Sign(position.z) * boxSize.z;
                    }
                }
            }
        }

        public static void PointInsideBox(ref Vector3 position, BoxCollider collider, float spacing)
        {
            Vector3 positionToCollider = collider.transform.InverseTransformPoint(position) - collider.center;
            PointInsideBox(ref positionToCollider, collider.size.Abs() / 2 - collider.transform.InverseTransformVector(Vector3.one * spacing).Abs());
            position = collider.transform.TransformPoint(collider.center + positionToCollider);
        }
        private static void PointInsideBox(ref Vector3 position, Vector3 boxSize)
        {
            Vector3 distanceToCenter = position.Abs();
            if (distanceToCenter.x > boxSize.x) position.x = Mathf.Sign(position.x) * boxSize.x;
            if (distanceToCenter.y > boxSize.y) position.y = Mathf.Sign(position.y) * boxSize.y;
            if (distanceToCenter.z > boxSize.z) position.z = Mathf.Sign(position.z) * boxSize.z;
        }

        public static void PointOutsideCollider(ref Vector3 position, Collider collider, float spacing)
        {
            Vector3 closestPoint = collider.ClosestPoint(position);
            if (position == closestPoint) // inside collider
            {
                Vector3 bounceDir = position - collider.bounds.center;
                Debug.DrawLine(collider.bounds.center, closestPoint, Color.red);
                position = closestPoint + bounceDir.normalized * spacing;
            }
            else
            {
                Vector3 bounceDir = position - closestPoint;
                if (bounceDir.magnitude < spacing)
                {
                    position = closestPoint + bounceDir.normalized * spacing;
                }
            }
        }

        public static void DrawGizmosArrow(Vector3 startPoint, Vector3 direction, float halfWidth, Vector3 normal)
        {
            Vector3 sideDir = Vector3.Cross(direction, normal).normalized * halfWidth;
            Vector3[] vertices = new Vector3[8];
            vertices[0] = startPoint + sideDir * 0.5f;
            vertices[1] = vertices[0] + direction * 0.5f;
            vertices[2] = vertices[1] + sideDir * 0.5f;
            vertices[3] = startPoint + direction;
            vertices[4] = startPoint - sideDir + direction * 0.5f;
            vertices[5] = vertices[4] + sideDir * 0.5f;
            vertices[6] = startPoint - sideDir * 0.5f;
            vertices[7] = vertices[0];
            DrawGizmosPolyLine(vertices);
        }
        public static void DrawGizmosPolyLine(params Vector3[] vertices)
        {
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Gizmos.DrawLine(vertices[i], vertices[i + 1]);
            }
        }
    }
    public class EZCurveRectAttribute : PropertyAttribute
    {
        public Rect rect;
        public Color color = Color.green;

        public EZCurveRectAttribute()
        {
            this.rect = new Rect(0, 0, 1, 1);
        }
        public EZCurveRectAttribute(Rect rect)
        {
            this.rect = rect;
        }
        public EZCurveRectAttribute(float x, float y, float width, float height)
        {
            this.rect = new Rect(x, y, width, height);
        }
        public EZCurveRectAttribute(Rect rect, Color color)
        {
            this.rect = rect;
            this.color = color;
        }
        public EZCurveRectAttribute(float x, float y, float width, float height, Color color)
        {
            this.rect = new Rect(x, y, width, height);
            this.color = color;
        }
    }
    public class EZNestedEditorAttribute : PropertyAttribute
    {
    }

    // since there's no license, i found one here https://github.com/EZhex1991/EZSoftBone/pull/8
    /*
     * MIT License
     *
     * Copyright (c) 2022 Ethan Zack
     * 
     * Permission is hereby granted, free of charge, to any person obtaining a copy
     * of this software and associated documentation files (the "Software"), to deal
     * in the Software without restriction, including without limitation the rights
     * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
     * copies of the Software, and to permit persons to whom the Software is
     * furnished to do so, subject to the following conditions:
     * The above copyright notice and this permission notice shall be included in all
     * copies or substantial portions of the Software.
     * 
     * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
     * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
     * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
     * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
     * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
     * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
     * SOFTWARE.
     */
}

﻿using System.Collections.Generic;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(RobotSimulatorBaseController))]
    public class RobotSimulatorBaseControllerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
#endif

    public class RobotSimulatorBaseController : MonoBehaviour, IRobot
    {
        [SerializeField]
        protected GameObject jointRoot = null;

        [SerializeField]
        protected GameObject modelRoot = null;

        [SerializeField]
        protected GameObject clothModel = null;

        [SerializeField]
        protected JointController jointController = null;

        [SerializeField]
        protected float jointRotateScale = 3;

        [SerializeField]
        protected float spring = 3;

        [SerializeField]
        protected float damper = 2;

        [SerializeField]
        private AxisRangeView _axisRangeViewPrefab = null;

        [SerializeField]
        protected GameObject positionMarkerPrefab = null;

        protected Alter3EveCameraController eyeCameraLeft = null;
        public Alter3EveCameraController EyeCameraLeft
        {
            get { return this.eyeCameraLeft; }
        }
        protected Alter3EveCameraController eyeCameraRight = null;
        public Alter3EveCameraController EyeCameraRight
        {
            get { return this.eyeCameraRight; }
        }

        protected readonly Dictionary<string, JointParameter> dictionary = new Dictionary<string, JointParameter>();

        protected readonly Dictionary<string, PositionMarkerController> positionMarkers = new Dictionary<string, PositionMarkerController>();

        private readonly Dictionary<string, Transform> _jointTransforms = new Dictionary<string, Transform>();
        private readonly Dictionary<int, float> _axisValues = new Dictionary<int, float>();
        private readonly Dictionary<int, AxisRangeView[]> _axisViewLists = new Dictionary<int, AxisRangeView[]>();

        private readonly List<CollisionEventController> collisionEventLists = new List<CollisionEventController>();

        private readonly Vector3[] _positions = new Vector3[3];

        public void ResetAxes()
        {
            foreach (var jointParameter in dictionary.Values)
            {
                jointParameter.NextRotation = jointParameter.DefaultRotation;
            }
        }

        public void ToggleAxisRangeView(int axisNumber)
        {
            if (_axisViewLists.TryGetValue(axisNumber, out var axisViews))
            {
                _axisViewLists.Remove(axisNumber);
                foreach (var axisView in axisViews)
                {
                    Destroy(axisView.gameObject);
                }
            }
            else
            {
                var joints = jointController.GetItem(axisNumber);
                axisViews = new AxisRangeView[joints.Length];
                for (var i = 0; i < joints.Length; i++)
                {
                    var joint = joints[i];
                    var jointTransform = FindJoint(joint.JointName);
                    var axisView = Instantiate(_axisRangeViewPrefab, jointTransform.parent, false);
                    axisView.transform.localPosition = jointTransform.localPosition;
                    axisView.Label = $"{axisNumber}-{joint.JointName}";
                    axisView.Axis = joint.Axis;
                    axisView.AngleMin = joint.rangeMin;
                    axisView.AngleMax = joint.rangeMax;
                    axisViews[i] = axisView;
                }
                _axisViewLists.Add(axisNumber, axisViews);
            }
        }

        public float GetAxisValue(int axisNumber)
        {
            if (_axisValues.TryGetValue(axisNumber, out float value))
            {
                return value;
            }
            return 128;
        }

        /// <summary>
        /// 両手の座標を配列で返す
        /// </summary>
        /// <returns>左手の座標値と右手の座標値からなる要素数6の配列</returns>
        public IReadOnlyList<Vector3> GetHandsPositionArray()
        {
            var leftHand = positionMarkers["LeftHand"];
            var rightHand = positionMarkers["RightHand"];
            var head = positionMarkers["Head"];

            _positions[0] = leftHand.GetWorldPosition() - transform.position;
            _positions[1] = rightHand.GetWorldPosition() - transform.position;
            _positions[2] = head.GetWorldPosition() - transform.position;
            return _positions;
        }

        protected virtual void Awake()
        {
            CreateJointParameter();
            CreateCollision();
            EnableUpdateWhenOffscreenForAllRenderers();

            AttachPositionMarker("LeftHand", new Vector3(0, -0.2f, 0));
            AttachPositionMarker("RightHand", new Vector3(0, -0.2f, 0));
            AttachPositionMarker("Head", new Vector3(0, 0.2f, 0));
        }

        protected void AttachPositionMarker(string jointName, Vector3 offset)
        {
            var obj = Instantiate(positionMarkerPrefab);

            var controller = obj.GetComponent<PositionMarkerController>();

            var jointTransform = FindJoint(jointName);

            controller.gameObject.transform.localPosition = offset;
            controller.gameObject.transform.SetParent(jointTransform, false);
            obj.name = "PositionMarker_" + jointName;
            positionMarkers.Add(jointName, controller);



        }

        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            UpdateJointParameter();
        }

        protected virtual void OnDestroy()
        {

        }

        void CreateJointParameter()
        {
            var joints = jointRoot.GetComponentsInChildren<Transform>();
            foreach (var joint in joints)
            {
                _jointTransforms.Add(joint.name, joint);
            }

            foreach (var jointTableEntity in jointController.JointTableEntities)
            {
                foreach (var jointItem in jointTableEntity.JointItems)
                {
                    if (!dictionary.ContainsKey(jointItem.JointName))
                    {
                        var param = new JointParameter(jointItem, FindJoint(jointItem.JointName));
                        dictionary.Add(param.Name, param);
                    }
                }
            }
        }

        void CreateCollision()
        {
            var transforms = modelRoot.GetComponentsInChildren<Transform>();
            foreach (var transform in transforms)
            {
                transform.gameObject.AddComponent<CapsuleCollider>();
                var collitionEvent = transform.gameObject.AddComponent<CollisionEventController>();
                collisionEventLists.Add(collitionEvent);
            }
            DisableCollisionCheck();
        }

        public void SetCollisionCheckEnabled(bool enabled)
        {
            if (enabled)
            {
                EnableCollisionCheck();
            }
            else
            {
                DisableCollisionCheck();
            }
        }
        public void SetClothModelEnabled(bool enabled)
        {
            if (enabled)
            {
                EnableClothModel();
            }
            else
            {
                DisableClothModel();
            }
        }

        void EnableCollisionCheck()
        {
            foreach (var collisionEvent in collisionEventLists)
            {
                collisionEvent.EnableCollisionCheck();
            }
        }

        void DisableCollisionCheck()
        {
            foreach (var collisionEvent in collisionEventLists)
            {
                collisionEvent.DisableCollisionCheck();
            }
        }

        public void EnableClothModel()
        {
            clothModel?.SetActive(true);
        }
        public void DisableClothModel()
        {
            clothModel?.SetActive(false);
        }



        private void EnableUpdateWhenOffscreenForAllRenderers()
        {
            foreach (var renderer in modelRoot.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.updateWhenOffscreen = true;
            }
        }

        protected Transform FindJoint(string name)
        {
            return _jointTransforms[name];
        }

        protected JointParameter FindJointParameter(string name)
        {
            return dictionary[name];
        }

        protected void UpdateJointParameter()
        {
            var dt = Time.deltaTime;
            foreach (var param in dictionary.Values)
            {
                param.Update(spring, damper, dt);

                /*
                //以前のプログラム
                //                if (Quaternion.Angle(param.CurrentQuat, param.NextQuat) <= 1)
                                {
                 //                   param.CurrentQuat = param.NextQuat;
                                }
                 //               else
                                {
                                    param.CurrentQuat = Quaternion.Lerp(param.CurrentQuat, param.NextQuat, Time.deltaTime * jointRotateScale);
                                }
                                param.Transform.localRotation = param.CurrentQuat;
                */
            }
        }

        ///回転方向が時計回りか
        ///
        private bool IsRotateClockwise(float current, float next)
        {
            return next > current ? !(next - current > 180f)
                          : current - next > 180f;
        }
        protected void UpdateJoint(int axisNum, float value)
        {

            //            var leftHandWorldPosition = positionMarkers["LeftHand"].GetWorldPosition();
            //            Debug.Log("LeftHand " + leftHandWorldPosition.ToString());


            var jointItem = jointController.GetItem(axisNum);

            _axisViewLists.TryGetValue(axisNum, out var axisViews);

            var normalizedValue = value / 255f;

            foreach (var item in jointItem)
            {
                var param = FindJointParameter(item.JointName);
                param.UpdateTargetRotation(normalizedValue);
            }

            if (axisViews != null)
            {
                foreach (var axisView in axisViews)
                {
                    axisView.CurrentAngleRatio = normalizedValue;
                }
            }
            _axisValues[axisNum] = value;
        }



        #region IRobot
        public void MoveAxis(AxisParam axisParam)
        {

            UpdateJoint(axisParam.AxisNumber, (float)axisParam.Value);

            /*
            var jointItems = jointController.GetItem(axisParam.AxisNumber);

            foreach(var joint in jointItems)
            {
                Debug.Log("joint name : " + joint.JointName);
            }
            */
        }

        public void MoveAxes(AxisParam[] axisParams)
        {
            foreach (var axisParam in axisParams)
            {
                MoveAxis(axisParam);
            }
        }



        #endregion



    }
}

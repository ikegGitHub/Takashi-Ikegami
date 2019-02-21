using System.Collections.Generic;
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

        protected Dictionary<string, JointParameter> dictionary = new Dictionary<string, JointParameter>();

        [SerializeField]
        protected JointController jointController = null;

        [SerializeField]
        protected float jointRotateScale = 3;

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

        private readonly Dictionary<int, float> _axisValues = new Dictionary<int, float>();

        public void ResetAxes()
        {
            foreach (var jointParameter in dictionary.Values)
            {
                jointParameter.NextQuat = Quaternion.Euler(jointParameter.DefaultRotation);
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

        protected virtual void Awake()
        {
            CreateJointParameter();
            CreateCollision();

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

                var param = new JointParameter(joint);
                dictionary.Add(joint.name, param);

                //                var rigidBody = joint.gameObject.AddComponent<Rigidbody>();
                //                rigidBody.useGravity = false;

            }
        }

        void CreateCollision()
        {
            var transforms = modelRoot.GetComponentsInChildren<Transform>();
            foreach (var transform in transforms)
            {
                transform.gameObject.AddComponent<CapsuleCollider>();
                transform.gameObject.AddComponent<CollisionEventController>();
            }

        }


        protected Transform FindJoint(string name)
        {
            var trans = dictionary[name].Transform;
            return trans;
        }

        protected JointParameter FindJointParameter(string name)
        {
            var param = dictionary[name];
            return param;
        }

        protected void UpdateJointParameter()
        {
            foreach (var param in dictionary.Values)
            {
                //                var qx = Quaternion.AngleAxis(param.NextRotation.x, Vector3.right);
                //                var qy = Quaternion.AngleAxis(param.NextRotation.y, Vector3.up);
                //                var qz = Quaternion.AngleAxis(param.NextRotation.z, Vector3.forward);
                if (Quaternion.Angle(param.CurrentQuat, param.NextQuat) <= 1)
                {
                    param.CurrentQuat = param.NextQuat;
                }
                else
                {
                    param.CurrentQuat = Quaternion.Lerp(param.CurrentQuat, param.NextQuat, Time.deltaTime * jointRotateScale);
                }



                //                param.CurrentRotation = Vector3.Lerp(param.CurrentRotation, param.NextRotation, Time.deltaTime * jointRotateScale);
                //param.Transform.localRotation = Quaternion.Euler(param.CurrentRotation);


                param.Transform.localRotation = param.CurrentQuat;

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

            var jointItem = jointController.GetItem(axisNum);

            foreach (var item in jointItem)
            {
                var param = FindJointParameter(item.JointName);

                //                param.BeforeValue = param.CurrentValue;
                //               param.CurrentValue = value;

                var t = value / 255f;
                var ang = Mathf.Lerp(item.rangeMin, item.rangeMax, t);
                //                param.CurrentRotation = param.CurrentQuat.eulerAngles;
                param.StartRotation = param.CurrentRotation;
                var newRotation = item.Axis * ang;
                float ax = 0;
                float ay = 0;
                float az = 0;
                Quaternion qx = Quaternion.identity;
                Quaternion qy = Quaternion.identity;
                Quaternion qz = Quaternion.identity;
                if (item.Axis.x != 0)
                {
                    qx = Quaternion.AngleAxis(newRotation.x, Vector3.right);
                    ax = newRotation.x;
                }
                else
                {
                    qx = Quaternion.AngleAxis(param.CurrentRotation.x, Vector3.right);
                    ax = param.CurrentRotation.x;
                }
                if (item.Axis.y != 0)
                {
                    qy = Quaternion.AngleAxis(newRotation.y, Vector3.up);
                    ay = newRotation.y;
                }
                else
                {
                    qy = Quaternion.AngleAxis(param.CurrentRotation.y, Vector3.up);
                    ay = param.CurrentRotation.y;
                }
                if (item.Axis.z != 0)
                {
                    qz = Quaternion.AngleAxis(newRotation.z, Vector3.forward);
                    az = newRotation.z;
                }
                else
                {
                    qz = Quaternion.AngleAxis(param.CurrentRotation.z, Vector3.forward);
                    az = param.CurrentRotation.z;
                }
                //                param.NextQuat = qz * qx * qy;
                param.NextQuat = qx * qz * qy;
                param.NextRotation = new Vector3(ax, ay, az);
                param.CurrentRotation = param.NextRotation;
                //                param.Transform.localRotation = param.CurrentQuat;
            }

            _axisValues[axisNum] = value;
        }



        #region IRobot
        public void MoveAxis(AxisParam axisParam)
        {
#if DEBUG
            Debug.LogWarning("MoveAxis : " + axisParam.AxisNumber.ToString() + " value: " + axisParam.Value.ToString() + ": " + Time.realtimeSinceStartup.ToString());
#endif

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

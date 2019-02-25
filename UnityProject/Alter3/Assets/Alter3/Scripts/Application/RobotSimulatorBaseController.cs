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

        [SerializeField]
        protected float spring = 3;

        [SerializeField]
        protected float damper = 2;
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
            EnableUpdateWhenOffscreenForAllRenderers();
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

        private void EnableUpdateWhenOffscreenForAllRenderers()
        {
            foreach (var renderer in modelRoot.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.updateWhenOffscreen = true;
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
                //なんとなくスプリング＋ダンパー
                var dt = Time.deltaTime;

                var acceleration = ((param.NextRotation - param.CurrentRotation) * spring) - (param.Velocity * damper);
                param.Velocity += acceleration * dt;
                param.CurrentRotation += param.Velocity * dt;


                var qx = Quaternion.AngleAxis(param.CurrentRotation.x, Vector3.right);
                var qy = Quaternion.AngleAxis(param.CurrentRotation.y, Vector3.up);
                var qz = Quaternion.AngleAxis(param.CurrentRotation.z, Vector3.forward);

                param.Transform.localRotation = qx * qz * qy;


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

#if DEBUG || UNITY_EDITOR
                Debug.LogWarning("MoveAxis : " + axisNum.ToString() + " value: " + value.ToString() + "  " + newRotation.ToString() + ": " + Time.realtimeSinceStartup.ToString());
#endif

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
                //                param.NextQuat = qz * qx * qy;
                param.NextQuat = qx * qz * qy;
                param.NextRotation = new Vector3(ax, ay, az);
//                param.CurrentRotation = param.NextRotation;
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

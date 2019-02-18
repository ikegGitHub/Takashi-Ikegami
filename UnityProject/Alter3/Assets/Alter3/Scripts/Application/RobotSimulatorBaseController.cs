using System.Collections;
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

        public void ResetAxes()
        {
            foreach (var jointParameter in dictionary.Values)
            {
                jointParameter.NextQuat = Quaternion.Euler(jointParameter.DefaultRotation);
            }
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
        //        float ang = 0;
        int indexJoint15 = 0;
        void UpdateJoint15()
        {
            float[] valueArray = new float[]{
                 101,
                 153,
                 200,
                 216,
                 236,
                 252,
                 254,
                 249,
                 176,
                 87,
                 51,
                 46,
                 46,
                 82,
                 150,
                 166,
                 171,
                 176,
                 197,
                 218,
                 69,
                 43,
                 0,
                 0,
                 5,
                 47,
                 78,
                 88,
                 99,
                 109,
                 114,
                 120,
                 130,
                 135,
                 141,
                 146,
                 161,
                 172,
                 177,
                 182,
                 187,
                 182,
                 167,
                 156,
                 146,
                 141,
                 130,
             };
            UpdateJoint(15, valueArray[indexJoint15]);
            indexJoint15++;
            if (indexJoint15 >= valueArray.Length)
            {
                indexJoint15 = 0;
            }


        }

        int indexJoint16 = 0;
        void UpdateJoint16()
        {
            float[] valueArray = new float[]
            {
                170,
                175,
                191,
                206,
                201,
                191,
                180,
                149,
                87,
                66,
                61,
                56,
                61,
                71,
                87,
                92,
                102,
                108,
                118,
                134,
                149,
                154,
                154,
                149,
                139,
                128,
                118,
                113,
                108,
                102,
                97,
                87,
                82,
                87,
                102,
                118,
                134,
                139,
                149,
                154,
                165,
                180,
                175,
                165,
                154,
                144,
                139,
                128,
                123,
                118,
                113,
                108,
                102,
                97,
                92,
                76,
                61,
                56,
                61,
                71,
                87,
                97,
                108,
                123,
                144,
                149,
                149,
                144,
                134,
                123,
            };

            UpdateJoint(16, valueArray[indexJoint16]);
            indexJoint16++;
            if (indexJoint16 >= valueArray.Length)
            {
                indexJoint16 = 0;
            }


        }


        // Update is called once per frame
        protected virtual void Update()
        {
            UpdateJointParameter();
            //            UpdateJoint(16, 123);

            //            UpdateJoint15();
            //           UpdateJoint16();




#if false
            //            UpdateJoint(19, ang);
            //            UpdateJoint(18, ang);
            //             UpdateJoint(11,ang);
            //            UpdateJoint(43, ang);


            ang += 1;
            if (ang > 360)
            {
                ang -= 360f;
            }
#endif


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
            List<string> keyList = new List<string>(dictionary.Keys);
            foreach (var key in keyList)
            {
                var param = dictionary[key];


//                var qx = Quaternion.AngleAxis(param.NextRotation.x, Vector3.right);
//                var qy = Quaternion.AngleAxis(param.NextRotation.y, Vector3.up);
//                var qz = Quaternion.AngleAxis(param.NextRotation.z, Vector3.forward);
                var nextQuat = param.NextQuat;  // qy * qz * qx;



                param.CurrentQuat = Quaternion.Lerp(param.CurrentQuat, nextQuat, Time.deltaTime * jointRotateScale);
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

                param.BeforeValue = param.CurrentValue;
                param.CurrentValue = value;

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
//                param.NextQuat = qy * qz * qx;
                param.NextRotation = new Vector3(ax, ay, az);
                param.CurrentRotation = param.NextRotation;
                //                param.CurrentRotation = new Vector3(ax, ay, az);
                //                param.Transform.localRotation = Quaternion.Euler(param.CurrentRotation);    // Quaternion.AngleAxis(ang, item.Axis);
            }

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

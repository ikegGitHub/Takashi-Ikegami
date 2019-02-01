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
        protected Dictionary<string, JointParameter> dictionary = new Dictionary<string, JointParameter>();

        [SerializeField]
        protected JointController jointController = null;

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


        protected virtual void Awake()
        {
            var joints = jointRoot.GetComponentsInChildren<Transform>();
            foreach (var joint in joints)
            {

                var param = new JointParameter(joint);
                dictionary.Add(joint.name, param);
            }

        }
        // Use this for initialization
        protected virtual void Start()
        {

        }
        //        float ang = 0;

        // Update is called once per frame
        protected virtual void Update()
        {
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

        protected void UpdateJoint(int axisNum, float value)
        {
            var jointItem = jointController.GetItem(axisNum);

            foreach (var item in jointItem)
            {
                var param = FindJointParameter(item.JointName);
                var t = (float)value / 255;
                var ang = Mathf.Lerp(item.rangeMin, item.rangeMax, t);
                var newRotation = item.Axis * ang;
                float ax = 0;
                float ay = 0;
                float az = 0;
                if (newRotation.x != 0)
                {
                    ax = newRotation.x;
                }
                else
                {
                    ax = param.CurrentRotation.x;
                }
                if (newRotation.y != 0)
                {
                    ay = newRotation.y;
                }
                else
                {
                    ay = param.CurrentRotation.y;
                }
                if (newRotation.z != 0)
                {
                    az = newRotation.z;
                }
                else
                {
                    az = param.CurrentRotation.z;
                }
                param.CurrentRotation = new Vector3(ax, ay, az);
                param.Transform.localRotation = Quaternion.Euler(param.CurrentRotation);    // Quaternion.AngleAxis(ang, item.Axis);
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

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
        protected List<JointParameter> jointLists = new List<JointParameter>();

        protected virtual void Awake()
        {
            var joints = jointRoot.GetComponentsInChildren<Transform>();
            foreach (var joint in joints)
            {
                var param = new JointParameter(joint);
                jointLists.Add(param);
            }

        }
        // Use this for initialization
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void OnDestroy()
        {

        }


        #region IRobot
        public void MoveAxis(AxisParam axisParam)
        {

        


        }

        public void MoveAxes(AxisParam[] axisParams)
        {

        }



        #endregion



    }
}

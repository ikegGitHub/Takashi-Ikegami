using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFlag.Alter3Simulator
{

    public class Alter3SimulatorController : RobotSimulatorBaseController
    {
        [SerializeField]
        protected GameObject eyeCameraPrefab = null;

        [SerializeField]
        protected GameObject rigidBodyPredab = null;




        protected List<Alter3RigidBodyController> rigidlists = new List<Alter3RigidBodyController>();




        protected override void Awake()
        {
            base.Awake();
            CreateEyeCamera("LeftEye", EyeCameraPos.Left);
            CreateEyeCamera("RightEye", EyeCameraPos.Right);

            CreateRigdBody("LeftHand");
            CreateRigdBody("RightHand");
        }
        // Use this for initialization
        protected override void Start()
        {
            base.Start();

        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();


        }

        protected void CreateEyeCamera(string jointName, EyeCameraPos eyePos)
        {
            var param = FindJointParameter(jointName);
            if (eyePos == EyeCameraPos.Left)
            {
                eyeCameraLeft = Instantiate(eyeCameraPrefab).GetComponent<Alter3EveCameraController>();
                eyeCameraLeft.gameObject.transform.SetParent(param.Transform, false);
                eyeCameraLeft.gameObject.transform.localPosition = Vector3.zero;
                eyeCameraLeft.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                eyeCameraLeft.EyeCameraPos = eyePos;


            }
            else if (eyePos == EyeCameraPos.Right)
            {
                eyeCameraRight = Instantiate(eyeCameraPrefab).GetComponent<Alter3EveCameraController>();
                eyeCameraRight.gameObject.transform.SetParent(param.Transform, false);
                eyeCameraRight.gameObject.transform.localPosition = Vector3.zero;
                eyeCameraRight.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                eyeCameraRight.EyeCameraPos = eyePos;
            }

        }

        protected void CreateRigdBody(string jointName)
        {
            var param = FindJointParameter(jointName);
            var controller = Instantiate(rigidBodyPredab).GetComponent<Alter3RigidBodyController>();
            controller.gameObject.transform.SetParent(param.Transform, false);
            controller.gameObject.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            controller.gameObject.transform.rotation = Quaternion.identity;
            controller.Name = jointName;
            controller.OnEvent += OnEventRigidBody;
            rigidlists.Add(controller);
        }

        void OnEventRigidBody(Alter3RigidBodyController controller)
        {

        }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFlag.Alter3Simulator
{

    public class Alter3SimulatorController : RobotSimulatorBaseController
    {
        [SerializeField]
        protected GameObject eyeCameraPrefab = null;

        protected enum EyePos
        {
            Left,
            Right,
        }




        protected override void Awake()
        {
            base.Awake();
            CreateEyeCamera("LeftEye", EyePos.Left);
            CreateEyeCamera("RightEye", EyePos.Right);
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

        protected void CreateEyeCamera(string jointName, EyePos eyePos)
        {
            var param = FindJointParameter(jointName);
            if (eyePos == EyePos.Left)
            {
                eyeCameraLeft = Instantiate(eyeCameraPrefab).GetComponent<Alter3EveCameraController>();
                eyeCameraLeft.gameObject.transform.SetParent(param.Transform, false);
                eyeCameraLeft.gameObject.transform.localPosition = Vector3.zero;
                eyeCameraLeft.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));


            }
            else if (eyePos == EyePos.Right)
            {
                eyeCameraRight = Instantiate(eyeCameraPrefab).GetComponent<Alter3EveCameraController>();
                eyeCameraRight.gameObject.transform.SetParent(param.Transform, false);
                eyeCameraLeft.gameObject.transform.localPosition = Vector3.zero;
                eyeCameraLeft.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }



        }


    }
}


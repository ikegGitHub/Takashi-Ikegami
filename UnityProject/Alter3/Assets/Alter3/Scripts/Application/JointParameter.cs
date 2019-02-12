using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [System.Serializable]
    public class JointParameter
    {
        public string Name = "";
        public int AxisNum = 0;
        public Vector3 DefaultPosition = Vector3.zero;
        public Vector3 DefaultRotation = Vector3.zero;
        public Vector3 CurrentRotation = Vector3.zero;
        public Vector3 NextRotation = Vector3.zero;
        public Vector3 StartRotation = Vector3.zero;
        public Transform Transform = null;
        public Vector3 AxisVector = Vector3.zero;
//        public Vector3 AddRotationSpeed = Vector3.zero;
        public float CurrentValue = 128;
        public float BeforeValue = 128;
//        public float RotationTime = 3f;            //360度回転する時間
//        public float MaxAccelerationSpeed = 30f;       //加速度（１秒間に加算する角度）
 //       public float RotationSpeed = 0;
 //       public float AccelerationSpeed = 0;
//        public float MaxRotationSpeed = 0;

        /// <summary>
        /// 使用禁止
        /// </summary>
        private JointParameter()
        {

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="trans">Trans.</param>
        public JointParameter(Transform trans)
        {
            Transform = trans;
            Name = trans.name;
            DefaultPosition = trans.localPosition;
            DefaultRotation = trans.localRotation.eulerAngles;
            CurrentRotation = DefaultRotation;
            AxisVector = Quaternion.Euler(DefaultRotation) * Vector3.right;

//            MaxRotationSpeed = RotationTime / 360f;
 //           RotationSpeed = 0;


        }
    }
}



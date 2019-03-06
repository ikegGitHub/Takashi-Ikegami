﻿using System.Collections;
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
        public Quaternion CurrentQuat = Quaternion.identity;
        public Quaternion NextQuat = Quaternion.identity;
        public Vector3 NextRotation = Vector3.zero;
        public Vector3 StartRotation = Vector3.zero;
        public Transform Transform = null;
        public Vector3 AxisVector = Vector3.zero;

        public Vector3 Velocity = Vector3.zero;
        //        public Vector3 AddRotationSpeed = Vector3.zero;
        //        public float CurrentValue = 128;
        //        public float BeforeValue = 128;
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

        public void UpdateTargetRotation(JointItem jointItem, float normalizedValue)
        {
            var ang = Mathf.Lerp(jointItem.rangeMin, jointItem.rangeMax, normalizedValue);

            StartRotation = CurrentRotation;
            var newRotation = jointItem.Axis * ang;

            float ax = 0;
            float ay = 0;
            float az = 0;
            Quaternion qx = Quaternion.identity;
            Quaternion qy = Quaternion.identity;
            Quaternion qz = Quaternion.identity;
            if (jointItem.Axis.x != 0)
            {
                qx = Quaternion.AngleAxis(newRotation.x, Vector3.right);
                ax = newRotation.x;
            }
            else
            {
                qx = Quaternion.AngleAxis(CurrentRotation.x, Vector3.right);
                ax = CurrentRotation.x;
            }
            if (jointItem.Axis.z != 0)
            {
                qz = Quaternion.AngleAxis(newRotation.z, Vector3.forward);
                az = newRotation.z;
            }
            else
            {
                qz = Quaternion.AngleAxis(CurrentRotation.z, Vector3.forward);
                az = CurrentRotation.z;
            }
            if (jointItem.Axis.y != 0)
            {
                qy = Quaternion.AngleAxis(newRotation.y, Vector3.up);
                ay = newRotation.y;
            }
            else
            {
                qy = Quaternion.AngleAxis(CurrentRotation.y, Vector3.up);
                ay = CurrentRotation.y;
            }
            NextQuat = qx * qz * qy;
            NextRotation = new Vector3(ax, ay, az);
        }

        public void Update(float spring, float damper, float dt)
        {
            // なんとなくスプリング＋ダンパー
            var acceleration = ((NextRotation - CurrentRotation) * spring) - (Velocity * damper);
            Velocity += acceleration * dt;
            CurrentRotation += Velocity * dt;

            var qx = Quaternion.AngleAxis(CurrentRotation.x, Vector3.right);
            var qy = Quaternion.AngleAxis(CurrentRotation.y, Vector3.up);
            var qz = Quaternion.AngleAxis(CurrentRotation.z, Vector3.forward);

            Transform.localRotation = qx * qz * qy;
        }
    }
}

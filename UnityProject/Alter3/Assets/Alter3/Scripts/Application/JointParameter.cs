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
        public Transform Transform = null;
        public Vector3 AxisVector = Vector3.zero;


        /// <summary>
        /// 使用禁止
        /// </summary>
        private JointParameter()
        {

        }

        public JointParameter(Transform trans)
        {
            Transform = trans;
            Name = trans.name;
            DefaultPosition = trans.localPosition;
            DefaultRotation = trans.localRotation.eulerAngles;
            CurrentRotation = DefaultRotation;
            AxisVector = Quaternion.Euler(DefaultRotation) * Vector3.right;
        }
    }
}



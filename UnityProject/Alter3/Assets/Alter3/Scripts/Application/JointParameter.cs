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
        public Vector3 Position = Vector3.zero;
        public Vector3 Rotation = Vector3.zero;
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
            Position = trans.localPosition;
            Rotation = trans.localRotation.eulerAngles;
            AxisVector = Quaternion.Euler(Rotation) * Vector3.right;
        }
    }
}



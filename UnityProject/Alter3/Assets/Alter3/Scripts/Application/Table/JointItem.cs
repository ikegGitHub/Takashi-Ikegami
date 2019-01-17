using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [Serializable]
    public class JointItem
    {
        /// <summary>
        /// ジョイント名
        /// </summary>
        public string JointName;
        /// <summary>
        /// 回転軸方向
        /// </summary>
        public Vector3 Axis = Vector3.up;
        public int rangeMin = -180;
        public int rangeMax = 180;

    }
}

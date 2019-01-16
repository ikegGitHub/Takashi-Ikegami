using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class JointController : MonoBehaviour
    {
        [SerializeField]
        JointTable jointTable = null;

        public JointItem[] GetItem(int axisNum)
        {
            var entity = jointTable?.GetEntity(axisNum);
            return entity.JointItems;
        }


    }
}

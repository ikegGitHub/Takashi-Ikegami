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
    [CreateAssetMenu(fileName = "new JointTable", menuName = "Alter3/Table/JointTable")]
    public class JointTable : TableBase<JointTableEntity, int>
    {
    }
}

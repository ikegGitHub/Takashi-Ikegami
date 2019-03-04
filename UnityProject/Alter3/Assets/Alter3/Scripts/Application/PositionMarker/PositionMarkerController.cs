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
    public class PositionMarkerController : MonoBehaviour
    {

        public Vector3 GetWorldPosition()
        {
            return this.transform.position;
        }
    }
}

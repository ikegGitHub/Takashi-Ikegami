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
    public class Alter3RigidBodyController : MonoBehaviour
    {
        public Action<Alter3RigidBodyController> OnEvent = null;

        protected string name;
        public string Name
        {
            set { this.name = value; }
        }
        private void OnCollisionEnter(Collision collision)
        {
            OnEvent?.Invoke(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnEvent?.Invoke(this);

        }

    }
}

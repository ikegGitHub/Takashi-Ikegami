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
        public enum CollisionState
        {
            Enter,
            Stay,
            Exit,


        }
        public Action<Alter3RigidBodyController> OnEvent = null;

        protected string name = null;
        public string Name
        {
            set { this.name = value; }
            get { return this.name;}
        }

        private void OnTriggerEnter(Collider other)
        {
            OnEvent?.Invoke(this);

        }
        private void OnTriggerStay(Collider other)
        {
            OnEvent?.Invoke(this);

        }
        private void OnTriggerExit(Collider other)
        {
            OnEvent?.Invoke(this);

        }

    }
}

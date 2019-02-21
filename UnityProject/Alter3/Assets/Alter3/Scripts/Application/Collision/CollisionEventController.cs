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
    public class CollisionEventController : MonoBehaviour
    {
        public Action<CollisionEventController> OnEvent = null;
        private Material renderMaterial = null;
        private void Awake()
        {
            var mesh = this.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (mesh != null)
            {
                renderMaterial = mesh.material;
            }

        }

        private void SetColor(Color color)
        {
            renderMaterial?.SetColor("_Color", color);
            //            renderMaterial?.SetColor("_SpecColor", color);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(CollisionTagID.Hand.ToString()))
            {
                SetColor(Color.red);
            }

        }
        private void OnTriggerStay(Collider other)
        {

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(CollisionTagID.Hand.ToString()))
            {
                SetColor(Color.white);
            }

        }


    }
}

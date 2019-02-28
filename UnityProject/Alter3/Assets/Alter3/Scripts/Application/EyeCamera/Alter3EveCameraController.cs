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
    public class Alter3EveCameraController : MonoBehaviour
    {
        protected RenderTexture renderTexture = null;
        public RenderTexture RenderTexture
        {
            get { return this.renderTexture; }
        }

        [SerializeField]
        protected int resolutionX = 1024;
        [SerializeField]
        protected int resolutionY = 1024;
        [SerializeField]
        protected float fov = 60f;

        [SerializeField]
        protected Camera camera = null;

        private void Awake()
        {
            renderTexture = new RenderTexture(resolutionX, resolutionY, 24, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            camera.targetTexture = renderTexture;
        }


    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using System.Collections;
using System.IO;

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


        protected EyeCameraPos eyePos = EyeCameraPos.Left;
        public EyeCameraPos EyeCameraPos
        {
            get { return this.eyePos; }
            set { this.eyePos = value; }
        }


        private void Awake()
        {
            renderTexture = new RenderTexture(resolutionX, resolutionY, 24, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            camera.targetTexture = renderTexture;
        }

        int framecount = 0;

        void SaveTexture()
        {
            Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            Destroy(tex);

            string path = Application.dataPath + "/Capture";
            path = Path.Combine(path, "Screen" + framecount.ToString() + ".png");

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            File.WriteAllBytes(path, bytes);




        }


        private void Update()
        {
#if !UNITY_EDITOR
//            SaveTexture();
#endif
            framecount++;
        }
    }
}

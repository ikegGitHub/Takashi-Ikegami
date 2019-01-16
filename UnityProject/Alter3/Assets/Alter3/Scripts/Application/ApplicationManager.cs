using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ApplicationManager : SingletonMonoBehaviour<ApplicationManager>
    {

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            Screen.SetResolution(1280, 720, false, 60);
#else
			Screen.SetResolution(1280, 720, false, 60);
#endif
            //Don't Vsync 必要?
            Application.targetFrameRate = 60;
        }


        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }



        void OnEnable()
        {
        }

        void OnDisable()
        {
        }

        void OnApplicationPause(bool isPause)
        {
        }

        void OnApplicationQuit()
        {
        }




    }
}

using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ApplicationManager : SingletonMonoBehaviour<ApplicationManager>
    {
        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR || UNITY_STANDALONE
            Screen.SetResolution(1280, 720, false, 60);
#else
            Screen.SetResolution(1280, 720, false, 60);
#endif
            //Don't Vsync 必要?
            Application.targetFrameRate = 60;
        }
    }
}

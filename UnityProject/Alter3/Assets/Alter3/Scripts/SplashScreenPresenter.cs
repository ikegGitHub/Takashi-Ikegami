using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class SplashScreenPresenter : MonoBehaviour
    {
        private static readonly string MainSceneName = "Server";
        private static readonly string ThisSceneName = "Splash";

        [SerializeField]
        private Image _backgroundImage = null;

        [SerializeField]
        private Image _logoImage = null;

        private IEnumerator Start()
        {
            var asyncOp = SceneManager.LoadSceneAsync(MainSceneName, LoadSceneMode.Additive);

            yield return AnimateAlpha(_logoImage, 0, 1, 0.5f);

            yield return new WaitForSeconds(1.0f);
            yield return asyncOp;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MainSceneName));

            yield return AnimateAlpha(_logoImage, 1, 0, 0.5f);

            yield return AnimateAlpha(_backgroundImage, 1, 0, 0.5f);

            yield return SceneManager.UnloadSceneAsync(ThisSceneName);
        }

        private static IEnumerator AnimateAlpha(Graphic graphic, float a, float b, float duration)
        {
            float t = 0;
            while (t < duration)
            {
                SetAlpha(graphic, Mathf.Lerp(a, b, t / duration));
                t += Time.deltaTime;
                yield return null;
            }
            SetAlpha(graphic, b);
        }

        private static void SetAlpha(Graphic graphic, float alpha)
        {
            var c = graphic.color;
            c.a = alpha;
            graphic.color = c;
        }
    }
}

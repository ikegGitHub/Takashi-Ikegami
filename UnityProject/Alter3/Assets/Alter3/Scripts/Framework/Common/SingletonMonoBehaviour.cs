using UnityEngine;

/// <summary>
/// MonoBehaviourをシングルトンクラスとして扱いたい場合に使用してください。
/// base.Awakeの呼び忘れを防ぐためにAwakeは仮想メソッドとしていません。テンプレートを使用してファイルを生成する事を推奨
/// </summary>
/// <typeparam name="T">このクラスを継承するクラス自身</typeparam>
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected static T instance = null;


		public virtual void Awake()
		{
			if (instance != null)
			{
				Debug.LogWarning(typeof(T).FullName + " Awake 時に _instance が null ではありません。");
			}
			instance = (T)(object)this;
		}

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = (T)FindObjectOfType(typeof(T));
					if (instance == null)
					{
//						Debug.LogWarning(typeof(T).FullName + " の _instance が null です。");
					}
				}
				return instance;
			}
		}

		public virtual void OnDestroy()
		{
//			Debug.Log(typeof(T).FullName + ".OnDestroy()");
			instance = null;
		}
	}




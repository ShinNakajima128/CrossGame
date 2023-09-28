using UnityEngine;

/// <summary>
/// シングルトンの機能を持つコンポーネント。継承での使用のみとする
/// </summary>
/// <typeparam name="T">シングルトンとするコンポーネント</typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    #region property
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "がシーンに存在しません。");
                }
            }

            return instance;
        }
    }
    protected abstract bool IsDontDestroyOnLoad { get; }
    #endregion

    #region private
    private static T instance;
    #endregion

    #region unity method
    protected virtual void Awake()
    {
        //既に存在している場合は自身を消去
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (IsDontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion
}
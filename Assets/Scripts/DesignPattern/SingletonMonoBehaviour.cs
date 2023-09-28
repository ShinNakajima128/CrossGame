using UnityEngine;

/// <summary>
/// �V���O���g���̋@�\�����R���|�[�l���g�B�p���ł̎g�p�݂̂Ƃ���
/// </summary>
/// <typeparam name="T">�V���O���g���Ƃ���R���|�[�l���g</typeparam>
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
                    Debug.LogError(typeof(T) + "���V�[���ɑ��݂��܂���B");
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
        //���ɑ��݂��Ă���ꍇ�͎��g������
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
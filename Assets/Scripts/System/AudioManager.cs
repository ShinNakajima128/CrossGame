using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// オーディオ機能を管理するコンポーネント
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    #region property
    public int CurrentBGMVolume { get; set; } = 5;
    public int CurrentSEVolume { get; set; } = 5;

    protected override bool IsDontDestroyOnLoad => true;
    #endregion

    #region serialize
    [Header("各音量")]
    [SerializeField, Range(0f, 1f)]
    float _masterVolume = 1.0f;

    [SerializeField, Range(0f, 1f)]
    float _bgmVolume = 0.3f;

    [SerializeField, Range(0f, 1f)]
    float _seVolume = 1.0f;

    [SerializeField, Range(0f, 1f)]
    float _environmentVolume = 1f;

    [Header("AudioSourceの生成数")]
    [Tooltip("SEのAudioSourceの生成数")]
    [SerializeField]
    int _seAudioSourceNum = 5;

    [Tooltip("SEのAudioSourceの生成数")]
    [SerializeField]
    int _voiceAudioSourceNum = 5;

    [Header("各音源リスト")]
    [SerializeField]
    List<BGM> _bgmList = new List<BGM>();

    [SerializeField]
    List<SE> _seList = new List<SE>();

    [Header("使用する各オブジェクト")]
    [Tooltip("BGM用のAudioSource")]
    [SerializeField]
    AudioSource _bgmSource = default;

    [Tooltip("SE用のAudioSourceをまとめるオブジェクト")]
    [SerializeField]
    Transform _seSourcesParent = default;

    [Tooltip("環境音用のAudioSource")]
    [SerializeField]
    AudioSource _environmentSource = default;

    [Tooltip("AudioMixer")]
    [SerializeField]
    AudioMixer _mixer = default;
    #endregion

    #region private
    List<AudioSource> _seAudioSourceList = new List<AudioSource>();
    bool _isStoping = false;
    #endregion


    protected override void Awake()
    {
        base.Awake();

        //指定した数のSE用AudioSourceを生成
        for (int i = 0; i < _seAudioSourceNum; i++)
        {
            //SEAudioSourceのオブジェクトを生成し、親オブジェクトにセット
            var obj = new GameObject($"SESource{i + 1}");
            obj.transform.SetParent(_seSourcesParent);

            //生成したオブジェクトにAudioSourceを追加
            var source = obj.AddComponent<AudioSource>();
            
            if (_mixer != null)
            {
                source.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master")[2];
            }
            _seAudioSourceList.Add(source);
        }
    }

    #region play method
    /// <summary>
    /// BGMを再生
    /// </summary>
    /// <param name="type"> BGMの種類 </param>
    public static void PlayBGM(BGMType type, bool loopType = true)
    {
        var bgm = GetBGM(type);

        if (bgm != null)
        {
            if (Instance._bgmSource.clip == null)
            {
                Instance._bgmSource.clip = bgm.Clip;
                Instance._bgmSource.loop = loopType;
                Instance._bgmSource.volume = Instance._bgmVolume * Instance._masterVolume * bgm.Volume;
                Instance._bgmSource.Play();
                Debug.Log($"{bgm.BGMName}を再生");

            }
            else
            {
                Instance.StartCoroutine(Instance.SwitchingBgm(bgm, loopType));
                Debug.Log($"{bgm.BGMName}を再生");
            }

        }
        else
        {
            Debug.LogError($"BGM:{type}を再生できませんでした");
        }
    }

    /// <summary>
    /// SEを再生
    /// </summary>
    /// <param name="type"> SEの種類 </param>
    public static void PlaySE(SEType type)
    {
        var se = GetSE(type);

        if (se != null)
        {
            foreach (var s in Instance._seAudioSourceList)
            {
                if (!s.isPlaying)
                {
                    s.PlayOneShot(se.Clip, Instance._seVolume * Instance._masterVolume * se.Volume);
                    //Debug.Log($"{se.SEName}を再生");
                    return;
                }
            }
        }
        else
        {
            Debug.LogError($"SE:{type}を再生できませんでした");
        }
    }

    /// <summary>
    /// BGMを再生
    /// </summary>
    /// <param name="type"> BGMの種類 </param>
    public static void ChangePlayEnviroment(bool isPlaying)
    {
        if (isPlaying)
        {
            Instance._environmentSource.volume = Instance._environmentVolume * Instance._masterVolume;
            Instance._environmentSource.Play();
        }
        else
        {
            Instance._environmentSource.Stop();
        }
        
    }
    #endregion

    #region stop method
    /// <summary>
    /// 再生中のBGMを停止する
    /// </summary>
    public static void StopBGM()
    {
        Instance._bgmSource.Stop();
        Instance._bgmSource.clip = null;
    }

    /// <summary>
    /// 再生中のBGMの音量徐々に下げて停止する
    /// </summary>
    /// <param name="stopTime"> 停止するまでの時間 </param>
    public static void StopBGM(float stopTime)
    {
        Instance.StartCoroutine(Instance.LowerVolume(stopTime));
    }
    /// <summary>
    /// 再生中のSEを停止する
    /// </summary>
    public static void StopSE()
    {
        foreach (var s in Instance._seAudioSourceList)
        {
            s.Stop();
            s.clip = null;
        }
    }
    #endregion
    
    #region volume Method
    /// <summary>
    /// マスター音量を変更する
    /// </summary>
    /// <param name="masterValue"> 音量 </param>
    public static void MasterVolChange(float masterValue)
    {
        Instance._masterVolume = masterValue;
        Instance._bgmSource.volume = Instance._bgmVolume * Instance._masterVolume;
    }

    /// <summary>
    /// BGM音量を変更する
    /// </summary>
    /// <param name="bgmVolume"> 音量 </param>
    public static void BgmVolChange(float bgmVolume)
    {
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(Mathf.Clamp(bgmVolume, 0f, 1f)) * 20f, -80f, 0f);
        Debug.Log($"音量をdBに変換:{Mathf.Log10(bgmVolume) * 20f}");
        //audioMixerに代入
        Instance._mixer.SetFloat("BGM", volume);
       
        //Instance._bgmVolume = bgmValue;
        //Instance._bgmSource.volume = Instance._bgmVolume * Instance._masterVolume;
    }

    /// <summary>
    /// SE音量を変更する
    /// </summary>
    /// <param name="seVolume"> 音量 </param>
    public static void SeVolChange(float seVolume)
    {
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(Mathf.Clamp(seVolume, 0f, 1f)) * 20f, -80f, 0f);
        Debug.Log($"音量をdBに変換:{Mathf.Log10(seVolume) * 20f}");
        //audioMixerに代入
        Instance._mixer.SetFloat("SE", volume);
        
        //Instance._seVolume = seValue;
        //foreach (var s in Instance._seAudioSourceList)
        //{
        //    s.volume = Instance._seVolume;
        //}
    }

    /// <summary>
    /// BGMを徐々に変更する
    /// </summary>
    /// <param name="afterBgm"> 変更後のBGM </param>
    IEnumerator SwitchingBgm(BGM afterBgm, bool loopType = true)
    {
        _isStoping = false;
        float currentVol = _bgmSource.volume;

        while (_bgmSource.volume > 0)　//現在の音量を0にする
        {
            _bgmSource.volume -= Time.deltaTime * 1.5f;
            yield return null;
        }

        _bgmSource.clip = afterBgm.Clip;　//BGMの入れ替え
        _bgmSource.loop = loopType;
        _bgmSource.Play();

        while (_bgmSource.volume < currentVol)　//音量を元に戻す
        {
            _bgmSource.volume += Time.deltaTime * 1.5f;
            yield return null;
        }
        _bgmSource.volume = currentVol;
    }

    /// <summary>
    /// 音量を徐々に下げて停止するコルーチン
    /// </summary>
    /// <param name="time"> 停止するまでの時間 </param>
    IEnumerator LowerVolume(float time)
    {
        float currentVol = _bgmSource.volume;
        _isStoping = true;
        
        while (_bgmSource.volume > 0)　//現在の音量を0にする
        {
            _bgmSource.volume -= Time.deltaTime * currentVol / time;

            //途中でBGM等が変更された場合は処理を中断
            if (!_isStoping)
            {
                yield break;
            }
            yield return null;
        }

        _isStoping = false;
        Instance._bgmSource.Stop();
        Instance._bgmSource.clip = null;
    }
    #endregion

    #region get method
    /// <summary>
    /// BGMを取得
    /// </summary>
    /// <param name="type"> BGMの種類 </param>
    /// <returns> 指定したBGM </returns>
    static BGM GetBGM(BGMType type)
    {
        var bgm = Instance._bgmList.FirstOrDefault(b => b.BGMType == type);
        return bgm;
    }
    /// <summary>
    /// SEを取得
    /// </summary>
    /// <param name="type"> SEの種類 </param>
    /// <returns> 指定したSE </returns>
    static SE GetSE(SEType type)
    {
        var se = Instance._seList.FirstOrDefault(s => s.SEType == type);
        return se;
    }
    #endregion
}

[Serializable]
public class BGM
{
    public string BGMName;
    public BGMType BGMType;
    public AudioClip Clip;
    [Range(0, 1)]
    public float Volume = 1f;
}
[Serializable]
public class SE
{
    public string SEName;
    public SEType SEType;
    public AudioClip Clip;
    [Range(0,1)]
    public float Volume = 1f;
}

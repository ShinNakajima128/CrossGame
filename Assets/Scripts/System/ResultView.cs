using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// リザルト画面の表示機能を持つコンポーネント
/// </summary>
public class ResultView : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Header("Variables")]
    [SerializeField]
    private float _mainItemViewInterval = 1.0f;

    [SerializeField]
    private float _subItemViewInterval = 0.5f;

    [SerializeField]
    private int _hitchhikerScoreScale = 5000;

    [SerializeField]
    private TimeScoreScale[] _timeScoreScales = default;

    [Header("UIObjects")]
    [SerializeField]
    private TextMeshProUGUI _currentScoreTMP = default;

    [SerializeField]
    private TextMeshProUGUI _hitchhikerAmountTMP = default;

    [SerializeField]
    private TextMeshProUGUI _hitchhikerScoreTMP = default;

    [SerializeField]
    private TextMeshProUGUI _clearTimeTMP = default;

    [SerializeField]
    private TextMeshProUGUI _clearTimeScoreTMP = default;

    [SerializeField]
    private TextMeshProUGUI _totalScoreTMP = default;

    [SerializeField]
    private Button _retryButton = default;

    [SerializeField]
    private Button _titleButton = default;

    [SerializeField]
    private CanvasGroup _resultGroup = default;
    #endregion

    #region private
    private bool _isButtonClicked = false;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Start()
    {
        Initialize();
    }
    #endregion

    #region public method
    /// <summary>
    /// リザルトを表示する
    /// </summary>
    public void OnResultView()
    {
        StartCoroutine(ResultCoroutine());
    }
    #endregion

    #region private method
    private void Initialize()
    {
        _currentScoreTMP.text = "";
        _hitchhikerAmountTMP.text = "";
        _hitchhikerScoreTMP.text = "";
        _clearTimeTMP.text = "";
        _clearTimeScoreTMP.text = "";
        _totalScoreTMP.text = "";

        _retryButton.OnClickAsObservable()
                    .TakeUntilDestroy(this)
                    .Where(_ => !_isButtonClicked && _resultGroup.alpha == 1)
                    .Subscribe(_ =>
                    {
                        OnButtonClick(ResultButtonType.Retry);
                    });

        _titleButton.OnClickAsObservable()
                    .TakeUntilDestroy(this)
                    .Where(_ => !_isButtonClicked && _resultGroup.alpha == 1)
                    .Subscribe(_ =>
                    {
                        OnButtonClick(ResultButtonType.Title);
                    });

        _retryButton.gameObject.SetActive(false);
        _titleButton.gameObject.SetActive(false);
    }
    /// <summary>
    /// リザルト画面のボタンのイベント処理
    /// </summary>
    /// <param name="type">ボタンの種類</param>
    private void OnButtonClick(ResultButtonType type)
    {
        _isButtonClicked = true;
        GameManager.Instance.OnGameReset();


        FadeManager.Fade(FadeType.Out, () =>
        {
            FadeManager.Fade(FadeType.In);

            switch (type)
            {
                case ResultButtonType.Retry:
                    GameManager.Instance.OnGameReStart();
                    //GameManager.Instance.ChangeViewGroup(GameState.InGame);
                    break;
                case ResultButtonType.Title:
                    CameraManager.Instance.ChangeCamera(CameraType.Title, 0f);
                    GameManager.Instance.ChangeGameState(GameState.Title);
                    GameManager.Instance.ChangeViewGroup(GameState.Title);
                    break;
                default:
                    break;
            }
            _isButtonClicked = false;
        });
    }

    /// <summary>
    /// クリア時間に応じたスコアを算出する
    /// </summary>
    /// <param name="resultTime">クリアタイム</param>
    /// <returns>スコア</returns>
    private int CalclateTimeScore(float resultTime)
    {
        int score = _timeScoreScales[0].ScoreScale;

        for (int i = 0; i < _timeScoreScales.Length; i++)
        {
            Debug.Log($"スコアタイム：{_timeScoreScales[i].Time} クリアタイム{resultTime}");
            if (_timeScoreScales[i].Time < resultTime)
            {
                break;
            }
            else
            {
                score = _timeScoreScales[i].ScoreScale;
            }
        }
        Debug.Log($"クリアタイム:{resultTime}");
        return score;
    }

    /// <summary>
    /// テキストをリセットする
    /// </summary>
    private void ResetView()
    {
        _currentScoreTMP.text = "";
        _hitchhikerAmountTMP.text = "";
        _hitchhikerScoreTMP.text = "";
        _clearTimeTMP.text = "";
        _clearTimeScoreTMP.text = "";
        _totalScoreTMP.text = "";
        _retryButton.gameObject.SetActive(false);
        _titleButton.gameObject.SetActive(false);
    }
    #endregion

    #region coroutine method
    private IEnumerator ResultCoroutine()
    {
        ResetView();

        int currentScore = ScoreManager.Instance.CurrentScore;
        int currentHitchhikerAmount = HitchhikerManager.Instance.CurrentHitchhikerAmount;
        float currentProgressTime = TimeManager.Instance.CurrentProgressTime;

        int currentHitchhikerScore = 0;
        int currentProgressTimeScore = 0;

        yield return new WaitForSeconds(_mainItemViewInterval);
        
        _currentScoreTMP.text = $"{currentScore}";
        AudioManager.PlaySE(SEType.Result_TextView);

        yield return new WaitForSeconds(_mainItemViewInterval);

        _hitchhikerAmountTMP.text = $"{currentHitchhikerAmount}人";
        AudioManager.PlaySE(SEType.Result_TextView);

        yield return new WaitForSeconds(_subItemViewInterval);

        currentHitchhikerScore = _hitchhikerScoreScale * currentHitchhikerAmount;
        _hitchhikerScoreTMP.text = $"+{currentHitchhikerScore}";
        AudioManager.PlaySE(SEType.Result_TextView);

        yield return new WaitForSeconds(_mainItemViewInterval);

        _clearTimeTMP.text = $"{((int)currentProgressTime / 60).ToString().PadLeft(2, '0')}:{((int)currentProgressTime % 60).ToString().PadLeft(2, '0')}:{((currentProgressTime - Mathf.FloorToInt(currentProgressTime)) * 100).ToString("F0").PadLeft(2, '0')}";
        AudioManager.PlaySE(SEType.Result_TextView);

        yield return new WaitForSeconds(_subItemViewInterval);

        currentProgressTimeScore = CalclateTimeScore(currentProgressTime);
        _clearTimeScoreTMP.text = $"+{currentProgressTimeScore}";
        AudioManager.PlaySE(SEType.Result_TextView);

        yield return new WaitForSeconds(_mainItemViewInterval);

        _totalScoreTMP.text = $"{currentScore + currentHitchhikerScore + currentProgressTimeScore}";
        AudioManager.PlaySE(SEType.Result_TotalScoreView);

        yield return new WaitForSeconds(_mainItemViewInterval * 2);

        _retryButton.gameObject.SetActive(true);
        _titleButton.gameObject.SetActive(true);
    }
    #endregion
}
[System.Serializable]
public class TimeScoreScale
{
    public string ScoreScaleName;
    public int Time;
    public int ScoreScale;
}
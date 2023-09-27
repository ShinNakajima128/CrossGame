using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimeManager : MonoBehaviour
{
    #region property
    public static TimeManager Instance { get; private set; }
    public float CurrentProgressTime => _currentProgressTime.Value;
    #endregion

    #region serialize
    [SerializeField]
    private int _countDownTime = 3;

    [SerializeField]
    private int _limitTime = 300;

    [SerializeField]
    private TimeView _timeView = default;
    #endregion

    #region private
    private bool _isInGame = false;
    #endregion

    #region Constant
    #endregion

    #region Event
    private ReactiveProperty<float> _currentProgressTime = new ReactiveProperty<float>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Start()
    {
        GameManager.Instance.IsInGameObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value =>
                            {
                                _isInGame = value;
                            });

        GameManager.Instance.GameResetObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => Initialize());

        _currentProgressTime.TakeUntilDestroy(this)
                            .Subscribe(value => _timeView.ProgressTimeView(value));
    }
    #endregion

    #region public method
    public void OnCountDown()
    {
        StartCoroutine(CountDownCoroutine());
    }
    #endregion

    #region private method
    private void Initialize()
    {
        _timeView.CountDownView("");
        _timeView.ProgressTimeView(0);
    }
    #endregion

    #region coroutine method
    private IEnumerator CountDownCoroutine()
    {
        AudioManager.StopBGM();
        AudioManager.PlaySE(SEType.CountDown);
        GameManager.Instance.ChangeGameState(GameState.InGame);

        for (int i = 0; i < _countDownTime; i++)
        {
            _timeView.CountDownView($"{_countDownTime - i}");
            yield return new WaitForSeconds(0.8f);
        }

        _timeView.CountDownView("Start!");
        GameManager.Instance.OnChangeIsInGame(true);
        _currentProgressTime.Value = 0;
        StartCoroutine(LimitTimeCoroutine());

        yield return new WaitForSeconds(1.0f);

        AudioManager.PlayBGM(BGMType.InGame);
        _timeView.CountDownView("");
    }

    private IEnumerator LimitTimeCoroutine()
    {
        while (_isInGame)
        {
            _currentProgressTime.Value += Time.deltaTime;
            yield return null; 
        }
    }
    #endregion
}

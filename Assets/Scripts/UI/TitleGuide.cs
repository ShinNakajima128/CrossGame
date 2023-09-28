using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using DG.Tweening;

/// <summary>
/// タイトル画面のガイド用Textのコンポーネント
/// </summary>
public class TitleGuide : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    #endregion

    #region private
    private TextMeshProUGUI _guideTMP;
    private Tween _currentTween;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _guideTMP = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        OnDefaultAnimation();

        GameManager.Instance.GameStartObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => OnGameStartAnimation());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void OnDefaultAnimation()
    {
        _currentTween = _guideTMP.DOFade(0.1f, 0.8f)
                                 .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnGameStartAnimation()
    {
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
        }
        _guideTMP.DOFade(0, 0);

        _guideTMP.DOFade(1, 0.1f)
                 .SetLoops(5, LoopType.Yoyo);

        StartCoroutine(DefaultAnimationCoroutine());
    }
    #endregion
    
    #region coroutine method
    private IEnumerator DefaultAnimationCoroutine()
    {
        yield return new WaitForSeconds(3.0f);

        OnDefaultAnimation();
    }
    #endregion
}

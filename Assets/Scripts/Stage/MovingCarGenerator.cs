using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MovingCarGenerator : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _generateInterval = 1.0f;

    [SerializeField]
    private GameObject[] _movingCars = default;

    [SerializeField]
    private Transform[] _leftGeneratePoints = default;

    [SerializeField]
    private Transform[] _rightGeneratePoints = default;

    [SerializeField]
    private GenerateArea _generateArea = default;
    #endregion

    #region private
    private GeneratePointType _pointType = default;
    private bool _isGenerating = false;
    #endregion

    #region Constant
    private const int POINT_LENGTH = 3;
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {
        _generateArea.IsInAreaObserver
                     .TakeUntilDestroy(this)
                     .Subscribe(value => SwitchingGenerate(value));
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void SwitchingGenerate(bool isInArea)
    {
        if (isInArea)
        {
            int randomType = Random.Range(0, 2);
            _pointType = (GeneratePointType)randomType;
            StartCoroutine(GenerateCoroutine());
        }
        else
        {
            _isGenerating = false;
        }
    }
    #endregion

    #region coroutine method
    private IEnumerator GenerateCoroutine()
    {
        _isGenerating = true;

        while (_isGenerating)
        {
            int randomPointIndex = Random.Range(0, POINT_LENGTH);
            int randomCarIndex = Random.Range(0, _movingCars.Length);
            var car = Instantiate(_movingCars[randomCarIndex], transform);
            car.AddComponent<Obstacle.MovingCar>();

            switch (_pointType)
            {
                case GeneratePointType.Left:
                    car.transform.position = _leftGeneratePoints[randomPointIndex].position;
                    car.transform.rotation = _leftGeneratePoints[randomPointIndex].rotation;
                    break;
                case GeneratePointType.Right:
                    car.transform.position = _rightGeneratePoints[randomPointIndex].position;
                    car.transform.rotation = _rightGeneratePoints[randomPointIndex].rotation;
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(_generateInterval);
        }
    }
    #endregion
}

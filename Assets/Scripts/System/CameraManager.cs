using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    #region property
    public static CameraManager Instance { get; private set; }
    #endregion

    #region serialize
    [Header("Objects")]
    [SerializeField]
    private GameCamera[] _cameras = default;

    [SerializeField]
    private CinemachineBrain _brain = default;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region public method
    public void ChangeCamera(CameraType type, float BlendTime = 1.5f)
    {
        _brain.m_DefaultBlend.m_Time = BlendTime;
        ActiveCamera(type);
    }
    #endregion

    #region private method
    private void ActiveCamera(CameraType type)
    {
        foreach (var camera in _cameras)
        {
            camera.ViewCamera.Priority = 10;
        }

        var nextCamera = _cameras.FirstOrDefault(x => x.Type == type);
        nextCamera.ViewCamera.Priority = 15;
    }
    #endregion
    
    #region coroutine method
    #endregion
}

[System.Serializable]
public class GameCamera
{
    public string CameraName;
    public CameraType Type;
    public CinemachineVirtualCamera ViewCamera;
}

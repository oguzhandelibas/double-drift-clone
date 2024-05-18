using System.Collections;
using System.Collections.Generic;
using DoubleDrift;
using DoubleDrift.UIModule;
using UnityEngine;
using Zenject;

public class GameManager : AbstractSingleton<GameManager>
{
    [SerializeField] private Transform inGameTransform;
    [Inject] private InfinityPathManager _infinityPathManager;
    [Inject] private CarManager _carManager;
    [Inject] private CameraManager _cameraManager;
    
    private int levelIndex = 0;
    public bool gameIsStarted = false;
    
    void Awake()
    {
        gameIsStarted = false;
        Transform vehicleTransform = _carManager.Initialize(inGameTransform);
        _cameraManager.SetFollowObject(vehicleTransform.GetChild(0));
        _infinityPathManager.Initialize(levelIndex, vehicleTransform);
    }

    public int GetLevelIndex() => levelIndex;

    public void StartGame()
    {
        gameIsStarted = true;
    }
    
    public void StopGame()
    {
        gameIsStarted = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DoubleDrift;
using DoubleDrift.UIModule;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameManager : AbstractSingleton<GameManager>
{
    [SerializeField] private Transform inGameTransform;
    [Inject] private InfinityPathManager _infinityPathManager;
    [Inject] private CarManager _carManager;
    [Inject] private CameraManager _cameraManager;

    private LevelPathData _currentLevelPathData;
    
    private int levelIndex = 0;
    public bool gameIsActive = false;
    
    public void Initialize(LevelPathData levelPathData)
    {
        _currentLevelPathData = levelPathData;
        Subscribe();
        
        gameIsActive = false;
        Transform vehicleTransform = _carManager.Initialize(inGameTransform);
        _cameraManager.SetFollowObject(vehicleTransform.GetChild(0));
        _infinityPathManager.Initialize(levelPathData, vehicleTransform);
    }

    public int GetLevelIndex() => levelIndex;

    public void StartGame()
    {
        gameIsActive = true;
    }
    
    public void StopGame()
    {
        gameIsActive = false;
    }

    public void LevelSuccess()
    {
        UIManager.Instance.Show<LevelCompletedUI>();
        StopGame();
    }

    public void LevelFailed()
    {
        Debug.Log("Level Failed!");
        UIManager.Instance.Show<LevelFailedUI>();
        StopGame();
    }
    
    public void NextLevel()
    {
        StopGame();
    }
    
    public void RestartGame()
    {
        LevelSignals.Instance.onRestartLevel?.Invoke();
        Initialize(_currentLevelPathData);
    }

    #region EVENT SUBCRIBTION

    private void Subscribe()
    {
        Debug.Log("Game Manager Enabled");
        LevelSignals.Instance.onLevelSuccessful += LevelSuccess;
        LevelSignals.Instance.onLevelFailed += LevelFailed;
        LevelSignals.Instance.onNextLevel += NextLevel;
        LevelSignals.Instance.onRestartLevel += RestartGame;
    }

    private void OnDisable()
    {
        LevelSignals.Instance.onLevelSuccessful -= LevelSuccess;
        LevelSignals.Instance.onLevelFailed -= LevelFailed;
        LevelSignals.Instance.onNextLevel -= NextLevel;
        LevelSignals.Instance.onRestartLevel -= RestartGame;
    }

    #endregion
}

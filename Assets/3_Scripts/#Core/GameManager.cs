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
    [Inject] private PoliceManager _policeManager;
    [Inject] private OpponentManager _opponentManager;
    
    private LevelPathData _currentLevelPathData;
    
    private int _levelIndex = 0;
    public bool gameIsActive = false;
    
    public void Initialize(LevelPathData levelPathData, bool firstInit)
    {
        _policeManager.ResetPoliceCar();
        _opponentManager.ResetOpponentCar();
        
        _currentLevelPathData = levelPathData;
        if(!firstInit) UnSubscribe();
        Subscribe();
        
        gameIsActive = false;
        Transform vehicleTransform = _carManager.Initialize(inGameTransform, firstInit);
        _cameraManager.SetFollowObject(vehicleTransform.GetChild(0));
        _infinityPathManager.Initialize(levelPathData, firstInit);
    }

    public int GetLevelIndex() => _levelIndex;

    #region Events

    public void StartGame()
    {
        gameIsActive = true;
        _policeManager.MoveTarget(_carManager.GetCurrentCarTransform());
        _opponentManager.MoveTarget();
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
        _policeManager.CatchPlayer(_carManager.GetCurrentCarTransform());
        Debug.Log("Level Failed!");
        UIManager.Instance.Show<LevelFailedUI>();
        StopGame();
    }
    
    public void NextLevel()
    {
        StopGame();
        _levelIndex++;
        if (BootLoader.Instance.GetLevelDataCount()-1 >= _levelIndex) _levelIndex = 0;
        
        Initialize(BootLoader.Instance.GetLevelPath(_levelIndex) ,false);
        UIManager.Instance.Show<HomeUI>();
        UIManager.Instance.SetLevelIndex(_levelIndex);
    }
    
    public void RestartGame()
    {
        Initialize(_currentLevelPathData, false);
        UIManager.Instance.Show<HomeUI>();
    }

    #endregion
    

    #region EVENT SUBCRIBTION

    private void Subscribe()
    {
        Debug.Log("Game Manager Enabled");
        LevelSignals.Instance.onLevelSuccessful += LevelSuccess;
        LevelSignals.Instance.onLevelFailed += LevelFailed;
        LevelSignals.Instance.onNextLevel += NextLevel;
        LevelSignals.Instance.onRestartLevel += RestartGame;
    }

    private void UnSubscribe()
    {
        LevelSignals.Instance.onLevelSuccessful -= LevelSuccess;
        LevelSignals.Instance.onLevelFailed -= LevelFailed;
        LevelSignals.Instance.onNextLevel -= NextLevel;
        LevelSignals.Instance.onRestartLevel -= RestartGame;
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    #endregion
}

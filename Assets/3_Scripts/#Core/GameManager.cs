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

    private LevelPathData _currentLevelPathData;
    
    private int levelIndex = 0;
    public bool gameIsActive = false;
    
    public void Initialize(LevelPathData levelPathData, bool firstInit)
    {
        _currentLevelPathData = levelPathData;
        if(!firstInit) UnSubscribe();
        Subscribe();
        
        gameIsActive = false;
        Transform vehicleTransform = _carManager.Initialize(inGameTransform, firstInit);
        _cameraManager.SetFollowObject(vehicleTransform.GetChild(0));
        _infinityPathManager.Initialize(levelPathData, vehicleTransform, firstInit);
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
        levelIndex++;
        if (BootLoader.Instance.GetLevelDataCount()-1 >= levelIndex) levelIndex = 0;
        
        Initialize(BootLoader.Instance.GetLevelPath(levelIndex) ,false);
        UIManager.Instance.Show<HomeUI>();
        UIManager.Instance.SetLevelIndex(levelIndex);
    }
    
    public void RestartGame()
    {
        Initialize(_currentLevelPathData, false);
        UIManager.Instance.Show<HomeUI>();
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

using System.Collections;
using System.Collections.Generic;
using DoubleDrift.UIModule;
using UnityEngine;
using Zenject;

namespace DoubleDrift
{
    public class LevelManager : AbstractSingleton<LevelManager>
    {
        #region FIELDS

        [Inject] private GameManager _gameManager;
        [Inject] private UIManager _uiManager;
        [Inject] private LevelSignals _levelSignals;
        //private LevelData currentLevelData;

        
        #endregion

        #region UNITY FUNCTIONS

        private void Start()
        {
            _levelSignals.onLevelInitialize.Invoke();
            //currentLevelData = _gameManager.GetCurrentLevelData();
        }

        #endregion

        #region SUBSCRIBE EVENTS

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _levelSignals.onLevelInitialize += OnInitializeLevel;
            _levelSignals.onLevelSuccessful += OnLevelSuccesful;
            _levelSignals.onNextLevel += OnNextLevel;
            _levelSignals.onRestartLevel += OnRestartLevel;
            _levelSignals.onGetLevelCount += GetLevelCount;
        }


        private void UnsubscribeEvents()
        {
            _levelSignals.onLevelInitialize -= OnInitializeLevel;
            _levelSignals.onLevelSuccessful -= OnLevelSuccesful;
            _levelSignals.onNextLevel -= OnNextLevel;
            _levelSignals.onRestartLevel -= OnRestartLevel;
            _levelSignals.onGetLevelCount -= GetLevelCount;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region LEVEL FUNCTIONS

        private void OnInitializeLevel()
        {
            //_gameManager.GameHasContinue = true;
            //currentLevelData = _gameManager.GetCurrentLevelData();
            
        }

        private void OnLevelSuccesful()
        {
            //_gameManager.GameHasContinue = true;
            Debug.Log($"OnLevelSuccesful - Level Succesfull");
            _uiManager.Show<UnlockUI>();
        }

        private void OnNextLevel()
        {
            //_gameManager.NextLevel();
            //currentLevelData = _gameManager.GetCurrentLevelData();
            _gameManager.StartGame();
            _uiManager.Show<HomeUI>();
            Debug.Log($"OnNextLevel - Home UI Çağrıldı");
        }

        private void OnRestartLevel()
        {
            
        }

        private int GetLevelCount()
        {
            return _gameManager.GetLevelIndex();
        }

        #endregion
    }
}

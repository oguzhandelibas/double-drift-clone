using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DoubleDrift.UIModule
{
    public class GameUI : View
    {
        //[Inject] private GameManager _gameManager;
        //[Inject] private LevelSignals _levelSignals;
        [SerializeField] private TextMeshProUGUI levelCountText;


        public void SetLevelCountText()
        {
            //levelCountText.text = "Level " + (_gameManager.GetLevelIndex() + 1);
        }

        #region SUBSCRIBE EVENTS

        private void OnEnable()
        {
            SetLevelCountText();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            //_levelSignals.onNextLevel += SetLevelCountText;
        }


        private void UnsubscribeEvents()
        {
            //_levelSignals.onNextLevel -= SetLevelCountText;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion
    }
}
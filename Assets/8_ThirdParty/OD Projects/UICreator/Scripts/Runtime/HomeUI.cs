using UnityEngine;
using Zenject;

namespace DoubleDrift.UIModule
{
    public class HomeUI : View
    {
        [Inject] private LevelSignals _levelSignals;
        #region BUTTONS

        public void _PlayButton()
        {
            Debug.Log($"Play Button Invoked!");
            UIManager.Instance.Show<GameUI>();
            GameManager.Instance.StartGame();
            _levelSignals.onLevelInitialize.Invoke();
        }
        #endregion
    }
}

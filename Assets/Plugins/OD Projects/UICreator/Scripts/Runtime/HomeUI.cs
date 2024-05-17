using UnityEngine;
using Zenject;

namespace DoubleDrift.UIModule
{
    public class HomeUI : View
    {
        //[Inject] private LevelSignals _levelSignals;
        #region BUTTONS

        public void _StartButton()
        {
            //UIManager.Instance.Show<GameUI>();
            //_levelSignals.onLevelInitialize.Invoke();
        }
        #endregion
    }
}

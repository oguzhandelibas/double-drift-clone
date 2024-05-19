
using DoubleDrift.UIModule;
using UnityEngine;

namespace DoubleDrift
{
    public class LevelFailedUI : View
    {
        public void _TryAgain()
        {
            LevelSignals.Instance.onRestartLevel?.Invoke();
        }
    }
}


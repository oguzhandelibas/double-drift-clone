using UnityEngine;

namespace DoubleDrift.UIModule
{
    public class LevelCompletedUI : View
    {
        public void _NextLevel()
        {
            LevelSignals.Instance.onNextLevel?.Invoke();
        }
    }
}

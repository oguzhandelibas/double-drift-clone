using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleDrift.UIModule
{
    public class LevelCompletedUI : View
    {
        [SerializeField] private Image progressImage;
        [SerializeField] private TextMeshProUGUI progressPercentange;
        public void _NextLevel()
        {
            LevelSignals.Instance.onNextLevel?.Invoke();
        }

        public void SetProgress(int totalLevelCount, int currentLevelCount)
        {
            currentLevelCount++;
            Debug.Log($"CurrentLevel: {currentLevelCount}, Total Level {totalLevelCount}");
            
            float targetFillAmount = Mathf.InverseLerp(0,totalLevelCount, currentLevelCount);
            int percentange = (int)(targetFillAmount * 100);

            progressPercentange.text = $"{percentange}%";
            
            progressImage.DOFillAmount(targetFillAmount, 1).SetEase(Ease.Linear);
        }
    }
}

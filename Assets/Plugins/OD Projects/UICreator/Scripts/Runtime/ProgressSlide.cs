using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSlide : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;

    public void SetProgressSlideValue(float value)
    {
        progressSlider.DOValue(value, .1f);
    }

    public float GetProgressSlideValue()
    {
        return progressSlider.value;
    }

    public void SetProgressSlideLevelCount(int currentLevelCount)
    {
        currentLevelText.text = currentLevelCount.ToString();
        nextLevelText.text = (currentLevelCount + 1).ToString();
    }
}

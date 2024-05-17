using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressStage : MonoBehaviour
{
    [SerializeField] private Sprite filledStageSprite;
    [SerializeField] private List<Image> stages;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;

    private int stageValue = 0;
    
    public void SetProgressStageValue(int value)
    {
        if (value > 0 && value < 4)
            stageValue = value;

        for (int i = 0; i < stageValue; i++)
        {
            stages[i].sprite = filledStageSprite;
        }
    }

    public int GetProgressStageValue()
    {
        return stageValue;
    }

    public void SetProgressStageLevelCount(int currentLevelCount)
    {
        currentLevelText.text = currentLevelCount.ToString();
        nextLevelText.text = (currentLevelCount + 1).ToString();
    }
}

using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleDrift.UIModule
{
    public class ChestFill : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private RectTransform shineHolder;
        [SerializeField] private Image progressSlider;
        [SerializeField] private Sprite stageDoneSprite;
        [SerializeField] private Sprite currentStageSprite;
        [SerializeField] private Sprite uncompletedStageSprite;
        [SerializeField] private List<Image> stages;

        private int progressValue;

        public void ChestFillShine()
        {
            shineHolder.DORotate(Vector3.forward * 30, .2f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }

        public void SetChestFillRewardIcon(Sprite rewardIconSprite)
        {
            rewardIcon.sprite = rewardIconSprite;
        }

        public void SetChestFillProgressValue(int value)
        {
            if (value > 0 && value < 4)
            {
                progressValue = value;
            }

            progressSlider.DOFillAmount(progressValue * .33f, .1f);
            for (int i = 0; i < progressValue; i++)
            {
                stages[i].sprite = stageDoneSprite;
            }

            if (progressValue != 3)
            {
                stages[progressValue].sprite = currentStageSprite;
            }
        }

        public int GetChestFillProgressValue()
        {
            return progressValue;
        }
    }
}

using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DoubleDrift.UIModule
{
    public class UnlockUI : View
    {
        [SerializeField] private CoinController _coinController;
        [SerializeField] private RectTransform shineHolder;
        [SerializeField] private Image itemBackgroundIcon;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI unlockPercentageText;
        [SerializeField] private TextMeshProUGUI willGenerateCoinCount;
        [SerializeField] private Transform unlockedTextHolder;
        [SerializeField] private GameObject continueButton;

        //[Inject] private LevelSignals _levelSignals;
        private float progressValue;

        private void OnEnable()
        {
            itemIcon.gameObject.SetActive(true);
            continueButton.SetActive(true);
        }


        public void _ContinueButton()
        {
            continueButton.SetActive(false);
            itemIcon.gameObject.SetActive(false);
            willGenerateCoinCount.text = 30.ToString();
            _coinController.CreateCoins(30);
        }

        public void UnlockShine()
        {
            shineHolder.DORotate(Vector3.forward * 30, .2f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }

        public void SetUnlockItemIcon(Sprite itemIconSprite)
        {
            itemIcon.sprite = itemIconSprite;
            itemBackgroundIcon.sprite = itemIconSprite;
        }

        public void SetUnlockItemProgressValue(float value)
        {
            if (value >= 0f && value <= 1f)
            {
                progressValue = value;
            }
            else if (value > 1f)
            {
                progressValue = value / 100f;
            }

            if (Math.Abs(progressValue - 1f) < .01f)
            {
                unlockedTextHolder.gameObject.SetActive(true);
                progressValue = 1f;
            }

            itemIcon.DOFillAmount(progressValue, .1f);
            unlockPercentageText.text = "% " + ((int)(progressValue * 100));
        }

        public float GetUnlockItemProgressValue()
        {
            return progressValue;
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace DoubleDrift.UIModule
{
    public class CoinController : MonoBehaviour
    {
        #region FIELDS

        //[Inject] private LevelSignals _levelSignals;
        [SerializeField] private Transform coinParent;
        [SerializeField] private TextMeshProUGUI coinText;

        [SerializeField] private GameObject coinPrefab;

        #endregion

        #region VARIABLES

        private List<Transform> coins = new List<Transform>();
        private int _willCreateCoinAmount;
        private int _coinCount;

        #endregion

        #region COIN CONTROL

        private void Start()
        {
            _coinCount = PlayerPrefs.GetInt("CoinCount", 0);
            coinText.text = _coinCount.ToString();
        }

        private void IncreaseCoinText()
        {
            _coinCount++;
            coinText.text = _coinCount.ToString();
        }

        public void CreateCoins(int coinCount)
        {
            _willCreateCoinAmount = coinCount;

            for (int i = 0; i < _willCreateCoinAmount; i++)
            {
                Transform coinTransform = Instantiate(coinPrefab, coinParent).transform;
                coinTransform.GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(Random.Range(-150, 150), Random.Range(-150, 150)), 0.2f);
                coins.Add(coinTransform);
            }

            CountCoins();
        }


        public void CountCoins()
        {
            var delay = 0f;

            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

                coins[i].transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(400f, 840f), 0.8f)
                    .SetDelay(delay + 0.5f).SetEase(Ease.InBack).OnComplete(IncreaseCoinText);


                coins[i].transform.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f)
                    .SetEase(Ease.Flash);


                coins[i].transform.DOScale(0f, 0.3f).SetDelay(delay + 1.3f)
                    .SetEase(Ease.OutBack)
                    .OnComplete((delegate
                    {
                        Destroy(coins[i].gameObject);
                    }));

                delay += 0.1f;

                // counter.transform.parent.GetChild(0).transform.DOScale(1.1f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f);
            }

            CoinProcessDone(delay + 1.5f);
        }

        private async Task CoinProcessDone(float delay)
        {
            await Task.Delay(Mathf.RoundToInt(delay * 1000));
            //_levelSignals.onNextLevel?.Invoke();
            coins.Clear();
            PlayerPrefs.SetInt("CoinCount", _coinCount);
        }
        #endregion
    }
}

using DoubleDrift.UIModule;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DoubleDrift
{
    public class GarageUI : View
    {
        [Inject] private CarManager _carManager;
        private CarData[] _carDatas;

        [SerializeField] private Image carImage;
        [SerializeField] private Image speedAmount;
        [SerializeField] private Image accelerationAmount;
        [SerializeField] private Image handlingAmount;

        [SerializeField] private Transform nextCarButton, previousCarButton;
        private int _currentCarIndex;
        private void Start()
        {
            _currentCarIndex = 0;
            
            Initialize();
        }

        private void Initialize()
        {
            _carDatas = _carManager.GetCarDatas();
            SetCarProperties(_carDatas[0]);
        }

        private void SetCarProperties(CarData carData)
        {
            if (_currentCarIndex <= 0)
            {
                _currentCarIndex = 0;
                SetButtonActiveness(false,true);
            }
            else if (_currentCarIndex >= _carDatas.Length - 1)
            {
                _currentCarIndex = _carDatas.Length - 1;
                SetButtonActiveness(true,false);
            }
            else
            {
                SetButtonActiveness(true,true);
            }
            carImage.sprite = carData.car.image;
            
            speedAmount.fillAmount = Mathf.InverseLerp(0,200, carData.car.maxSpeed);
            accelerationAmount.fillAmount = Mathf.InverseLerp(0,100, carData.car.acceleration);
            handlingAmount.fillAmount = Mathf.InverseLerp(0,50, carData.car.handling);
        }

        private void SetButtonActiveness(bool previousBtn, bool nextBtn)
        {
            previousCarButton.transform.localScale = new Vector3(previousBtn ? -1 : 0, 1, 1);
            nextCarButton.transform.localScale = new Vector3(nextBtn ? 1 : 0, 1, 1);
        }

        #region UI BUTTONS

        public void _DriveButton()
        {
            _carManager.SetCar(_currentCarIndex);
            _ClosePanel();
        }
        
        public void _NextCarButton()
        {
            _currentCarIndex++;
            SetCarProperties(_carDatas[_currentCarIndex]);
        }
        
        public void _PreviousCarButton()
        {
            _currentCarIndex--;
            SetCarProperties(_carDatas[_currentCarIndex]);
        }

        #endregion
    }
}

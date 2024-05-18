using DG.Tweening;
using DoubleDrift.UIModule;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DoubleDrift
{
    public class CarController : MonoBehaviour, IControllable
    {
        /// <summary>
        /// TO : DO
        /// ARAÇ KALKIŞ ANINDA ÇATARA PATARA !!!
        /// DRIFT SÜRESİNE GÖRE EASE TİPİ
        /// ok DRIFT ESNASINDA KAMERA HAREKETİ
        /// </summary>

        [HideInInspector] public CarManager carManager;
        [SerializeField] private Ease _ease;
        
        [SerializeField] private Transform carTransform;
        [SerializeField] private Transform carBodyTransform;

        [SerializeField] private GameObject[] burstEffects;
        [SerializeField] private Transform[] frontWheels;
        
        public float rotationDuration = 0.5f;
        public float moveDuration = 2.5f;

        private Tween _rotateTween, _moveTween;
        private CarData _carData;
        private float _carMaxSpeed, _carAcceleration, _carHandling;
        
        public float acceleration = 5f; // Hızlanma değeri (m/s^2)
        private float _currentSpeed = 0f; // Anlık hız (m/s)
        private float currentAccelerationTime = 0f; // Anlık hızlanma süresi
        private bool isAccelerating = false; 
        public void Initialize()
        {
            _carData = carManager.GetCarData();
            _carMaxSpeed = _carData.GetCarSpeed();
            _carAcceleration = _carData.GetCarAcceleration();
            _carHandling = _carData.GetCarHandling();
            
            StartEngine();
        }
        
        void Update()
        {
            if(!GameManager.Instance.gameIsStarted) return;
            
            if (isAccelerating)
            {
                currentAccelerationTime += Time.deltaTime;
                _currentSpeed += acceleration * Time.deltaTime;
                
                if (_currentSpeed > _carMaxSpeed)
                {
                    _currentSpeed = _carMaxSpeed;
                }
            }
            else
            {
                if (_currentSpeed > 0)
                {
                    _currentSpeed -= acceleration * Time.deltaTime * 0.5f;

                    if (_currentSpeed < 0)
                    {
                        _currentSpeed = 0;
                    }
                }
            }
            carManager.CurrentCarSpeed = (int)_currentSpeed;
            UIManager.Instance.SetCarSpeed((int)_currentSpeed);
        }
        
        public void StartEngine()
        {
            isAccelerating = true;
        }

        public void StopEngine()
        {
            isAccelerating = false;
        }
        
        public void Rotate(float targetRotation)
        {
            _rotateTween = carTransform.DORotate(new Vector3(0, -targetRotation/2.25f, 0), rotationDuration, RotateMode.Fast);

            DriftEffect();
            
            if(targetRotation > 0) DriftOnLeft(targetRotation/2);
            else DriftOnRight(targetRotation/2);
            
            float movePos = Mathf.Clamp(carTransform.localPosition.x + (targetRotation / 40), -3, 3);
            _moveTween = carTransform.DOLocalMove(new Vector3(movePos, 0, 0), moveDuration).SetEase(Ease.Flash);
        }

        private bool driftEffectStarted = false;
        private void DriftEffect()
        {
            if(driftEffectStarted) return;

            _carMaxSpeed -= 10;
            carManager.cameraManager.ZoomIn();
            foreach (var burstEffect in burstEffects)
            {
                burstEffect.SetActive(true);
            }
            driftEffectStarted = true;
        }

        private void DriftOnLeft(float targetRotation)
        {
            carBodyTransform.DOLocalRotate(new Vector3(1.5f, 0, -6.0f), rotationDuration, RotateMode.Fast);
            foreach (var frontWheel in frontWheels)
            {
                frontWheel.DOLocalRotate(new Vector3(0f, targetRotation, 0.0f), rotationDuration, RotateMode.Fast);
            }
        }

        private void DriftOnRight(float targetRotation)
        {
            carBodyTransform.DOLocalRotate(new Vector3(1.5f, 0, 6.0f), rotationDuration, RotateMode.Fast);
            foreach (var frontWheel in frontWheels)
            {
                frontWheel.DOLocalRotate(new Vector3(0f, targetRotation, 0.0f), rotationDuration, RotateMode.Fast);
            }
        }
        
        public void Reset()
        {
            _carMaxSpeed = _carData.GetCarSpeed();
            _rotateTween.Kill();
            _moveTween.Kill();
            carManager.cameraManager.ZoomOut();
            
            foreach (var burstEffect in burstEffects)
            {
                burstEffect.SetActive(false);
            }
            
            foreach (var frontWheel in frontWheels)
            {
                frontWheel.DOLocalRotate(new Vector3(0f, 0f, 0.0f), rotationDuration, RotateMode.Fast);
            }

            driftEffectStarted = false;
            
            carBodyTransform.DOLocalRotate(Vector3.zero, rotationDuration, RotateMode.Fast);
            carTransform.DORotate(Vector3.zero, rotationDuration, RotateMode.Fast).SetEase(_ease);
            carTransform.DOLocalMove(new Vector3(carTransform.localPosition.x, 0, 0), moveDuration);
        }


        
        
    }
}

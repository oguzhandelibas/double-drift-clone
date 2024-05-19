using System;
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
        
        private float _currentSpeed = 0f; // Anlık hız (m/s)
        private bool isAccelerating = false; 
        
        public void Initialize()
        {
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            
            _carData = carManager.GetCarData();
            _carMaxSpeed = _carData.GetCarSpeed();
            _carAcceleration = _carData.GetCarAcceleration();
            _carHandling = _carData.GetCarHandling();

            carTransform.localPosition = Vector3.zero;
            
            _currentSpeed = 0;
            
            Reset();
            StartEngine();
        }
        

        void Update()
        {
            if(!GameManager.Instance.gameIsActive) return;
            
            if (isAccelerating)
            {
                _currentSpeed += _carAcceleration * Time.deltaTime;
                
                if (_currentSpeed > _carMaxSpeed)
                {
                    _currentSpeed = _carMaxSpeed;
                }
            }
            else
            {
                if (_currentSpeed > 0)
                {
                    _currentSpeed -= _carAcceleration * Time.deltaTime * 0.5f;

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
            if(targetRotation < 5 && targetRotation > -5) return;
            _rotateTween = carTransform
                .DORotate(new Vector3(0, -targetRotation / 2.25f, 0), rotationDuration, RotateMode.Fast);

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


        public void LevelSuccesful()
        {
            Reset();
            StopEngine();
            MoveInfinity();
        }

        public void LevelFailed()
        {
            Reset();
            StopEngine();
        }
        
        public void MoveInfinity()
        {
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            carManager.cameraManager.SetFollowObject(null);
            Vector3 targetPos = new Vector3(0,0,300);
            transform.DOMove(targetPos, 2.0f);
        }

        #region EVENT SUBSCRIPTION

        private void OnEnable()
        {
            LevelSignals.Instance.onLevelSuccessful += LevelSuccesful;
            LevelSignals.Instance.onLevelFailed += LevelFailed;
        }
        
        private void OnDisable()
        {
            LevelSignals.Instance.onLevelSuccessful -= LevelSuccesful;
            LevelSignals.Instance.onLevelFailed -= LevelFailed;
        }

        #endregion
        
    }
}

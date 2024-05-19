using DG.Tweening;
using DoubleDrift.UIModule;
using UnityEngine;

namespace DoubleDrift
{
    public class CarController : MonoBehaviour, IControllable
    {
        #region VARIABLES

        #region SerializeFields

        [SerializeField] private Ease easeType;
        
        [SerializeField] private Transform carTransform;
        [SerializeField] private Transform carBodyTransform;

        [SerializeField] private GameObject[] burstEffects;
        [SerializeField] private Transform[] frontWheels;

        #endregion

        #region Public & Private Fields

        [HideInInspector] public CarManager carManager;
        
        private Tween _rotateTween, _moveTween;
        private CarData _carData;
        
        #endregion

        #region Private Variables
        
        private float _carMaxSpeed, _carAcceleration, _carHandling;
        private float _currentSpeed = 0f;
        private bool _isAccelerating = false; 
        private bool _driftEffectStarted = false;
        
        #endregion

        #region Public Variables

        public float rotationDuration = 0.5f;
        public float moveDuration = 2.5f;

        #endregion

        #endregion
        
        #region Initialization

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
        
        public void StartEngine()
        {
            _isAccelerating = true;
        }

        public void StopEngine()
        {
            _isAccelerating = false;
        }

        #endregion
        
        #region Drift

        void Update()
        {
            if(!GameManager.Instance.gameIsActive) return;
            
            if (_isAccelerating)
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
        
        
        
        public void Rotate(float targetRotation)
        {
            if(targetRotation < 5 && targetRotation > -5) return;
            _rotateTween = carTransform
                .DORotate(new Vector3(0, -targetRotation / 2.0f, 0), rotationDuration, RotateMode.Fast);

            DriftEffect();
            
            if(targetRotation > 0) DriftOnLeft(targetRotation/2);
            else DriftOnRight(targetRotation/2);
            
            float movePos = Mathf.Clamp(carTransform.localPosition.x + (targetRotation / 40), -3, 3);
            _moveTween = carTransform.DOLocalMove(new Vector3(movePos, 0, 0), moveDuration).SetEase(Ease.Flash);
        }

        
        private void DriftEffect()
        {
            if(_driftEffectStarted) return;

            _carMaxSpeed -= 10;
            carManager.cameraManager.Zoom(ZoomType.ZoomIn);
            foreach (var burstEffect in burstEffects) burstEffect.SetActive(true);
            _driftEffectStarted = true;
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
            carManager.cameraManager.Zoom(ZoomType.ZoomOut);
            
            foreach (var burstEffect in burstEffects) 
                burstEffect.SetActive(false);
            
            foreach (var frontWheel in frontWheels) 
                frontWheel.DOLocalRotate(new Vector3(0f, 0f, 0.0f), rotationDuration, RotateMode.Fast);

            _driftEffectStarted = false;
            
            carBodyTransform.DOLocalRotate(Vector3.zero, rotationDuration, RotateMode.Fast);
            carTransform.DORotate(Vector3.zero, rotationDuration, RotateMode.Fast).SetEase(easeType);
            carTransform.DOLocalMove(new Vector3(carTransform.localPosition.x, 0, 0), moveDuration);
        }

        #endregion

        #region Events

        private void LevelSuccesful()
        {
            Reset();
            StopEngine();
            MoveInfinity();
        }

        private void LevelFailed()
        {
            Reset();
            StopEngine();
        }
        
        private void MoveInfinity()
        {
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            carManager.cameraManager.SetFollowObject(null);
            Vector3 targetPos = new Vector3(0,0,300);
            transform.DOMove(targetPos, 5.0f);
        }

        #endregion
        
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

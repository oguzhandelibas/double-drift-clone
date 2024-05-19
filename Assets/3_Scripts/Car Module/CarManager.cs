using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DoubleDrift
{
    public class CarManager : MonoBehaviour
    {
        [SerializeField] private CarData[] carDatas;
        [SerializeField] private CarData currentCarData;

        [Inject] private InfinityPathManager _infinityPathManager;
        [Inject] public CameraManager cameraManager;
        [SerializeField] private SlideControl slideControl;
        private IControllable _controllable;
        private GameObject _currentCarController;
        public int CurrentCarSpeed { get; set; }

        public CarData[] GetCarDatas() => carDatas;
        public CarData GetCarData() => currentCarData;

        public void SetCar(int index)
        {
            UnSubscribe();
            Debug.Log($"Indexxxss: {index}");
            currentCarData = carDatas[index];
            Destroy(_currentCarController);

            _currentCarController = Instantiate(currentCarData.car.prefab, transform.parent).gameObject;
            _controllable = _currentCarController.GetComponent<IControllable>();
            Subscribe();
            
            _infinityPathManager.SetVehicle(_currentCarController.transform);
            Initialize(transform.parent, false);
        }
        public Transform GetCurrentCarTransform() => _currentCarController.transform;
        
        public Transform Initialize(Transform carParent, bool firstInit)
        {
            Debug.Log($"Car Manager Initializing...");
            if (firstInit)
            {
                _currentCarController = Instantiate(currentCarData.car.prefab, carParent).gameObject; 
                
                _controllable = _currentCarController.GetComponent<IControllable>();
                Subscribe();
            }
            
            CarController carController = _currentCarController.GetComponent<CarController>();
            carController.carManager = this;
            carController.Initialize();
            carController.Reset();
            carController.StartEngine();
            
            
            Debug.Log($"Car Manager Initialized!");
            return _currentCarController.transform;
        }
        
        #region EVENT SUBSCRIPTION

        private void Subscribe()
        {
            slideControl.OnSlide += _controllable.Rotate;
            slideControl.OnSlideEnd += _controllable.Reset;
        }

        private void UnSubscribe()
        {
            slideControl.OnSlide -= _controllable.Rotate;
            slideControl.OnSlideEnd -= _controllable.Reset;
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        #endregion
    }
}

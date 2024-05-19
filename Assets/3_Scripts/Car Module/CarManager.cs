using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DoubleDrift
{
    public class CarManager : MonoBehaviour
    {
        [SerializeField] private CarData carData;
        [Inject] public CameraManager cameraManager;
        [SerializeField] private SlideControl slideControl;
        private IControllable _controllable;
        public int CurrentCarSpeed { get; set; }

        public CarData GetCarData() => carData;
        
        public Transform Initialize(Transform carParent)
        {
            Debug.Log($"Car Manager Initializing...");

            Transform controllerTransform = Instantiate(carData.car.prefab, carParent).transform;
            CarController carController = controllerTransform.GetComponent<CarController>();
            carController.carManager = this;
            carController.Initialize();
            
            _controllable = controllerTransform.GetComponent<IControllable>();
            
            Subscribe();
            
            Debug.Log($"Car Manager Initialized!");
            return controllerTransform;
        }
        
        #region EVENT SUBSCRIPTION

        private void Subscribe()
        {
            slideControl.OnSlide += _controllable.Rotate;
            slideControl.OnSlideEnd += _controllable.Reset;
        }

        private void OnDisable()
        {
            slideControl.OnSlide -= _controllable.Rotate;
            slideControl.OnSlideEnd -= _controllable.Reset;
        }

        #endregion
    }
}

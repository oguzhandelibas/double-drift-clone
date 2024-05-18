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

        public Transform Initialize(Transform carParent)
        {
            Debug.Log($"Car Manager Initializing...");

            Transform controllerTransform = Instantiate(carData.car.prefab, carParent).transform;
            controllerTransform.GetComponent<CarController>().carManager = this;
            _controllable = controllerTransform.GetComponent<IControllable>();
            
            slideControl.OnSlide += _controllable.Rotate;
            slideControl.OnSlideEnd += _controllable.Reset;
            
            
            Debug.Log($"Car Manager Initialized!");
            return controllerTransform;
        }
        
        
        #region Action Subscribe

        private void OnDisable()
        {
            slideControl.OnSlide -= _controllable.Rotate;
            slideControl.OnSlideEnd -= _controllable.Reset;
        }

        #endregion
    }
}

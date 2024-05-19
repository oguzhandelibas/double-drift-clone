using UnityEngine;
using Zenject;

namespace DoubleDrift
{
    public class CarManager : MonoBehaviour
    {
        #region SerializeFields

        [SerializeField] private CarData[] carDatas;
        [SerializeField] private CarData currentCarData;
        [SerializeField] private SlideControl slideControl;
        
        #endregion

        #region Injection

        [Inject] private InfinityPathManager _infinityPathManager;
        [Inject] public CameraManager cameraManager;

        #endregion

        #region Properties

        public int CurrentCarSpeed { get; set; }
        public CarData[] GetCarDatas() => carDatas;
        public CarData GetCarData() => currentCarData;
        public Transform GetCurrentCarTransform() => _currentCarController.transform;

        #endregion

        #region Private Fields

        private IControllable _controllable;
        private GameObject _currentCarController;

        #endregion

        #region Initialization

        public void SetCar(int index)
        {
            UnSubscribe();
            currentCarData = carDatas[index];
            Destroy(_currentCarController);
            
            
            Initialize(transform.parent, true);
        }
        
        
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
            
            _currentCarController.transform.position = new Vector3(0, 0, 15);
            _infinityPathManager.SetVehicle(_currentCarController.transform);
            
            Debug.Log($"Car Manager Initialized!");
            return _currentCarController.transform;
        }

        #endregion
        
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

using UnityEngine;
using DG.Tweening;
using Zenject;

namespace DoubleDrift
{
    public class SteeringWheelController : MonoBehaviour, IControllable
    {
        [Inject] private SlideControl _slideControl;
        [SerializeField] private Transform targetWheel;
        public float rotationDuration = 0.5f; // Döndürme süresi
        
        public void Rotate(float targetRotation)
        {
            targetWheel.DORotate(new Vector3(0, 0, -targetRotation), rotationDuration, RotateMode.Fast);
        }

        public void Reset()
        {
            targetWheel.DORotate(Vector3.zero, 2, RotateMode.Fast).SetEase(Ease.OutQuad); 
        }
        
        #region Action Subscribe

        private void OnEnable()
        {
            _slideControl.OnSlide += Rotate;
            _slideControl.OnSlideEnd += Reset;
        }

        private void OnDisable()
        {
            _slideControl.OnSlide -= Rotate;
            _slideControl.OnSlideEnd -= Reset;
        }

        #endregion
    }
}
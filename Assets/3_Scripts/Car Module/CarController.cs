using DG.Tweening;
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
        /// Efekt işlemlerini farklı bir class'a al, sürdürülebilir bi yapıda olsun
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
        
        public void Rotate(float targetRotation)
        {
            _rotateTween = carTransform.DORotate(new Vector3(0, -targetRotation/2.25f, 0), rotationDuration, RotateMode.Fast);

            DriftEffect();
            
            if(targetRotation < 0) DriftOnLeft(targetRotation/2);
            else DriftOnRight(targetRotation/2);
            
            float movePos = Mathf.Clamp(carTransform.localPosition.x + (targetRotation / 40), -3, 3);
            _moveTween = carTransform.DOLocalMove(new Vector3(movePos, 0, 0), moveDuration).SetEase(Ease.Flash);
        }

        private bool driftEffectStarted = false;
        private void DriftEffect()
        {
            if(driftEffectStarted) return;
            carManager.cameraManager.ZoomIn();
            foreach (var burstEffect in burstEffects)
            {
                burstEffect.SetActive(true);
            }
            driftEffectStarted = true;
        }

        private void DriftOnLeft(float targetRotation)
        {
            carBodyTransform.DOLocalRotate(new Vector3(1.5f, 0, 6.0f), rotationDuration, RotateMode.Fast);
            foreach (var frontWheel in frontWheels)
            {
                frontWheel.DOLocalRotate(new Vector3(0f, -targetRotation, 0.0f), rotationDuration, RotateMode.Fast);
            }
        }

        private void DriftOnRight(float targetRotation)
        {
            carBodyTransform.DOLocalRotate(new Vector3(1.5f, 0, -6.0f), rotationDuration, RotateMode.Fast);
            foreach (var frontWheel in frontWheels)
            {
                frontWheel.DOLocalRotate(new Vector3(0f, targetRotation, 0.0f), rotationDuration, RotateMode.Fast);
            }
        }
        
        public void Reset()
        {
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

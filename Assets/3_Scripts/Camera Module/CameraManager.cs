using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace DoubleDrift
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera cameraComponent;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField]
        private float zoomEffectDuration = 1.0f;
        public void ZoomIn()
        {
            cameraComponent.DOFieldOfView(65f, zoomEffectDuration);
        }

        public void ZoomOut()
        {
            cameraComponent.DOFieldOfView(75f, zoomEffectDuration);
        }

        public void SetFollowObject(Transform target)
        {
            if (virtualCamera != null)
            {
                virtualCamera.Follow = target;
                virtualCamera.LookAt = target;
            }
        }
    }
}

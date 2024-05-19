using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace DoubleDrift
{
    public enum ZoomType
    {
        ZoomIn,
        ZoomOut
    }
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera cameraComponent;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField]
        private float zoomEffectDuration = 1.0f;

        public void Zoom(ZoomType zoomType)
        {
            float endValue = 0;
            switch (zoomType)
            {
                case ZoomType.ZoomIn:
                    endValue = 65;
                    break;
                case ZoomType.ZoomOut:
                    endValue = 75;
                    break;
            }
            
            cameraComponent.DOFieldOfView(endValue, zoomEffectDuration);
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

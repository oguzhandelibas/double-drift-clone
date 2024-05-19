using DoubleDrift.UIModule;
using UnityEngine;

namespace DoubleDrift
{
    public class CrashController : MonoBehaviour
    {
        [SerializeField] private CarController carController;
        [SerializeField] private GameObject crashEffect;
        private void OnTriggerEnter(Collider other)
        {
            Obstacle component;
            if (other.TryGetComponent<Obstacle>(out component))
            {
                crashEffect.SetActive(true);
                LevelSignals.Instance.onLevelFailed?.Invoke();
                Debug.Log($"Car crashed to {component.gameObject.name}");
            }
        }
    }
}

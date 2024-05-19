using DG.Tweening;
using UnityEngine;

namespace DoubleDrift
{
    public class PoliceManager : MonoBehaviour
    {
        [SerializeField] private Transform policeCar;

        public void ResetPoliceCar()
        {
            DOTween.KillAll();
            policeCar.transform.position = Vector3.zero;
        }
        
        public void MoveTarget(Transform target)
        {
            Vector3 targetPos = target.position + (Vector3.up * 0.3f);
            policeCar.DOMove(targetPos + (Vector3.back * 4f), 2.0f)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1, () =>
                    {
                        policeCar.DOMove(targetPos + (Vector3.back * 5f), 3)
                            .OnComplete(() =>
                        {
                            DOVirtual.DelayedCall(3, () =>
                            {
                                policeCar.DOMove(targetPos + (Vector3.back * 10), 3);
                            });
                        });;
                    });
                });
        }

        public void CatchPlayer(Transform target)
        {
            DOTween.KillAll();
            Vector3 targetPos = target.position + (Vector3.up * 0.3f);
            policeCar.DOMove(targetPos + (Vector3.back * 3f), 1.5f);
        }
    }
}

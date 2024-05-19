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
            Vector3 initialMove = targetPos + (Vector3.back * 4f);
            Vector3 secondMove = targetPos + (Vector3.back * 5f);
            Vector3 finalMove = targetPos + (Vector3.back * 10f);

            policeCar.DOMove(initialMove, 2.0f)
                .OnComplete(() => 
                    DOVirtual.DelayedCall(1, () =>
                        policeCar.DOMove(secondMove, 3)
                            .OnComplete(() =>
                                DOVirtual.DelayedCall(3, () =>
                                    policeCar.DOMove(finalMove, 3)
                                )
                            )
                    )
                );
        }
        
        public void CatchPlayer(Transform target)
        {
            DOTween.KillAll();
            Vector3 targetPos = target.position + (Vector3.up * 0.3f);
            policeCar.DOMove(targetPos + (Vector3.back * 3f), 1.5f);
        }
    }
}

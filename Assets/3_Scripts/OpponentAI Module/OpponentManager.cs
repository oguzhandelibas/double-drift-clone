using DG.Tweening;
using UnityEngine;

namespace DoubleDrift
{
    public class OpponentManager : MonoBehaviour
    {
        [SerializeField] private Transform opponentCar;

        public void ResetOpponentCar()
        {
            DOTween.KillAll();
            opponentCar.transform.position = new Vector3(3,0.35f,0);
        }
        
        public void MoveTarget()
        {
            Vector3 targetPos = new Vector3(3, 0, 200);
            opponentCar.DOMove(targetPos, 10.0f).SetEase(Ease.Flash);
        }
    }
}

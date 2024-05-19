using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DoubleDrift
{
    public interface IControllable
    {
        void Rotate(float targetRotation);
        void Reset();
    }
}

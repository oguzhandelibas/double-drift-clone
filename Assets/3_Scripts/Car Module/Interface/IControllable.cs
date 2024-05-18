using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDrift
{
    public interface IControllable
    {
        void Rotate(float targetRotation);
        void Reset();
    }
}
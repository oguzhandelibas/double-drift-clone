using System.Collections;
using System.Collections.Generic;
using DoubleDrift;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private InfinityPathManager _infinityPathManager;
    public int levelIndex = 0; 
    void Start()
    {
        _infinityPathManager.Initialize(levelIndex);
    }
}

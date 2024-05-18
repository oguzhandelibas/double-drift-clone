using System.Collections;
using System.Collections.Generic;
using DoubleDrift;
using UnityEngine;
using Zenject;

public class GameManager : AbstractSingleton<GameManager>
{
    [SerializeField] private Transform inGameTransform;
    [Inject] private InfinityPathManager _infinityPathManager;
    [Inject] private CarManager _carManager;
    [Inject] private CameraManager _cameraManager;
    
    public int levelIndex = 0; 
    
    void Awake()
    {
        Transform vehicleTransform = _carManager.Initialize(inGameTransform);
        _cameraManager.SetFollowObject(vehicleTransform.GetChild(0));
        _infinityPathManager.Initialize(levelIndex, vehicleTransform);
        
    }
}

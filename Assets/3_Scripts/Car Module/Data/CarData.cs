using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

namespace DoubleDrift
{
    [System.Serializable]
    public class Car
    {
        [Header("Visual")]
        public Sprite image;
        public CarController prefab;
        
        [Header("Information")]
        public CarType name;
        public float maxSpeed;
        public float acceleration;
        public float handling;
    }
    
    [CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/CarModule/CarData", order = 1)]
    public class CarData : ScriptableObject
    {
        public Car car;

        public float GetCarSpeed() => car.maxSpeed;
        public float GetCarAcceleration() => car.acceleration;
        public float GetCarHandling() => car.handling;
    }
}
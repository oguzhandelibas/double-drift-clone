using AYellowpaper.SerializedCollections;
using UnityEngine;

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
        public float speed;
        public float acceleration;
        public float handling;
    }
    
    [CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/CarModule/CarData", order = 1)]
    public class CarData : ScriptableObject
    {
        public Car car;
    }
}
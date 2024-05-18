using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DoubleDrift
{
    
    [CreateAssetMenu(fileName = "TrafficCarData", menuName = "ScriptableObjects/TrafficModule/TrafficCarData", order = 1)]
    public class TrafficCarData : ScriptableObject
    {
        [SerializedDictionary("Car Type", "Car Object")]
        public SerializedDictionary<TrafficCarType, GameObject> TrafficCars;

        public GameObject GetRandomCar()
        {
            List<TrafficCarType> keys = new List<TrafficCarType>(TrafficCars.Keys);
            TrafficCarType randomKey = keys[Random.Range(0, keys.Count)];
            return TrafficCars[randomKey];
        }
    }
}

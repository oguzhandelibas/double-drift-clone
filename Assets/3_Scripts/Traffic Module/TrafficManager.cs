using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDrift
{
    public class TrafficManager : MonoBehaviour
    {
        public TrafficCarData trafficCarData;
        public TrafficDensity trafficDensity = TrafficDensity.Sparse;
        

        public GameObject GetTrafficCar(TrafficCarType trafficCarType) => trafficCarData.TrafficCars[trafficCarType];
        public GameObject GetRandomTrafficCar() => trafficCarData.GetRandomCar();
    }
}

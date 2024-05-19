using System.Collections.Generic;
using UnityEngine;

namespace DoubleDrift
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private TrafficPool trafficPool;
        [SerializeField] private Transform[] carSpawnPoints;
        [SerializeField] private string trafficCarTag = "TrafficCar";

        private List<GameObject> activeTrafficCars = new List<GameObject>();
        private bool trafficCreated = false;

        public void CreateTraffic(TrafficManager trafficManager)
        {
            if (!trafficCreated)
            {
                trafficPool.Initialize(trafficCarTag, transform);
                trafficCreated = true;
            }
            else
            {
                ClearPreviousTraffic();
            }

            int trafficCarCount = 0;
            switch (trafficManager.trafficDensity)
            {
                case TrafficDensity.Sparse:
                    trafficCarCount = (int)(carSpawnPoints.Length / 6);
                    break;
                case TrafficDensity.Medium:
                    trafficCarCount = (int)(carSpawnPoints.Length / 5);
                    break;
                case TrafficDensity.Frequent:
                    trafficCarCount = (int)(carSpawnPoints.Length / 4);
                    break;
            }
        
            Shuffle(trafficCarCount);    
        }

        private void Shuffle(int trafficCarCount)
        {
            // Fisher-Yates Shuffle
            Transform[] shuffledSpawnPoints = (Transform[])carSpawnPoints.Clone();
            for (int i = shuffledSpawnPoints.Length - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
                (shuffledSpawnPoints[i], shuffledSpawnPoints[randomIndex]) = (shuffledSpawnPoints[randomIndex], shuffledSpawnPoints[i]);
            }

            for (int i = 0; i < trafficCarCount; i++)
            {
                Transform spawnPoint = shuffledSpawnPoints[i];
                GameObject trafficCar = trafficPool.SpawnFromPool(trafficCarTag, spawnPoint.position, spawnPoint.rotation, out Vector3 objectSize);
                trafficCar.transform.SetParent(transform);
                activeTrafficCars.Add(trafficCar);
            }
        }
        

        public void ClearPreviousTraffic()
        {
            foreach (var car in activeTrafficCars)
            {
                trafficPool.ReturnToPool(car, trafficCarTag);
            }
            activeTrafficCars.Clear();
        }
    }
}

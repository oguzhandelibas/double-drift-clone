using System;
using System.Collections.Generic;
using DoubleDrift.UIModule;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DoubleDrift
{
    public class InfinityPathManager : MonoBehaviour
    {
        [Inject] private CarManager _carManager;
        [SerializeField] private PathPool pathPool;
        [SerializeField] private TrafficManager trafficManager;
        public PathType poolTag;
        public Vector3 initialSpawnPosition = Vector3.zero;
        public Transform vehicle; // Aracın Transform'u
        public int triggerDistance = 3; // Yeni yol oluşturma işleminin tetikleneceği mesafe
        
        
        public PathData PathData;
        public int numberOfPaths = 0; // Sahnede aynı anda kaç yol objesi bulunacağını belirler
        public int wayToGoCount; 
            
        private Queue<Path> activePaths = new Queue<Path>();
        private Vector3 nextSpawnPosition;
        private Vector3 pathOffset;
        private bool onPathChange = false;

        public void Initialize(Transform vehicleTransform)
        {
            Debug.Log("Infinity Path Manager Initialize Called!");
            vehicle = vehicleTransform;
            pathPool.Initialize(poolTag.ToString(), transform);

            foreach (var cur in PathData.Path)
            {
                numberOfPaths += cur.Value.size;
            }
            wayToGoCount = PathData.repeatCount * numberOfPaths;
            
            nextSpawnPosition = initialSpawnPosition;

            for (int i = 0; i < numberOfPaths; i++)
            {
                SpawnInitialPath(i > 0);
            }

            if (activePaths.Count == 0)
            {
                Debug.LogError("No paths were spawned. Please check the pool settings and ensure there are enough objects.");
            }
            Debug.Log("Infinity Path Manager Initialized!");
        }

        private void SpawnInitialPath(bool createTraffic)
        {
            Path path = pathPool.SpawnFromPool(poolTag.ToString(), nextSpawnPosition, Quaternion.identity, out Vector3 objectSize).GetComponent<Path>();
            if (path != null)
            {
                path.transform.SetParent(transform);
                
                if(createTraffic) path.CreateTraffic(trafficManager);
                
                activePaths.Enqueue(path);
                pathOffset = new Vector3(0, 0, objectSize.z);
                nextSpawnPosition += pathOffset;
            }
            else
            {
                Debug.LogError("Failed to spawn path from pool.");
            }
        }

        private void FixedUpdate()
        {
            if (activePaths == null || activePaths.Count == 0)
            {
                Debug.LogWarning("Active paths queue is null or empty!");
                return;
            }

            if (!GameManager.Instance.gameIsStarted) return;
          
            MovePaths();
            CheckAndRecyclePaths();
        }

        private void MovePaths()
        {
            if(onPathChange) return;

            float carSpeed = _carManager.CurrentCarSpeed / 3;
            foreach (Path path in activePaths)
            {
                path.transform.Translate(Vector3.back * carSpeed * Time.deltaTime);
            }
        }

        private void CheckAndRecyclePaths()
        {
            if (vehicle.position.z > GetActivePathWithOffsetAndTriggerDistance())
            {
                onPathChange = true;
                Path oldPath = activePaths.Dequeue();
                oldPath.ClearPreviousTraffic();
                oldPath.gameObject.SetActive(false);

                oldPath.transform.position = nextSpawnPosition - (pathOffset * triggerDistance);
                oldPath.CreateTraffic(trafficManager);
                
                oldPath.gameObject.SetActive(true);

                activePaths.Enqueue(oldPath);
                wayToGoCount--;
                if (wayToGoCount <= 0)
                {
                    UIManager.Instance.Show<LevelCompletedUI>();
                    GameManager.Instance.StopGame();
                    LevelSignals.Instance.onLevelSuccessful?.Invoke();
                }

                onPathChange = false;
            }
        }

        private float GetActivePathWithOffsetAndTriggerDistance()
        {
            return activePaths.Peek().transform.position.z + pathOffset.z * triggerDistance;
        }
    }
}

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
        public ObjectPooler objectPooler;
        public PathType poolTag;
        public Vector3 initialSpawnPosition = Vector3.zero;
        public Transform vehicle; // Aracın Transform'u
        public int triggerDistance = 3; // Yeni yol oluşturma işleminin tetikleneceği mesafe

        private PathData currentPathData;
        public int numberOfPaths = 0; // Sahnede aynı anda kaç yol objesi bulunacağını belirler
        public int wayToGoCount; 
            
        private Queue<GameObject> activePaths = new Queue<GameObject>();
        private Vector3 nextSpawnPosition;
        private Vector3 pathOffset;
        private bool onPathChange = false;

        public void Initialize(int levelIndex, Transform vehicleTransform)
        {
            Debug.Log("Infinity Path Manager Initialize Called!");
            vehicle = vehicleTransform;
            currentPathData = objectPooler.Initialize(levelIndex);

            foreach (var cur in currentPathData.Path)
            {
                numberOfPaths += cur.Value.size;
            }
            wayToGoCount = currentPathData.repeatCount * numberOfPaths;
            
            nextSpawnPosition = initialSpawnPosition;

            for (int i = 0; i < numberOfPaths; i++)
            {
                SpawnInitialPath();
            }

            if (activePaths.Count == 0)
            {
                Debug.LogError("No paths were spawned. Please check the pool settings and ensure there are enough objects.");
            }
            Debug.Log("Infinity Path Manager Initialized!");
        }

        private void SpawnInitialPath()
        {
            GameObject path = objectPooler.SpawnFromPool(poolTag, nextSpawnPosition, Quaternion.identity, out Vector3 objectSize);
            if (path != null)
            {
                path.transform.SetParent(transform);
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
            
            foreach (GameObject path in activePaths)
            {
                path.transform.Translate(Vector3.back * _carManager.CurrentCarSpeed * Time.deltaTime);
            }
        }

        private void CheckAndRecyclePaths()
        {
            if (vehicle.position.z > GetActivePathWithOffsetAndTriggerDistance())
            {
                onPathChange = true;
                GameObject oldPath = activePaths.Dequeue();
                oldPath.SetActive(false);

                oldPath.transform.position = nextSpawnPosition - (pathOffset * triggerDistance);
                oldPath.SetActive(true);

                activePaths.Enqueue(oldPath);
                wayToGoCount--;
                if (wayToGoCount <= 0)
                {
                    UIManager.Instance.Show<HomeUI>();
                    GameManager.Instance.StopGame();
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

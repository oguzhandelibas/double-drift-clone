using System.Collections.Generic;
using UnityEngine;

namespace DoubleDrift
{
    public class InfinityPathManager : MonoBehaviour
    {
        public ObjectPooler objectPooler;
        public PathType poolTag;
        public int numberOfPaths = 5; // Sahnede aynı anda kaç yol objesi bulunacağını belirler
        public Vector3 initialSpawnPosition = Vector3.zero;
        public Transform vehicle; // Aracın Transform'u
        public int triggerDistance = 3; // Yeni yol oluşturma işleminin tetikleneceği mesafe
        public float pathSpeed = 5f; // Yol hareket hızı

        private Queue<GameObject> activePaths = new Queue<GameObject>();
        private Vector3 nextSpawnPosition;
        private Vector3 pathOffset;

        public void Initialize(int levelIndex, Transform vehicleTransform)
        {
            Debug.Log("Infinity Path Manager Initialize Called!");
            vehicle = vehicleTransform;
            objectPooler.Initialize(levelIndex);

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

        private void Update()
        {
            if (activePaths == null || activePaths.Count == 0)
            {
                Debug.LogWarning("Active paths queue is null or empty!");
                return;
            }

            MovePaths();
            CheckAndRecyclePaths();
        }

        private void MovePaths()
        {
            foreach (GameObject path in activePaths)
            {
                path.transform.Translate(Vector3.back * pathSpeed * Time.deltaTime);
            }
        }

        private void CheckAndRecyclePaths()
        {
            if (vehicle.position.z > GetActivePathWithOffsetAndTriggerDistance())
            {
                GameObject oldPath = activePaths.Dequeue();
                oldPath.SetActive(false);

                oldPath.transform.position = nextSpawnPosition - (pathOffset * triggerDistance);
                oldPath.SetActive(true);

                activePaths.Enqueue(oldPath);
            }
        }

        private float GetActivePathWithOffsetAndTriggerDistance()
        {
            return activePaths.Peek().transform.position.z + pathOffset.z * triggerDistance;
        }
    }
}

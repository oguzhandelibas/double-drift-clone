using UnityEngine;
using System.Collections.Generic;

namespace DoubleDrift
{
    public class InfinityPathManager : MonoBehaviour
    {
        public ObjectPooler objectPooler;
        public PathTypes poolTag;
        public int numberOfPaths = 5; // Sahnede aynı anda kaç yol objesi bulunacağını belirler
        public Vector3 initialSpawnPosition = Vector3.zero;
        public Transform vehicle; // Aracın Transform'u
        public int triggerDistance = 3; // Yeni yol oluşturma işleminin tetikleneceği mesafe

        private Queue<GameObject> activePaths;
        private Vector3 nextSpawnPosition;
        private Vector3 pathOffset;

        public void Initialize(int levelIndex)
        {
            Debug.Log($"Infinity Path Manager Initialize Called!");
            objectPooler.Initialize(levelIndex);
            
            activePaths = new Queue<GameObject>();
            nextSpawnPosition = initialSpawnPosition;

            // İlk yolları oluştur
            for (int i = 0; i < numberOfPaths; i++)
            {
                GameObject path = objectPooler.SpawnFromPool(poolTag, nextSpawnPosition, Quaternion.identity, out Vector3 objectSize);
                if (path != null)
                {
                    activePaths.Enqueue(path);
                    pathOffset = new Vector3(0, 0, objectSize.z); // Objelerin z eksenindeki boyutuna göre offset ayarla
                    nextSpawnPosition += pathOffset;
                }
                else
                {
                    Debug.LogError("Failed to spawn path from pool.");
                }
            }

            if (activePaths.Count == 0)
            {
                Debug.LogError("No paths were spawned. Please check the pool settings and ensure there are enough objects.");
            }
            Debug.Log($"Infinity Path Manager Initialized!");
        }

        private void Update()
        {
            if (activePaths == null || activePaths.Count == 0)
            {
                Debug.LogWarning("Active paths queue is null or empty!");
                return;
            }

            // Aracın en öndeki yol objesi yeterince geride kalmışsa onu tekrar kullan
            if (vehicle.position.z > activePaths.Peek().transform.position.z + pathOffset.z * triggerDistance)
            {
                GameObject oldPath = activePaths.Dequeue();
                oldPath.SetActive(false);

                oldPath.transform.position = nextSpawnPosition;
                oldPath.SetActive(true);

                activePaths.Enqueue(oldPath);
                nextSpawnPosition += pathOffset;
            }
        }
    }
}

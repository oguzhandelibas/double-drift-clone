using System.Collections.Generic;
using DoubleDrift.UIModule;
using UnityEngine;
using Zenject;

namespace DoubleDrift
{
    public class InfinityPathManager : MonoBehaviour
    {
        #region Injection

        [Inject] private CarManager _carManager;

        #endregion

        #region SerializeFields

        [SerializeField] private PathPool pathPool;
        [SerializeField] private TrafficManager trafficManager;

        #endregion

        #region Public Fields

        public PathType poolTag;
        

        #endregion

        #region Private Fields

        private Vector3 initialSpawnPosition = Vector3.zero;
        private Transform vehicle;

        #endregion

        #region Private Variables

        private int triggerDistance = 3;
        
        private int numberOfPaths = 0;
        private int wayToGoCount; 
            
        private Queue<Path> activePaths = new Queue<Path>();
        private Vector3 nextSpawnPosition;
        private Vector3 pathOffset;
        private bool onPathChange = false;

        #endregion

        #region Initialization

        public void Initialize(LevelPathData levelPathData,Transform vehicleTransform, bool firstInit)
        {
            Debug.Log("Infinity Path Manager Initialize Called!");
            
            onPathChange = false;
            numberOfPaths = 0;
            wayToGoCount = 0;
            if(!firstInit) ResetBehaviour();
            
            vehicle = vehicleTransform;
            pathPool.Initialize(poolTag.ToString(), transform);
            pathPool.SetPool(levelPathData.Path[poolTag].prefab, levelPathData.Path[poolTag].size);
            
            
            foreach (var cur in levelPathData.Path) numberOfPaths += cur.Value.size;
            wayToGoCount = levelPathData.repeatCount * numberOfPaths;
            nextSpawnPosition = initialSpawnPosition;

            for (int i = 0; i < numberOfPaths; i++) SpawnInitialPath(i > 0);

            #region Debug

            if (activePaths.Count == 0)
            {
                Debug.LogError("No paths were spawned. Please check the pool settings and ensure there are enough objects.");
            }
            Debug.Log("Infinity Path Manager Initialized!");

            #endregion
        }
        private void SpawnInitialPath(bool createTraffic)
        {
            Debug.Log($"Spawn Position: {nextSpawnPosition}");
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

        #endregion
        
        #region Loop

        private void FixedUpdate()
        {
            if (!GameManager.Instance.gameIsActive) return;
            if (activePaths == null || activePaths.Count == 0)
            {
                Debug.LogWarning("Active paths queue is null or empty!");
                return;
            }
            
            MovePaths();
            CheckAndRecyclePaths();
        }
        
        // Yolların hareketini kontrol eder
        private void MovePaths()
        {
            if (onPathChange) return;
            
            // ReSharper disable once PossibleLossOfFraction
            float carSpeed = (_carManager.CurrentCarSpeed / 3);
            Vector3 moveDistance = Vector3.back * (carSpeed * Time.deltaTime);
    
            foreach (Path path in activePaths)
            {
                path.transform.position += moveDistance;
            }
        }
        
        // Yeni yol oluşumunu takip eder
        private void CheckAndRecyclePaths()
        {
            if (vehicle.position.z <= GetActivePathWithOffsetAndTriggerDistance()) return;
            onPathChange = true;

            Path oldPath = activePaths.Dequeue();
            oldPath.ClearPreviousTraffic();
            
            Vector3 newPosition = nextSpawnPosition - (pathOffset * triggerDistance);

            oldPath.transform.position = newPosition;
            oldPath.CreateTraffic(trafficManager);
            
            activePaths.Enqueue(oldPath);
            
            CheckLevelEnd();
            onPathChange = false;
        }
        
        // Levelin bitimini kontrol eder
        private void CheckLevelEnd()
        {
            wayToGoCount--;
            if (wayToGoCount > 0) return;
            
            UIManager.Instance.Show<LevelCompletedUI>();
            GameManager.Instance.StopGame();
            LevelSignals.Instance.onLevelSuccessful?.Invoke();
        }
        
        private float GetActivePathWithOffsetAndTriggerDistance()
        {
            return activePaths.Peek().transform.position.z + pathOffset.z * triggerDistance;
        }

        #endregion

        #region Tools

        // Yeni level için hazırlık sağlar
        private void ResetBehaviour()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            activePaths.Clear();
            pathPool.ResetPool();
        }

        #endregion
        
    }
}

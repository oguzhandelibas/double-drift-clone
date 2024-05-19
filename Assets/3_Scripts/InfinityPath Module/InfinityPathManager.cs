using System.Collections.Generic;
using DoubleDrift.UIModule;
using UnityEngine;
using Zenject;

namespace DoubleDrift
{
    public class InfinityPathManager : MonoBehaviour
    {
        #region VARIABLES

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

        private Vector3 _initialSpawnPosition = Vector3.zero;
        private Transform _vehicle;

        #endregion

        #region Private Variables

        private int _triggerDistance = 3;
        
        private int _numberOfPaths = 0;
        private int _wayToGoCount; 
            
        private Queue<Path> _activePaths = new Queue<Path>();
        private Vector3 _nextSpawnPosition;
        private Vector3 _pathOffset;
        private bool _onPathChange = false;

        #endregion

        #endregion
        
        #region Initialization

        public void SetVehicle(Transform vehicleTransform)
        {
            _vehicle = vehicleTransform;
        }
        
        public void Initialize(LevelPathData levelPathData, bool firstInit)
        {
            Debug.Log("Infinity Path Manager Initialize Called!");
            
            _onPathChange = false;
            _numberOfPaths = 0;
            _wayToGoCount = 0;
            if(!firstInit) ResetBehaviour();
            
            pathPool.Initialize(poolTag.ToString(), transform);
            pathPool.SetPool(levelPathData.Path[poolTag].prefab, levelPathData.Path[poolTag].size);
            
            
            foreach (var cur in levelPathData.Path) _numberOfPaths += cur.Value.size;
            _wayToGoCount = levelPathData.repeatCount * _numberOfPaths;
            _nextSpawnPosition = _initialSpawnPosition;

            for (int i = 0; i < _numberOfPaths; i++) SpawnInitialPath(i > 0);

            #region Debug

            if (_activePaths.Count == 0)
            {
                Debug.LogError("No paths were spawned. Please check the pool settings and ensure there are enough objects.");
            }
            Debug.Log("Infinity Path Manager Initialized!");

            #endregion
        }
        private void SpawnInitialPath(bool createTraffic)
        {
            Debug.Log($"Spawn Position: {_nextSpawnPosition}");
            Path path = pathPool.SpawnFromPool(poolTag.ToString(), _nextSpawnPosition, Quaternion.identity, out Vector3 objectSize).GetComponent<Path>();
            if (path != null)
            {
                path.transform.SetParent(transform);
                
                if(createTraffic) path.CreateTraffic(trafficManager);
                
                _activePaths.Enqueue(path);
                _pathOffset = new Vector3(0, 0, objectSize.z);
                _nextSpawnPosition += _pathOffset;
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
            if (_activePaths == null || _activePaths.Count == 0)
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
            if (_onPathChange) return;
            
            // ReSharper disable once PossibleLossOfFraction
            float carSpeed = (_carManager.CurrentCarSpeed / 3);
            Vector3 moveDistance = Vector3.back * (carSpeed * Time.deltaTime);
    
            foreach (Path path in _activePaths)
            {
                path.transform.position += moveDistance;
            }
        }
        
        // Yeni yol oluşumunu takip eder
        private void CheckAndRecyclePaths()
        {
            if (_vehicle.position.z <= GetActivePathWithOffsetAndTriggerDistance()) return;
            _onPathChange = true;

            Path oldPath = _activePaths.Dequeue();
            oldPath.ClearPreviousTraffic();
            
            Vector3 newPosition = _nextSpawnPosition - (_pathOffset * _triggerDistance);

            oldPath.transform.position = newPosition;
            oldPath.CreateTraffic(trafficManager);
            
            _activePaths.Enqueue(oldPath);
            
            CheckLevelEnd();
            _onPathChange = false;
        }
        
        // Levelin bitimini kontrol eder
        private void CheckLevelEnd()
        {
            _wayToGoCount--;
            if (_wayToGoCount > 0) return;
            
            UIManager.Instance.Show<LevelCompletedUI>();
            GameManager.Instance.StopGame();
            LevelSignals.Instance.onLevelSuccessful?.Invoke();
        }
        
        private float GetActivePathWithOffsetAndTriggerDistance()
        {
            return _activePaths.Peek().transform.position.z + _pathOffset.z * _triggerDistance;
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
            _activePaths.Clear();
            pathPool.ResetPool();
        }

        #endregion
        
    }
}

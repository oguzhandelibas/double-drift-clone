using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DoubleDrift
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
    }
    
    [CreateAssetMenu(fileName = "PathData", menuName = "ScriptableObjects/InfiniyPath/PathData", order = 1)]
    public class PathData : ScriptableObject
    {
        public int repeatCount;
        
        [SerializedDictionary("Path Type", "Path Object")]
        public SerializedDictionary<PathType, Pool> Path;
    }
}

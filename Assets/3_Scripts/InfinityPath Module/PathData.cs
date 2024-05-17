using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DoubleDrift
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
        public int repeatCount;
    }
    
    [CreateAssetMenu(fileName = "PathData", menuName = "ScriptableObjects/InfiniyPath/PathData", order = 1)]
    public class PathData : ScriptableObject
    {
        [SerializedDictionary("Path Type", "Path Object")]
        public SerializedDictionary<PathTypes, Pool> Path;
    }
}

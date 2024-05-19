using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DoubleDrift
{
    [CreateAssetMenu(fileName = "LevelPathData", menuName = "ScriptableObjects/InfiniyPath/LevelPathData", order = 1)]
    public class LevelPathData : ScriptableObject
    {
        public int repeatCount;
        
        [SerializedDictionary("Path Type", "Path Object")]
        public SerializedDictionary<PathType, Pool> Path;
    }
}

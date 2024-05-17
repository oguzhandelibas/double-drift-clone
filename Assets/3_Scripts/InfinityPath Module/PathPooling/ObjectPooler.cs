using System.Collections.Generic;
using UnityEngine;

namespace DoubleDrift
{

    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private PathData[] pathDatas;
        public Dictionary<PathTypes, Queue<GameObject>> poolDictionary;

        public void Initialize(int levelIndex)
        {
            Debug.Log($"Object Pooler Initialize Called!");
            poolDictionary = new Dictionary<PathTypes, Queue<GameObject>>();

            foreach (KeyValuePair<PathTypes, Pool> kvp in pathDatas[levelIndex].Path)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < kvp.Value.size; i++)
                {
                    GameObject obj = Instantiate(kvp.Value.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(kvp.Key, objectPool);
            }
            Debug.Log($"Object Pooler Initialized!");
        }

        public GameObject SpawnFromPool(PathTypes tag, Vector3 position, Quaternion rotation, out Vector3 objectSize)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                objectSize = Vector3.zero;
                return null;
            }

            if (poolDictionary[tag].Count == 0)
            {
                Debug.LogWarning("No objects available in pool with tag " + tag);
                objectSize = Vector3.zero;
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            poolDictionary[tag].Enqueue(objectToSpawn);

            objectSize = objectToSpawn.GetComponentInChildren<Renderer>().bounds.size;
            return objectToSpawn;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace DoubleDrift
{
    
    public abstract class Pooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }
        
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public Pool pool;

        public virtual void Initialize(string tag, Transform parent)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, parent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            pool.tag = tag;
            poolDictionary.Add(tag, objectPool);
            
            Debug.Log($"Object Pooler Initialized!");
        }
        
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, out Vector3 objectSize)
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

            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            poolDictionary[tag].Enqueue(objectToSpawn);

            objectSize = objectToSpawn.GetComponentInChildren<Renderer>().bounds.size;
            return objectToSpawn;
        }
        
        public void ReturnToPool(GameObject objectToReturn, string tag)
        {
            objectToReturn.SetActive(false);
            poolDictionary[tag].Enqueue(objectToReturn);
        }
    }
}
using UnityEngine;

namespace DoubleDrift
{
    public class PathPool : Pooler
    {
        public override void Initialize(string tag, Transform parent)
        {
            base.Initialize(tag, parent);
        }

        public void SetPool(GameObject prefab, int size)
        {
            pool.prefab = prefab;
            pool.size = size;
        }
    }
}

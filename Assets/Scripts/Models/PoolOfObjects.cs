using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class PoolOfObjects<T> : ComponentSingleton<PoolOfObjects<T>>
    {
        private readonly Queue<T> _pool;
        private readonly Transform _parentTransform;
        private readonly GameObject _flierPrefab;

        public PoolOfObjects(GameObject flierPrefab, Transform parentTransform, int startNumberOfFliers = 0)
        {
            _pool = new Queue<T>();
            _flierPrefab = flierPrefab;
            _parentTransform = parentTransform;

            for (var i = 0; i < startNumberOfFliers; i++)
                Create();
        }

        public void Put(T poolObj)
        {
            _pool.Enqueue(poolObj);
        }

        public T Get()
        {
            if (_pool.Count <= 0) Create();
            var poolObj = _pool.Dequeue();
            return poolObj;
        }

        private void Create()
        {
            var poolObj = Instantiate(_flierPrefab, _parentTransform);
            var component = poolObj.GetComponent<T>();
            Put(component);
        }
    }
}

using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Pools
{
    public class PoolOfObjects<T> : ComponentSingleton<PoolOfObjects<T>>
    {
        private readonly Queue<T> _poolOfFliers;
        private readonly Transform _parentTransform;
        private readonly GameObject _flierPrefab;

        public PoolOfObjects(GameObject flierPrefab, Transform parentTransform, int startNumberOfFliers = 0)
        {
            _poolOfFliers = new Queue<T>();
            _flierPrefab = flierPrefab;
            _parentTransform = parentTransform;

            for (var i = 0; i < startNumberOfFliers; i++)
                Create();
        }

        public void Put(T poolObj)
        {
            _poolOfFliers.Enqueue(poolObj);
        }

        public T Get()
        {
            if (_poolOfFliers.Count <= 0) Create();
            var poolObj = _poolOfFliers.Dequeue();
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

using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Models
{
    public class PoolOfFliers : ComponentSingleton<PoolOfFliers>
    {
        private readonly Queue<Flier> _poolOfFliers;
        private readonly Transform _parentTransform;
        private readonly GameObject _flierPrefab;

        public PoolOfFliers(GameObject flierPrefab, Transform parentTransform, int startNumberOfFliers = 0)
        {
            _poolOfFliers = new Queue<Flier>();
            _flierPrefab = flierPrefab;
            _parentTransform = parentTransform;

            for (var i = 0; i < startNumberOfFliers; i++)
                Create();
        }

        public void Put(Flier poolObj)
        {
            _poolOfFliers.Enqueue(poolObj);
            poolObj.gameObject.SetActive(false);
        }

        public Flier Get()
        {
            if (_poolOfFliers.Count <= 0) Create();
            var poolObj = _poolOfFliers.Dequeue();
            poolObj.gameObject.SetActive(true);
            return poolObj;
        }

        private void Create()
        {
            var poolObj = Instantiate(_flierPrefab, _parentTransform);
            var component = poolObj.GetComponent<Flier>();
            Put(component);
        }
    }
}

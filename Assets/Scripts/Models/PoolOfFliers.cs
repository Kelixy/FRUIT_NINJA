using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Models
{
    public class PoolOfFliers : ComponentSingleton<PoolOfFliers>
    {
        private readonly Queue<Flier> _fliersPool;
        private readonly Transform _parentTransform;
        private readonly GameObject _flierPrefab;

        public PoolOfFliers(GameObject flierPrefab, Transform parentTransform, int startNumberOfFliers = 0)
        {
            _fliersPool = new Queue<Flier>();
            _flierPrefab = flierPrefab;
            _parentTransform = parentTransform;

            for (var i = 0; i < startNumberOfFliers; i++)
                Create();
        }

        public void Put(Flier flier)
        {
            _fliersPool.Enqueue(flier);
            flier.Switch(false);
        }

        public Flier Get()
        {
            if (_fliersPool.Count <= 0) Create();
            var flier = _fliersPool.Dequeue();
            flier.Switch(true);
            return flier;
        }

        private void Create()
        {
            var flier = Instantiate(_flierPrefab, _parentTransform);
            var component = flier.GetComponent<Flier>();
            Put(component);
        }
    }
}

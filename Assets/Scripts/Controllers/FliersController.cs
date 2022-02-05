using Models;
using UnityEngine;
using Views;

namespace Controllers
{
    public class FliersController : MonoBehaviour
    {
        [Range(0, 10)] [SerializeField] private float fruitSpeed = 5;
        [Range(0, 1)] [SerializeField] private float bottomSpawnProbability = 0.8f;
        [Range(1, 10)] [SerializeField] private int numberOfFruits = 4;
        [Range(0, 10)] [SerializeField] private float roundDelay = 3;

        [SerializeField] private RectTransform screenRectTransform;

        [SerializeField] private Transform poolTransform;
        [SerializeField] private GameObject flierPrefab;

        private PoolOfFliers _poolOfFliers;
        private Vector2 _screenSize;
        private float _yCeiling;

        private Flier[] _fliers;

        private void Awake()
        {
            _screenSize = screenRectTransform.rect.size;
            _yCeiling = _screenSize.y / 2;
            
        }

        private void Start()
        {
            _poolOfFliers = new PoolOfFliers(flierPrefab, poolTransform, numberOfFruits);
            _fliers = new Flier[numberOfFruits];
            
            for (var i = 0; i < numberOfFruits; i++)
            {
                _fliers[i] = _poolOfFliers.Get();
            }
        }

        private (Vector3 startPoint, float angle) GetRandomStartValues()
        {
            Vector3 startPoint;
            float angle;
            float spawnValidator = Random.Range(0f, 1f);
        
            if (spawnValidator <= (1 - bottomSpawnProbability) / 2)
            {
                startPoint = new Vector3(- _screenSize.x / 2, Random.Range(-_yCeiling, _yCeiling / 2));
                angle = Random.Range(35f, 85f);
            }
            else if (spawnValidator <= bottomSpawnProbability)
            {
                startPoint = new Vector3(Random.Range(-_screenSize.x / 2, 0), -_yCeiling);
                angle = Random.Range(35f, 85f);
            }
            else
            {
                startPoint = new Vector3(_screenSize.x / 2, Random.Range(-_yCeiling, _yCeiling / 2));
                angle = Random.Range(95f, 135f);
            }
            
            return (startPoint, angle);
        }
        
        private bool CheckIfPointOnScene(Vector3 point, float indent = 0)
        {
            return point.x > - _screenSize.x / 2 - indent
                   && point.x < _screenSize.x / 2 + indent
                   && point.y > - _screenSize.y / 2 - indent
                   && point.y < _screenSize.y / 2 + indent;
        }

        private void MoveFliers(float deltaTime)
        {
            if (_fliers.Length <= 0) 
                return;
            
            foreach (var flier in _fliers)
            {
                if (flier.IsActive)
                {
                    var nextPoint = flier.MoveAlongTrajectory(deltaTime, fruitSpeed);

                    if (!CheckIfPointOnScene(nextPoint, 50))
                    {
                        _poolOfFliers.Put(flier);
                    }
                }
            }
        }

        private void Update()
        {
            MoveFliers(Time.deltaTime);
        }
    }
}
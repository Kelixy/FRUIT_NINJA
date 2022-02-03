using DG.Tweening;
using Models;
using UnityEngine;
using Views;

namespace Controllers
{
    public class FruitsController : MonoBehaviour
    {
        [Range(0, 200)] [SerializeField] private float fruitStartSpeed = 100;
        [Range(0, 2)] [SerializeField] private float gravity = 1;
        [Range(0, 10)] [SerializeField] private float jumpDuration = 1;
        [Range(0, 1)] [SerializeField] private float bottomSpawnProbability = 0.8f;
        [Range(1, 10)] [SerializeField] private int numberOfFruits = 4;
        [Range(0, 10)] [SerializeField] private float roundDelay = 3;

        [SerializeField] private RectTransform screenRectTransform;

        [SerializeField] private Transform poolTransform;
        [SerializeField] private GameObject fruitPrefab;

        private Vector2 _screenSize;
        private float _yCeiling;

        private TrajectoryCounter _trajectoryCounter;
        private int _numberOfLaunchedFruits;
        private Fruit[] _fruits;

        private void Awake()
        {
            _trajectoryCounter = new TrajectoryCounter();
            _fruits = new Fruit[numberOfFruits];
        }

        private void Start()
        {
            _screenSize = screenRectTransform.rect.size;
            _yCeiling = _screenSize.y / 2;

            LoadFruits();
            LaunchFruits();
        }
        
        private void LoadFruits()
        {
            for (var i = 0; i < numberOfFruits; i++)
            {
                var fruit = Instantiate(fruitPrefab, poolTransform);
                _fruits[i] = fruit.GetComponent<Fruit>();
            }
        }

        private void LaunchFruits()
        {
            var delay = roundDelay;
            foreach (var fruit in _fruits)
            {
                delay += Random.Range(0f, 1f);
                LaunchFruit(fruit, delay);
                _numberOfLaunchedFruits++;
            }
        }

        private void LaunchFruit(Fruit fruit, float delay)
        {
            var fruitRt = fruit.RectTransform;
            var (startPoint, angle) = GetRandomStartValues(fruit.Radius);
            var positions = _trajectoryCounter.GetPoints(startPoint, fruitStartSpeed, 1, angle, gravity, _screenSize, fruit.Radius);
            fruit.transform.localPosition = positions[0];
            DOTween.Sequence()
                .AppendInterval(delay)
                .AppendCallback(() => {fruit.Switch(true);})
                .Append(fruitRt.DOLocalPath(positions, jumpDuration).SetEase(Ease.InOutQuad))
                .AppendCallback(() =>
                {
                    fruit.Switch(false);
                    if (--_numberOfLaunchedFruits == 0)
                        LaunchFruits();
                });
        }

        private (Vector3 startPoint, float angle) GetRandomStartValues(float fruitRadius)
        {
            Vector3 startPoint;
            float angle;
            float spawnValidator = Random.Range(0f, 1f);
        
            if (spawnValidator <= (1 - bottomSpawnProbability) / 2)
            {
                startPoint = new Vector3(- fruitRadius - _screenSize.x / 2, Random.Range(-_yCeiling, _yCeiling / 2));
                angle = Random.Range(35f, 85f);
            }
            else if (spawnValidator <= bottomSpawnProbability)
            {
                startPoint = new Vector3(Random.Range(-_screenSize.x / 2, 0), - fruitRadius -_yCeiling);
                angle = Random.Range(35f, 85f);
            }
            else
            {
                startPoint = new Vector3(fruitRadius + _screenSize.x / 2, Random.Range(-_yCeiling, _yCeiling / 2));
                angle = Random.Range(95f, 135f);
            }
            
            return (startPoint, angle);
        }
    }
}

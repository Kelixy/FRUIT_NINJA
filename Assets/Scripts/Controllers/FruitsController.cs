using DG.Tweening;
using Models;
using UnityEngine;

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
        private int _numberOfFruitsOnScene;

        private void Awake()
        {
            _trajectoryCounter = new TrajectoryCounter();
        }

        private void Start()
        {
            _screenSize = screenRectTransform.rect.size;
            _yCeiling = _screenSize.y / 2;

            LaunchFruits();
        }

        private void LaunchFruits()
        {
            var delay = roundDelay;
            for (var i = 0; i < numberOfFruits; i++)
            {
                delay += Random.Range(0f, 1f);
                LaunchFruit(delay);
                _numberOfFruitsOnScene++;
            }
        }

        private void LaunchFruit(float delay)
        {
            var fruit = Instantiate(fruitPrefab, poolTransform);
            var fruitRt = fruit.GetComponent<RectTransform>();
            var fruitHeight = fruitRt.rect.height;
            var fruitRadius = fruitHeight / 2;
            var (startPoint, angle) = GetRandomStartValues(fruitHeight);
            var positions = _trajectoryCounter.GetPoints(startPoint, fruitStartSpeed, 1, angle, gravity, _screenSize, fruitRadius);
            fruit.transform.localPosition = positions[0];
            DOTween.Sequence()
                .AppendInterval(delay)
                .Append(fruitRt.DOLocalPath(positions, jumpDuration).SetEase(Ease.InOutQuad))
                .AppendCallback(() =>
                {
                    Destroy(fruit);
                    if (--_numberOfFruitsOnScene == 0)
                        LaunchFruits();
                });
        }

        private (Vector3 startPoint, float angle) GetRandomStartValues(float fruitHeight)
        {
            Vector3 startPoint;
            float angle;
            float spawnValidator = Random.Range(0f, 1f);
        
            if (spawnValidator <= (1 - bottomSpawnProbability) / 2)
            {
                startPoint = new Vector3(- fruitHeight - _screenSize.x / 2, Random.Range(-_yCeiling, _yCeiling / 2));
                angle = Random.Range(35f, 85f);
            }
            else if (spawnValidator <= bottomSpawnProbability)
            {
                startPoint = new Vector3(Random.Range(-_screenSize.x / 2, 0), - fruitHeight -_yCeiling);
                angle = Random.Range(35f, 85f);
            }
            else
            {
                startPoint = new Vector3(fruitHeight + _screenSize.x / 2, Random.Range(-_yCeiling, _yCeiling / 2));
                angle = Random.Range(95f, 135f);
            }
            
            return (startPoint, angle);
        }
    }
}

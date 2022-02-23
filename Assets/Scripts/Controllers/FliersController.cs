using System.Collections.Generic;
using DG.Tweening;
using Mechanics;
using Pools;
using UnityEngine;
using Views;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class FliersController : MonoBehaviour
    {
        [SerializeField] private FliersControllerSettings settings;
        [SerializeField] private Transform poolTransform;
        [SerializeField] private RectTransform flierPrefab;

        private ControllersManager _controllersManager;
        private PoolOfObjects<Flier> _poolOfFliers;
        private Vector2 _sceneSize;
        private float _sceneHalfHeight;
        private float _sceneHalfWidth;
        private int _currentNumberOfFliers;
        private float _flierRadius;

        private List<Flier> _fliers;
        public List<Flier> ActiveFliers => _fliers;
        public FliersControllerSettings Settings => settings;
        public float FlierRadius => _flierRadius;
        public int CurrentNumberOfFliers => _currentNumberOfFliers;
        public int CurrentNumberOfBombs { get; set; }
        public int CurrentNumberOfLifes { get; set; }

        public void Initialize()
        {
            _controllersManager = ControllersManager.Instance;
            _sceneSize = _controllersManager.SceneController.SceneSize;
            _sceneHalfWidth = _sceneSize.x / 2;
            _sceneHalfHeight = _sceneSize.y / 2;
            _flierRadius = flierPrefab.rect.height / 2;
            
            
            _poolOfFliers = new PoolOfObjects<Flier>(flierPrefab.gameObject, poolTransform);
            _fliers = new List<Flier>();
            _currentNumberOfFliers = settings.MinNumberOfFliers;
        }

        public void ReInit()
        {
            _currentNumberOfFliers = settings.MinNumberOfFliers;

            foreach (var flier in _fliers)
            {
                _poolOfFliers.Put(flier);
                flier.Switch(false);
            }
            
            _fliers.Clear();
        }

        public void LaunchFliers()
        {
            if (_controllersManager.GameController.IsPlayingBlocked) 
                return;
            
            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    var (startPoint, angle) = GetStartRandomValues();
                    var flier = _poolOfFliers.Get();
                    flier.Switch(true);
                    if (!_fliers.Contains(flier)) _fliers.Add(flier);
                    flier.ReInit(startPoint, angle);
                })
                .AppendInterval(Random.Range(settings.SpawnDelay.from, settings.SpawnDelay.to))
                .SetLoops(_currentNumberOfFliers);
        }

        private (Vector3 startPoint, float angle) GetStartRandomValues()
        {
            Vector3 startPoint = default;
            float angle = default;
            float spawnValidator = Random.Range(0f, 1f);
            float probability = 0f;
            
            for (var i = 0; i < settings.SpawnZones.Length; i++)
            {
                probability += settings.SpawnZones[i].SpawnProbability;
                if (!(spawnValidator <= probability)) continue;
                
                float xPos, yPos;
                    
                switch (settings.SpawnZones[i].SpawnAreaType)
                {
                    case SpawnAreaTypes.Left:
                    {
                        var sceneWidthPosRatio = Random.Range(settings.SpawnZones[i].MinPosRatioToSpawnSide, settings.SpawnZones[i].MaxPosRatioToSpawnSide);
                        xPos = -_sceneHalfWidth - _flierRadius;
                        yPos = sceneWidthPosRatio * _sceneSize.y - _sceneHalfHeight;
                        break;
                    }
                    case SpawnAreaTypes.Right:
                    {
                        var sceneWidthPosRatio = Random.Range(settings.SpawnZones[i].MinPosRatioToSpawnSide, settings.SpawnZones[i].MaxPosRatioToSpawnSide);
                        xPos = _sceneHalfWidth + _flierRadius;
                        yPos = sceneWidthPosRatio * _sceneSize.y - _sceneHalfHeight;
                        break;
                    }
                    default:
                    {
                        var sceneWidthPosRatio = Random.Range(settings.SpawnZones[i].MinPosRatioToSpawnSide, settings.SpawnZones[i].MaxPosRatioToSpawnSide);
                        xPos = sceneWidthPosRatio * _sceneSize.x - _sceneHalfWidth;
                        yPos = -_sceneHalfHeight - _flierRadius;
                        break;
                    }
                }

                var minAngle = settings.SpawnZones[i].Angle - settings.SpawnZones[i].AngleDeviation;
                var maxAngle = settings.SpawnZones[i].Angle + settings.SpawnZones[i].AngleDeviation;
                    
                angle = Random.Range(minAngle, maxAngle);
                startPoint = new Vector2(xPos, yPos);

                break;
            }
            
            return (startPoint, angle);
        }
        
        private bool CheckIfPointOnScene(Vector3 point, float indent = 0)
        {
            return point.x > - _sceneHalfWidth - indent
                   && point.x < _sceneHalfWidth + indent
                   && point.y > - _sceneHalfHeight - indent
                   && point.y < _sceneHalfHeight + indent;
        }

        public bool CheckIfBombsNumberIsOk() => CurrentNumberOfBombs+1 < _currentNumberOfFliers * settings.BombsFractionInPack;
        public bool CheckIfLifesNumberIsOk() => CurrentNumberOfLifes+1 < _currentNumberOfFliers * settings.LifesFractionInPack;

        private void MoveFliers()
        {
            int index = 0;
            while (index < _fliers.Count)
            {
                var (leftHalfPos, rightHalfPos) = _fliers[index].MoveAlongTrajectory(settings.JumpPower, settings.FlierSpeed);

                if (!CheckIfPointOnScene(leftHalfPos, _flierRadius) && !CheckIfPointOnScene(rightHalfPos, _flierRadius))
                {
                    switch (_fliers[index].KindOfFlierMechanic)
                    {
                        case KindOfFlierMechanic.Bomb:
                            ControllersManager.Instance.FliersController.CurrentNumberOfBombs--;
                            break;
                        case KindOfFlierMechanic.Life:
                            ControllersManager.Instance.FliersController.CurrentNumberOfLifes--;
                            break;
                        default:
                        {
                            if (!_fliers[index].IsDissected && _fliers[index].KindOfFlierMechanic == KindOfFlierMechanic.Fruit)
                                _controllersManager.GameController.DecreaseHp();
                            break;
                        }
                    }

                    RemoveFlier(_fliers[index]);

                    if (_fliers.Count != 0) continue;
                    if (_currentNumberOfFliers < settings.MaxNumberOfFliers)
                    {
                        _currentNumberOfFliers++;
                    }
                        
                    LaunchFliers();
                }
                else index++;
            }
        }

        private void RemoveFlier(Flier flier)
        {
            _poolOfFliers.Put(flier);
            flier.Switch(false);
            _fliers.Remove(flier);
        }

        private void Update()
        {
            MoveFliers();
        }
    }
}

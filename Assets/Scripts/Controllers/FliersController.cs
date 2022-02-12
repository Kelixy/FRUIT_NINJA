using System.Collections.Generic;
using DG.Tweening;
using Models;
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
        private PoolOfFliers _poolOfFliers;
        private Vector2 _sceneSize;
        private float _sceneHalfHeight;
        private float _sceneHalfWidth;
        private int _numberOfFliers;
        private float _flierRadius;

        private List<Flier> _fliers;
        public List<Flier> ActiveFliers => _fliers;
        public float FlierRadius => _flierRadius;

        public void Initialize()
        {
            _controllersManager = ControllersManager.Instance;
            _sceneSize = _controllersManager.SceneController.SceneSize;
            _sceneHalfWidth = _sceneSize.x / 2;
            _sceneHalfHeight = _sceneSize.y / 2;
            _flierRadius = flierPrefab.rect.height / 2;
            
            
            _poolOfFliers = new PoolOfFliers(flierPrefab.gameObject, poolTransform, settings.MinNumberOfFliers);
            _fliers = new List<Flier>();
            _numberOfFliers = settings.MinNumberOfFliers;
        }

        public void ReInit()
        {
            _numberOfFliers = settings.MinNumberOfFliers;
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
                .SetLoops(_numberOfFliers);
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

        private void MoveFliers()
        {
            int index = 0;
            while (index < _fliers.Count)
            {
                var nextPoint = _fliers[index].MoveAlongTrajectory(settings.JumpPower, settings.FlierSpeed);

                if (!CheckIfPointOnScene(nextPoint.leftHalfPos, _flierRadius) && !CheckIfPointOnScene(nextPoint.rightHalfPos, _flierRadius))
                {
                    if (!_fliers[index].IsDissected)
                        _controllersManager.SceneController.HealthPoints.DecreaseHP();
                    
                    _poolOfFliers.Put(_fliers[index]);
                    _fliers[index].Switch(false);
                    _fliers.Remove(_fliers[index]);

                    if (_fliers.Count == 0)
                    {
                        if (_numberOfFliers < settings.MaxNumberOfFliers)
                        {
                            _controllersManager.SceneController.BackgroundEffects.IncreaseCloudSpeed();
                            _numberOfFliers++;
                        }
                        
                        LaunchFliers();
                    }
                }
                else index++;
            }
        }

        private void Update()
        {
            MoveFliers();
        }
    }
}

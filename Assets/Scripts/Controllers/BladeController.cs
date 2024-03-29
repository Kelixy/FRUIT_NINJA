using System;
using Mechanics;
using UnityEngine;

namespace Controllers
{
    public class BladeController : MonoBehaviour
    {
        private const float HoldPointerCooldown = 0.1f;
        
        [SerializeField] private Transform bladePoint;
        [SerializeField] private ParticleSystem bladeTrack;
        [SerializeField] private new Camera camera;

        private ControllersManager _controllersManager;
        private Vector3 _cachedBladePos;
        private Vector3 _bladePos;
        private bool _isBladeUnsheathed;
        private bool _isCountingHoldTimer;
        private int _cachedPoints;
        private float _pieceOfSecond;
        private float _holdPointerTimer;

        public void Initialize()
        {
            _controllersManager = ControllersManager.Instance;
        }

        private void DissectFlierIfPossible()
        {
            if (!_isBladeUnsheathed) return;

            _bladePos = camera.ScreenToWorldPoint(Input.mousePosition);
            _bladePos.z = 0;
            bladePoint.localPosition = _bladePos;
            var bladeDirection = (_bladePos - _cachedBladePos).normalized;

            foreach (var flier in _controllersManager.FliersController.ActiveFliers)
            {
                if (!flier.IsDissected && Count2dDistance(_bladePos, flier.transform.position) <
                    _controllersManager.FliersController.FlierRadius)
                {
                    flier.DissectTheFlier(bladeDirection);
                    
                    if (flier.KindOfFlierMechanic == KindOfFlierMechanic.Fruit)
                        _cachedPoints++;
                }
            }

            _cachedBladePos = _bladePos;
        }

        private float Count2dDistance(Vector3 pointA, Vector3 pointB)
        {
            var catetX = pointB.x - pointA.x;
            var catetY = pointB.y - pointA.y;
            return (float) Math.Sqrt(catetX*catetX + catetY*catetY);
        }

        private void AddCachedPoints()
        {
            _controllersManager.SceneController.Score.IncreaseScore(CountPointsAndBonus(_cachedPoints));
            _cachedPoints = 0;
        }

        private int CountPointsAndBonus(int value)
        {
            if (value == 1) return value;
            
            var sum = 0;
            for (var i = 1; i <= value; i++)
                sum += i;

            return sum;
        }

        private void PlayOrStopBladeTrack()
        {
            _isBladeUnsheathed = CheckIfHoldPointer();

            switch (_isBladeUnsheathed)
            {
                case true when !bladeTrack.isPlaying:
                    bladeTrack.Play();
                    break;
                case false when bladeTrack.isPlaying:
                    bladeTrack.Stop();
                    break;
            }
        }

        private void CachePoints()
        {
            _pieceOfSecond += Time.deltaTime;

            if (!(_pieceOfSecond >= 1)) return;
            AddCachedPoints();
            _pieceOfSecond = 0;
        }

        private bool CheckIfHoldPointer()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isCountingHoldTimer = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isCountingHoldTimer = false;
                _holdPointerTimer = 0;
            }

            if (_isCountingHoldTimer)
            {
                _holdPointerTimer += Time.deltaTime;

                if (_holdPointerTimer > HoldPointerCooldown)
                {
                    return true;
                }
            }

            return false;
        }

        private void Update()
        {
            if (ControllersManager.Instance.GameController.IsPlayingBlocked)
                return;

            CachePoints();
            PlayOrStopBladeTrack();
            DissectFlierIfPossible();
        }
    }
}

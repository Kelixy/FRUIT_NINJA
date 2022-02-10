using System;
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
                    _cachedPoints++;
                }
            }

            _cachedBladePos = _bladePos;
        }
        
        private float Count2dDistance(Vector3 pointA, Vector3 pointB)
        {
            return (float)Math.Sqrt(Math.Pow((pointB.x - pointA.x), 2) + Math.Pow((pointB.y - pointA.y), 2));
        }

        private void AddCachedPoints()
        {
            if (_cachedPoints > 1)
                _cachedPoints += _cachedPoints / 2;
            _controllersManager.SceneController.Score.IncreaseScore(_cachedPoints);
            _cachedPoints = 0;
        }

        private void PlayOrStopBladeTrack()
        {
            _isBladeUnsheathed = CheckIfHoldPointer();
            if (_isBladeUnsheathed && !bladeTrack.isPlaying)
                bladeTrack.Play();
            else if (!_isBladeUnsheathed && bladeTrack.isPlaying)
                bladeTrack.Stop();
        }

        private void CachePoints()
        {
            _pieceOfSecond += Time.deltaTime;
            
            if (_pieceOfSecond >= 1)
            {
                AddCachedPoints();
                _pieceOfSecond = 0;
            }
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

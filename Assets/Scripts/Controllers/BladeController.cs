using System;
using UnityEngine;

namespace Controllers
{
    public class BladeController : MonoBehaviour
    {
        [SerializeField] private Transform bladePoint;
        [SerializeField] private ParticleSystem bladeTrack;
        [SerializeField] private new Camera camera;

        private ControllersManager _controllersManager;
        private Vector3 _cachedBladePos;
        private Vector3 _bladePos;
        private bool _isBladeUnsheathed;

        public void Initialize()
        {
            _controllersManager = ControllersManager.Instance;
        }

        private void DissectFlierIfPossible()
        {
            if (!_isBladeUnsheathed) return;
            
            _bladePos = (Vector2)Input.mousePosition - ControllersManager.Instance.SceneController.SceneSize / 2;
            _bladePos.z = 0;
            var _bladePointPos = camera.ScreenToWorldPoint(Input.mousePosition);
            _bladePointPos.z = 90;
            bladePoint.localPosition = _bladePointPos;
            var bladeDirection = (_bladePos - _cachedBladePos).normalized;
            
            foreach (var flier in _controllersManager.FliersController.ActiveFliers)
            {
                if (!flier.IsDissected && Count2dDistance(_bladePos, flier.transform.localPosition) <
                    _controllersManager.FliersController.FlierRadius)
                {
                    flier.DissectTheFlier(bladeDirection);
                    _controllersManager.SceneController.Score.IncreaseScore();
                }
            }

            _cachedBladePos = _bladePos;
        }
        
        private float Count2dDistance(Vector3 pointA, Vector3 pointB)
        {
            return (float)Math.Sqrt(Math.Pow((pointB.x - pointA.x), 2) + Math.Pow((pointB.y - pointA.y), 2));
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                bladeTrack.Play();
                _isBladeUnsheathed = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                bladeTrack.Stop();
                _isBladeUnsheathed = false;
            }
            
            DissectFlierIfPossible();
        }
    }
}

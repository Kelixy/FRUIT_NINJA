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
        private Vector3 _mousePos;
        private bool _isBladeUnsheathed;

        public void Initialize()
        {
            _controllersManager = ControllersManager.Instance;
        }

        private void DissectFlierIfPossible()
        {
            if (!_isBladeUnsheathed) return;
                
            _mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            _mousePos.z = 0;
            bladePoint.position = _mousePos;
                
            foreach (var flier in _controllersManager.FliersController.ActiveFliers)
            {
                if (!flier.IsDissected && Count2dDistance(bladePoint.localPosition, flier.transform.localPosition) <
                    _controllersManager.FliersController.FlierRadius)
                {
                    flier.DissectTheFlier();
                    _controllersManager.SceneController.Score.IncreaseScore();
                }
            }
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

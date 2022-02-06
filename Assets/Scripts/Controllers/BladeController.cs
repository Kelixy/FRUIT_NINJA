using System;
using UnityEngine;

namespace Controllers
{
    public class BladeController : MonoBehaviour
    {
        [SerializeField] private Transform bladePoint;
        [SerializeField] private ParticleSystem bladeTrack;
        [SerializeField] private FliersController fliersController;
        [SerializeField] private new Camera camera;

        private Vector3 _mousePos;
        private bool _isBladeUnsheathed;

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
            
            if (_isBladeUnsheathed)
            {
                _mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                _mousePos.z = 0;
                bladePoint.position = _mousePos;
                
                foreach (var flier in fliersController.ActiveFliers)
                {
                    
                    if (Count2dDistance(bladePoint.localPosition, flier.transform.localPosition) <
                        fliersController.FlierRadius)
                    {
                        flier.DissectTheFlier();
                    }
                }
            }
        }
    }
}

using Models;
using UnityEngine;

namespace Views
{
    public class Flier : MonoBehaviour
    {
        public bool IsActive => gameObject.activeSelf;
        private float FlyingAngle { get; set; }
        private float LifeTimer { get; set; }
        
        public void ReInit(Vector3 startLocalPosition, float flyingAngle)
        {
            transform.localPosition = startLocalPosition;
            FlyingAngle = flyingAngle;
        }

        public void Switch(bool shouldBeActive)
        {
            gameObject.SetActive(shouldBeActive);
            LifeTimer = 0;
        }

        public Vector3 MoveAlongTrajectory(float plusTime, float speed)
        {
            LifeTimer += plusTime * speed;
            var nextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(transform.localPosition,
                speed, LifeTimer, FlyingAngle);
            transform.localPosition = nextPoint;
            
            return nextPoint;
        }
    }
}

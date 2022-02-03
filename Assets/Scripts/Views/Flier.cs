using Models;
using UnityEngine;

namespace Views
{
    public class Flier : MonoBehaviour
    {
        public bool IsActive => gameObject.activeSelf;
        public float FlyingAngle { private get; set; }
        private float LifeTimer { get; set; }
        
        public void Switch(bool shouldBeActive)
        {
            gameObject.SetActive(shouldBeActive);
            LifeTimer = 0;
        }

        public Vector3 MoveAlongTrajectory(float plusTime, float speed)
        {
            LifeTimer += plusTime;
            var nextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(transform.localPosition,
                speed, LifeTimer, FlyingAngle);
            transform.localPosition = nextPoint;
            
            return nextPoint;
        }
    }
}

using Models;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Views
{
    public class Flier : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Image leftImg;
        [SerializeField] private Image rightImg;
        public bool IsActive => gameObject.activeSelf;
        private float FlyingAngle { get; set; }
        private float LifeTimer { get; set; }
        private bool _isDissected;
        
        public void ReInit(Vector3 startLocalPosition, float flyingAngle)
        {
            _isDissected = false;
            transform.rotation = default;
            leftImg.transform.rotation = default;
            rightImg.transform.rotation = default;
            leftImg.transform.localPosition = Vector3.zero;
            rightImg.transform.localPosition = Vector3.zero;
            transform.localPosition = startLocalPosition;
            FlyingAngle = flyingAngle;
        }

        public void Switch(bool shouldBeActive)
        {
            gameObject.SetActive(shouldBeActive);
            LifeTimer = 0;
        }

        public void DissectTheFlier() => _isDissected = true;

        public Vector3 MoveAlongTrajectory(float plusTime, float speed)
        {
            LifeTimer += plusTime * speed;
            var nextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(transform.localPosition,
                speed, LifeTimer, FlyingAngle);
            transform.localPosition = nextPoint;

            if (_isDissected)
            {
                transform.rotation = default;
                var leftHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(leftImg.transform.localPosition, speed, LifeTimer, 165f);
                var rightHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(rightImg.transform.localPosition, speed, LifeTimer, 15f);

                leftImg.transform.localPosition = leftHalfNextPoint;
                rightImg.transform.localPosition = rightHalfNextPoint;
                leftImg.transform.Rotate(0,0,1);
                rightImg.transform.Rotate(0,0,-1);
            }
            else transform.Rotate(0,0,-1);
            
            return nextPoint;
        }
        
        private void OnEnable()
        {
            int index = Random.Range(0, sprites.Length);
            leftImg.sprite = sprites[index];
            rightImg.sprite = sprites[index];
        }
    }
}

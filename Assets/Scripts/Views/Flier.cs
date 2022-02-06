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
        private Vector3 _startLocalPosition;
        
        public void ReInit(Vector3 startLocalPosition, float flyingAngle)
        {
            _isDissected = false;
            transform.rotation = default;
            leftImg.transform.rotation = default;
            rightImg.transform.rotation = default;
            leftImg.transform.localPosition = Vector3.zero;
            rightImg.transform.localPosition = Vector3.zero;
            _startLocalPosition = startLocalPosition;
            transform.localPosition = startLocalPosition;
            FlyingAngle = flyingAngle;
        }

        public void Switch(bool shouldBeActive)
        {
            LifeTimer = 0;
            gameObject.SetActive(shouldBeActive);
        }

        public void DissectTheFlier() => _isDissected = true;

        public Vector3 MoveAlongTrajectory(float jumpPower, int speed)
        {
            LifeTimer += Time.deltaTime;
            var nextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer, FlyingAngle);
            transform.localPosition = _startLocalPosition + nextPoint * speed;

            if (_isDissected)
            {
                transform.rotation = default;
                var leftHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer, 165f);
                var rightHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer, 15f);

                leftImg.transform.localPosition += leftHalfNextPoint;
                rightImg.transform.localPosition += rightHalfNextPoint;
                leftImg.transform.Rotate(0,0,1);
                rightImg.transform.Rotate(0,0,-1);
            }
            else transform.Rotate(0,0,-1);
            
            return transform.localPosition;
        }
        
        private void OnEnable()
        {
            int index = Random.Range(0, sprites.Length);
            leftImg.sprite = sprites[index];
            rightImg.sprite = sprites[index];
        }
    }
}

using Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Views
{
    public class Flier : MonoBehaviour
    {
        [SerializeField] private FlierSettings[] flierSettings;
        [SerializeField] private SpriteRenderer leftHalf;
        [SerializeField] private SpriteRenderer rightHalf;
        [SerializeField] private Transform leftPointTransform;
        [SerializeField] private Transform rightPointTransform;
        [SerializeField] private ParticleSystem splashEffect;
        [SerializeField] private RectTransform rectTransform;
        
        private Vector3 _startLocalPosition;
        private Vector3 _bladeDirection;
        private float _slicesFallingSpeed;
        private float _leftSlideFallingAngle;
        private float _rightSlideFallingAngle;
        
        public bool IsDissected { get; private set; }
        private float FlyingAngle { get; set; }
        private float LifeTimer { get; set; }
        private Transform LeftHalfTransform => leftHalf.transform;
        private Transform RightHalfTransform => rightHalf.transform;

        public void ReInit(Vector3 startLocalPosition, float flyingAngle)
        {
            IsDissected = false;
            transform.rotation = default;
            LeftHalfTransform.rotation = default;
            RightHalfTransform.rotation = default;
            LeftHalfTransform.localPosition = leftPointTransform.localPosition;
            RightHalfTransform.localPosition = rightPointTransform.localPosition;
            _startLocalPosition = startLocalPosition;
            transform.localPosition = startLocalPosition;
            FlyingAngle = flyingAngle;
        }

        public void Switch(bool shouldBeActive)
        {
            LifeTimer = 0;
            gameObject.SetActive(shouldBeActive);
        }

        public void DissectTheFlier(Vector3 bladeDirection)
        {
            IsDissected = true;
            _bladeDirection = bladeDirection;
            LifeTimer = 0;
            splashEffect.Play();
        }

        public (Vector3 leftHalfPos, Vector3 rightHalfPos) MoveAlongTrajectory(float jumpPower, int speed)
        {
            LifeTimer += Time.deltaTime;
            var nextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer, FlyingAngle);
            if (!IsDissected)
                transform.localPosition = _startLocalPosition + nextPoint * speed;

            if (IsDissected)
            {
                transform.rotation = default;
                transform.localPosition += _bladeDirection*2 + Vector3.down;
                var fallingSpeed = Random.Range(2f, 4f);
                var leftHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer*3, 165f);
                var rightHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer*2, 15f);

                LeftHalfTransform.localPosition = leftHalfNextPoint;
                RightHalfTransform.localPosition = rightHalfNextPoint;
                LeftHalfTransform.Rotate(0,0,0.5f);
                RightHalfTransform.Rotate(0,0,-0.7f);
            }
            else transform.Rotate(0,0,-1);
            
            return (transform.localPosition + LeftHalfTransform.localPosition * rectTransform.localScale.x, transform.localPosition + RightHalfTransform.localPosition * rectTransform.localScale.x);
        }
        
        private void OnEnable()
        {
            var index = Random.Range(0, flierSettings.Length);
            leftHalf.sprite = flierSettings[index].LeftHalfSprite;
            rightHalf.sprite = flierSettings[index].RightHalfSprite;
            var splashEffectMain = splashEffect.main;
            splashEffectMain.startColor = flierSettings[index].SplashColor;
        }
    }
}

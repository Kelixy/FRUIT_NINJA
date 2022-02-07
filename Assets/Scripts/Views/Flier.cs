using Models;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Views
{
    public class Flier : MonoBehaviour
    {
        [SerializeField] private FlierSettings[] flierSettings;
        [SerializeField] private Image leftImg;
        [SerializeField] private Image rightImg;
        [SerializeField] private ParticleSystem splashEffect;
        
        private Vector3 _startLocalPosition;
        
        public bool IsDissected { get; private set; }
        private float FlyingAngle { get; set; }
        private float LifeTimer { get; set; }
        private Transform LeftHalfTransform => leftImg.transform;
        private Transform RightHalfTransform => rightImg.transform;

        public void ReInit(Vector3 startLocalPosition, float flyingAngle)
        {
            IsDissected = false;
            transform.rotation = default;
            leftImg.transform.rotation = default;
            rightImg.transform.rotation = default;
            LeftHalfTransform.localPosition = Vector3.zero;
            RightHalfTransform.localPosition = Vector3.zero;
            _startLocalPosition = startLocalPosition;
            transform.localPosition = startLocalPosition;
            FlyingAngle = flyingAngle;
        }

        public void Switch(bool shouldBeActive)
        {
            LifeTimer = 0;
            gameObject.SetActive(shouldBeActive);
        }

        public void DissectTheFlier()
        {
            IsDissected = true;
            splashEffect.Play();
        }

        public Vector3 MoveAlongTrajectory(float jumpPower, int speed)
        {
            LifeTimer += Time.deltaTime;
            var nextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer, FlyingAngle);
            transform.localPosition = _startLocalPosition + nextPoint * speed;

            if (IsDissected)
            {
                transform.rotation = default;
                var leftHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer, 165f);
                var rightHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer, 15f);

                LeftHalfTransform.localPosition += leftHalfNextPoint;
                RightHalfTransform.localPosition += rightHalfNextPoint;
                LeftHalfTransform.Rotate(0,0,1);
                RightHalfTransform.Rotate(0,0,-1);
            }
            else transform.Rotate(0,0,-1);
            
            return transform.localPosition;
        }
        
        private void OnEnable()
        {
            var index = Random.Range(0, flierSettings.Length);
            leftImg.sprite = flierSettings[index].Sprite;
            rightImg.sprite = flierSettings[index].Sprite;
            var splashEffectMain = splashEffect.main;
            splashEffectMain.startColor = flierSettings[index].SplashColor;
        }
    }
}

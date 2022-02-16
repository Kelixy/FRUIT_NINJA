using Controllers;
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
        private int _kindOfSettings;
        
        public bool IsDissected { get; private set; }
        private float FlyingAngle { get; set; }
        private float LifeTimer { get; set; }
        private Transform LeftHalfTransform => leftHalf.transform;
        private Transform RightHalfTransform => rightHalf.transform;
        public KindOfFlierMechanic KindOfFlierMechanic => (KindOfFlierMechanic)flierSettings[_kindOfSettings].KindOfMechanic;

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
            if (IsDissected) return;
            
            IsDissected = true;
            _bladeDirection = bladeDirection;
            LifeTimer = 0;
            splashEffect.Play();
            FlierMechanics.DoMechanic(KindOfFlierMechanic);
        }

        public (Vector3 leftHalfPos, Vector3 rightHalfPos) MoveAlongTrajectory(float jumpPower, int speed)
        {
            LifeTimer += Time.deltaTime;
            var nextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer * flierSettings[_kindOfSettings].SpeedMultiplier, FlyingAngle);
            if (!IsDissected)
            {
                transform.localPosition = _startLocalPosition + nextPoint * speed;
                transform.Rotate(0,0,-1);
            }
            else
            {
                transform.rotation = default;
                transform.localPosition += _bladeDirection*2 + Vector3.down;
                
                var leftHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer*3, 165f);
                var rightHalfNextPoint = TrajectoryCounter.GetTrajectoryPointInMoment(jumpPower, LifeTimer*2, 15f);

                LeftHalfTransform.localPosition = leftHalfNextPoint;
                RightHalfTransform.localPosition = rightHalfNextPoint;
                LeftHalfTransform.Rotate(0,0,0.5f);
                RightHalfTransform.Rotate(0,0,-0.7f);
            }
            
            return (transform.localPosition + LeftHalfTransform.localPosition * rectTransform.localScale.x, transform.localPosition + RightHalfTransform.localPosition * rectTransform.localScale.x);
        }
        
        private void OnEnable()
        {
            var fliersController = ControllersManager.Instance.FliersController;
            var isLife = false;
            var isBomb = Random.Range(0f, 1f) <= fliersController.Settings.BombsProbability && fliersController.CheckIfBombsNumberIsOk();
            if (!isBomb) isLife = Random.Range(0f, 1f) <= fliersController.Settings.LifesProbability && !ControllersManager.Instance.SceneController.HealthPoints.CheckIfMaxHpReached() && fliersController.CheckIfLifesNumberIsOk();
            
            if (isBomb)
            {
                fliersController.CurrentNumberOfBombs++;
                _kindOfSettings = (int) KindOfFlierMechanic.Bomb;
            }
            else if (isLife)
            {
                fliersController.CurrentNumberOfLifes++;
                _kindOfSettings = (int) KindOfFlierMechanic.Life;
            }
            else
            {
                _kindOfSettings = Random.Range(0, flierSettings.Length-2);
            }
            
            leftHalf.sprite = flierSettings[_kindOfSettings].LeftHalfSprite;
            rightHalf.sprite = flierSettings[_kindOfSettings].RightHalfSprite;
            var splashEffectMain = splashEffect.main;
            splashEffectMain.startColor = flierSettings[_kindOfSettings].SplashColor;
        }
    }
}

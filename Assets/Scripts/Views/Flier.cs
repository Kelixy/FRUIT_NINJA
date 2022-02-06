using System;
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

        private void OnEnable()
        {
            int index = Random.Range(0, sprites.Length);
            leftImg.sprite = sprites[index];
            rightImg.sprite = sprites[index];
        }
    }
}

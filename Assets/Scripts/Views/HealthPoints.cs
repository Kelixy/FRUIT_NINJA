using Controllers;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class HealthPoints : MonoBehaviour
    {
        private const int MaxHp = 6;
        
        [SerializeField] private RectTransform[] hearts;
        [Range(1,MaxHp)] [SerializeField] private int startHeartsNumber;
        private int _heartsNumber;

        private int HeartsNumber
        {
            get => _heartsNumber;
            set
            {
                if (value >= 0 && value <= MaxHp)
                    _heartsNumber = value;
            }
        }
        
        public void ReInit()
        {
            _heartsNumber = startHeartsNumber;
            for (var i = 0; i < _heartsNumber; i++)
            {
                AddHeart(i);
            }
        }

        public void IncreaseHP()
        {
            if (HeartsNumber >= hearts.Length) return;
            AddHeart(HeartsNumber);
            HeartsNumber++;
        }
        
        public void DecreaseHP()
        {
            if (HeartsNumber == 0) return;
            
            HeartsNumber--;
            
            if (HeartsNumber == 0 && !ControllersManager.Instance.GameController.IsPlayingBlocked)
            {
                ControllersManager.Instance.GameController.EndGame();
            }

            RemoveHeart(HeartsNumber);
        }

        private void AddHeart(int index)
        {
            if (hearts[index].localScale.x < 1)
                DOTween.Sequence().Append(hearts[index].DOScale(Vector3.one, 1)).SetEase(Ease.Flash);
        }
        
        private void RemoveHeart(int index)
        {
            if (hearts[index].localScale.x > 0)
                DOTween.Sequence().Append(hearts[index].DOScale(Vector3.zero, 1)).SetEase(Ease.Flash);
        }
    }
}

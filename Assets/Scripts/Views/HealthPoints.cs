using Controllers;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class HealthPoints : MonoBehaviour
    {
        private const int MaxHp = 6;
        
        [SerializeField] private RectTransform[] hearts;
        [Range(1,MaxHp)] [SerializeField] private int heartsNumber;

        private int HeartsNumber
        {
            get => heartsNumber;
            set
            {
                if (value >= 0 && value <= MaxHp)
                    heartsNumber = value;
            }
        }
        
        public void ReInit()
        {
            for (var i = 0; i < heartsNumber; i++)
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
            HeartsNumber--;
            
            if (HeartsNumber == 0)
            {
                ControllersManager.Instance.GameController.EndRound();
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

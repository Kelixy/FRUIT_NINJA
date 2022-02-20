using Counters;
using DG.Tweening;
using UnityEngine;

namespace Views
{
    public class HeartsPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform[] hearts;

        [Range(1, HealthPointsCounter.MaxHp)] [SerializeField]
        private int startHeartsNumber;

        private int _heartsNumber;

        public void Initialize()
        {
            ReInit(startHeartsNumber);
        }

        private void ReInit(int heartsNumber)
        {
            _heartsNumber = heartsNumber;
            for (var i = 0; i < _heartsNumber; i++)
            {
                AddHeart(i);
            }
        }

        public void AddHeart(int index)
        {
            if (index >= hearts.Length) return;
            if (hearts[index].localScale.x < 1)
                DOTween.Sequence().Append(hearts[index].DOScale(Vector3.one, 1)).SetEase(Ease.Flash);
        }

        public void RemoveHeart(int index)
        {
            if (index < 0) return;
            if (hearts[index].localScale.x > 0)
                DOTween.Sequence().Append(hearts[index].DOScale(Vector3.zero, 1)).SetEase(Ease.Flash);
        }
    }
}
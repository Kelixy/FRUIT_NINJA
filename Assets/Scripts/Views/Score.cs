using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private Text scoreLabel;
        [SerializeField] private Text bestScoreLabel;
        [SerializeField] private Transform hideScorePoint;

        private int _bestScore;
        private string _bestScoreHeader = "Best: ";
        private string _bestScoreKey = "best_score";
        
        public int CurrentScore { get; private set; }
        public int BestScore => _bestScore;
        public int NumberOfDissectedFruits { get; private set; }

        public void ReInit()
        {
            _bestScore = PlayerPrefs.GetInt(_bestScoreKey);
            scoreLabel.text = CurrentScore.ToString();
            bestScoreLabel.text = _bestScoreHeader + _bestScore;
        }

        public void IncreaseScore(int points)
        {
            NumberOfDissectedFruits++;
            CurrentScore += points;
            ReInit();

            if (CurrentScore > _bestScore)
            {
                _bestScore = CurrentScore;
                bestScoreLabel.text = _bestScoreHeader + _bestScore;
                PlayerPrefs.SetInt(_bestScoreKey, _bestScore);
            }
        }

        public void ShowScoreLabel()
        {
            DOTween.Sequence()
                .Append(scoreLabel.rectTransform.DOAnchorPos(Vector2.zero, 1));
        }

        public void HideScoreLabel()
        {
            DOTween.Sequence()
                .Append(scoreLabel.rectTransform.DOAnchorPos(hideScorePoint.localPosition, 1));
        }
    }
}

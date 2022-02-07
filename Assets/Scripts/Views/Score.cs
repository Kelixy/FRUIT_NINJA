using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private Text scoreLabel;
        [SerializeField] private Text bestScoreLabel;

        private int _score;
        private int _bestScore;
        private string _bestScoreHeader = "Best: ";
        private string _bestScoreKey = "best_score";

        public void ReInit()
        {
            _bestScore = PlayerPrefs.GetInt(_bestScoreKey);
            scoreLabel.text = _score.ToString();
            bestScoreLabel.text = _bestScoreHeader + _bestScore;
        }

        public void IncreaseScore()
        {
            _score++;
            ReInit();

            if (_score > _bestScore)
            {
                _bestScore = _score;
                bestScoreLabel.text = _bestScoreHeader + _bestScore;
                PlayerPrefs.SetInt(_bestScoreKey, _bestScore);
            }
        }
    }
}

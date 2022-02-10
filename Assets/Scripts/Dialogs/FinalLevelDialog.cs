using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogs
{
    public class FinalLevelDialog : Dialog
    {
        [SerializeField] private Text finalMessage;
        [SerializeField] private Text finalSubMessage;
        public new void Show()
        {
            ControllersManager.Instance.SceneController.Score.HideScoreLabel();
            var numberOfDissectedFruits = ControllersManager.Instance.SceneController.Score.NumberOfDissectedFruits;
            var currentScore = ControllersManager.Instance.SceneController.Score.CurrentScore;
            var bestScore = ControllersManager.Instance.SceneController.Score.BestScore;
            finalMessage.text = $"You've dissected {numberOfDissectedFruits} fruits!";
            finalSubMessage.text = $"Your score: {currentScore}\nBest score: {bestScore}";
            base.Show();
        }
        
        public new void Hide()
        {
            base.Hide();
        }
    }
}
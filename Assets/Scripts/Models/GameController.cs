using Controllers;
using Dialogs;
using UnityEngine;

namespace Models
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private FinalLevelDialog finalLevelDialog;
        
        private ControllersManager _controllersManager;
        
        public bool IsPlayingBlocked { get; private set; }
        
        private void Start()
        {
            _controllersManager = ControllersManager.Instance;
            
            _controllersManager.SceneController.Initialize();
            _controllersManager.BladeController.Initialize();
            _controllersManager.FliersController.Initialize();
        }

        public void StartGame()
        {
            IsPlayingBlocked = false;
            _controllersManager.FliersController.ReInit();
            _controllersManager.FliersController.LaunchFliers();
        }

        public void EndGame()
        {
            ControllersManager.Instance.SceneController.Score.StopAnimatedIncrease();
            ControllersManager.Instance.SceneController.FinalLevelDialog.Show();
            IsPlayingBlocked = true;
        }

        private void ReInitAllParams()
        {
            ControllersManager.Instance.SceneController.Score.ReInit();
            ControllersManager.Instance.SceneController.Score.ShowScoreLabel();
            ControllersManager.Instance.SceneController.FinalLevelDialog.Hide();
            ControllersManager.Instance.SceneController.HealthPoints.ReInit();
            ControllersManager.Instance.FliersController.ReInit();
        }

        public void Replay()
        {
            ReInitAllParams();
            StartGame();
        }

        public void ExitToMenu()
        {
            ReInitAllParams();
            ControllersManager.Instance.SceneController.FinalLevelDialog.Hide();
            ControllersManager.Instance.SceneController.StartDialog.Show();
        }
    }
}
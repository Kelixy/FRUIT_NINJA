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

        public void EndGame()
        {
            finalLevelDialog.Show();
            IsPlayingBlocked = true;
        }

        public void Replay()
        {
            ControllersManager.Instance.SceneController.Score.ShowScoreLabel();
            finalLevelDialog.Hide();
            IsPlayingBlocked = false;
        }

        public void ExitToMenu()
        {
            
        }
    }
}
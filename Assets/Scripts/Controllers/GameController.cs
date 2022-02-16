using System;
using Counters;
using Dialogs;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private FinalLevelDialog finalLevelDialog;
        [SerializeField] private int startHp;
        
        private ControllersManager _controllersManager;
        public HealthPointsCounter HealthPointsCounter { get; private set; }
        
        private Action IncreaseHpAction;
        private Action DecreaseHpAction;
        
        public bool IsPlayingBlocked { get; private set; }
        
        private void Start()
        {
            _controllersManager = ControllersManager.Instance;
            HealthPointsCounter = new HealthPointsCounter(startHp);
            
            _controllersManager.SceneController.Initialize();
            _controllersManager.BladeController.Initialize();
            _controllersManager.FliersController.Initialize();
            
            SetGameActions();
        }

        private void SetGameActions()
        {
            IncreaseHpAction = () => { _controllersManager.SceneController.HeartsPanel.AddHeart(HealthPointsCounter.HealthPointsNumber++); };
            DecreaseHpAction = () => { _controllersManager.SceneController.HeartsPanel.RemoveHeart(--HealthPointsCounter.HealthPointsNumber); };
        }

        public void IncreaseHp() => IncreaseHpAction?.Invoke();
        public void DecreaseHp() => DecreaseHpAction?.Invoke();

        public void StartGame()
        {
            IsPlayingBlocked = false;
            _controllersManager.FliersController.ReInit();
            _controllersManager.FliersController.LaunchFliers();
            _controllersManager.SceneController.BackgroundEffects.Reinit();
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
            ControllersManager.Instance.SceneController.HeartsPanel.Initialize();
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
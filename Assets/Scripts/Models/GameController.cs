using Controllers;
using UnityEngine;

namespace Models
{
    public class GameController : MonoBehaviour
    {
        private ControllersManager _controllersManager;
        
        private void Start()
        {
            _controllersManager = ControllersManager.Instance;
            
            _controllersManager.SceneController.Initialize();
            _controllersManager.BladeController.Initialize();
            _controllersManager.FliersController.Initialize();
        }

        public void EndRound()
        {
            
        }
    }
}
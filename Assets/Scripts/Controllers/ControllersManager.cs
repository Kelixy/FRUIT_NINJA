using Models;
using UnityEngine;

namespace Controllers
{
    public class ControllersManager : ComponentSingleton<ControllersManager>
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private FliersController fliersController;
        [SerializeField] private BladeController bladeController;
        [SerializeField] private SceneController sceneController;

        public GameController GameController => gameController;
        public FliersController FliersController => fliersController;
        public BladeController BladeController => bladeController;
        public SceneController SceneController => sceneController;
    }
}

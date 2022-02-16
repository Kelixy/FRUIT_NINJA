using Controllers;

namespace Mechanics
{
    public enum KindOfFlierMechanic
    {
        Fruit,
        Bomb = 4,
        Life
    }

    public static class FlierMechanics
    {
        public static void DoMechanic(KindOfFlierMechanic mechanic)
        {
            switch (mechanic)
            {
                case KindOfFlierMechanic.Fruit :
                    DoFruitMechanic();
                    break;
                case KindOfFlierMechanic.Bomb :
                    DoBombMechanic();
                    break;
                case KindOfFlierMechanic.Life :
                    DoLifeMechanic();
                    break;
            }
        }

        private static void DoFruitMechanic()
        {
            
        }

        private static void DoBombMechanic()
        {
            ControllersManager.Instance.GameController.DecreaseHp();
            ControllersManager.Instance.SceneController.BackgroundEffects.ShakeCamera();
        }

        private static void DoLifeMechanic()
        {
            ControllersManager.Instance.GameController.IncreaseHp();
        }
    }
}
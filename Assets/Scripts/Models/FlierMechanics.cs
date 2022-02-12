using Controllers;

namespace Models
{
    public enum KindOfFlierMechanic
    {
        Fruit,
        Bomb,
        Life
    }

    public static class FlierMechanics
    {
        private static int _cachedPoints;
        
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
            ControllersManager.Instance.SceneController.HealthPoints.DecreaseHP();
            ControllersManager.Instance.SceneController.BackgroundEffects.ShakeCamera();
        }

        private static void DoLifeMechanic()
        {
            ControllersManager.Instance.SceneController.HealthPoints.IncreaseHP();
        }
    }
}
using UnityEngine;

namespace Counters
{
    public class HealthPointsCounter
    {
        public const int MaxHp = 6;

        private int _healthPointsCount;

        public int HealthPointsNumber
        {
            get => _healthPointsCount;
            set
            {
                if (value >= 0 && value <= MaxHp)
                    _healthPointsCount = value;
            }
        }

        public HealthPointsCounter(int startHp)
        {
            ReInit(startHp);
        }

        public void ReInit(int startHp)
        {
            HealthPointsNumber = startHp;
        }

        public bool CheckIfMaxHpReached() => HealthPointsNumber >= MaxHp;
    }
}
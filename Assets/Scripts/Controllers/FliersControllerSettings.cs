using System;
using System.Linq;
using Models;
using UnityEngine;

namespace Controllers
{
    public enum SpawnAreaTypes { Left, Right, Bottom}
    public class FliersControllerSettings : ComponentSingleton<FliersControllerSettings>
    {
        private const float MaxBombsProbability = 0.5f;
        private const float MaxLifesProbability = 0.25f;
        
        [Serializable]
        private struct Range
        {
            [Range(0,1)] [SerializeField] private float from, to;
            public (float from, float to) Value => (from, to);

            public Range(float from, float to)
            {
                this.from = from;
                this.to = to;
            }

            public void AlignSpawnDelay() => to = from;
        }
        
        [Serializable]
        public struct SpawnZoneSettings
        {
            [Range(0,1)] [SerializeField] private float minPosRatioToSpawnSide;
            [Range(0,1)] [SerializeField] private float maxPosRatioToSpawnSide;
            
            [Range(0,360)] [SerializeField] private int angle;
            [Range(0,180)] [SerializeField] private int angleDeviation;
            
            [Range(0,1)] [SerializeField] private float spawnProbability;
            [SerializeField] private SpawnAreaTypes spawnAreaType;
            
            public float MinPosRatioToSpawnSide => minPosRatioToSpawnSide;
            public float MaxPosRatioToSpawnSide => maxPosRatioToSpawnSide;
            public int Angle => angle;
            public int AngleDeviation => angleDeviation;
            public float SpawnProbability => spawnProbability;
            
            public SpawnAreaTypes SpawnAreaType => spawnAreaType;

            public void AlignMaxPosRatioToSpawnSide() => maxPosRatioToSpawnSide = minPosRatioToSpawnSide;
            public void IncreaseSpawnProbability(float value) => spawnProbability += value;
            public void ResetSpawnProbability() => spawnProbability = 0;
        }

        [Range(0, 500)] [SerializeField] private int flierSpeed = 250;
        [Range(0, 15)] [SerializeField] private float jumpPower = 7.5f;
        [Range(1, 10)] [SerializeField] private int minNumberOfFliers = 4;
        [Range(1, 10)] [SerializeField] private int maxNumberOfFliers = 10;
        [Range(0, 10)] [SerializeField] private float roundDelay = 3;
        [Range(0, 10)] [SerializeField] private int roundsNumber = 5;
        [Range(0f,1)][SerializeField] private float bombsFractionInPack;
        [Range(0f,1)][SerializeField] private float lifesFractionInPack;
        [Range(0f,MaxBombsProbability)][SerializeField] private float bombsProbability;
        [Range(0f,MaxLifesProbability)][SerializeField] private float lifesProbability;
        
        [SerializeField] private Range spawnDelay;
        [SerializeField] private SpawnZoneSettings[] spawnZones;

        public int FlierSpeed => flierSpeed;
        public float JumpPower => jumpPower;
        public int MinNumberOfFliers => minNumberOfFliers;
        public int MaxNumberOfFliers => maxNumberOfFliers;
        public float BombsFractionInPack => bombsFractionInPack;
        public float LifesFractionInPack => lifesFractionInPack;
        public float BombsProbability => bombsProbability;
        public float LifesProbability => lifesProbability;
        public float RoundDelay => roundDelay;
        public int RoundsNumber => roundsNumber;
        public (float from, float to) SpawnDelay => spawnDelay.Value;
        public SpawnZoneSettings[] SpawnZones => spawnZones;
        
        private void OnValidate()
        {
            if (maxNumberOfFliers < minNumberOfFliers)
                maxNumberOfFliers = minNumberOfFliers;
            
            if (spawnDelay.Value.to < spawnDelay.Value.from)
                spawnDelay.AlignSpawnDelay();
            
            var deltaProbability = (1 - spawnZones.Sum(x => x.SpawnProbability)) / spawnZones.Length;
            var rest = 0f;
            
            for (var i = 0; i < spawnZones.Length; i++)
            {
                if (spawnZones[i].MaxPosRatioToSpawnSide < spawnZones[i].MinPosRatioToSpawnSide)
                    spawnZones[i].AlignMaxPosRatioToSpawnSide();

                if (deltaProbability != 0)
                {
                    spawnZones[i].IncreaseSpawnProbability(deltaProbability + rest);

                    if (rest > 0) rest = 0;
                    if (spawnZones[i].SpawnProbability < 0)
                    {
                        rest = spawnZones[i].SpawnProbability;
                        spawnZones[i].ResetSpawnProbability();
                    }
                }
            }
        }
    }
}
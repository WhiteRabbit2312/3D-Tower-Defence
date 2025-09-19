using UnityEngine;

namespace TowerDefense.Data
{
    [System.Serializable]
    public class ProbabilisticEnemy
    {
        public EnemyData EnemyData;

        [Tooltip("The chance for this enemy to be picked for spawning.")] [Range(0f, 100f)]
        public float SpawnChance;
    }
}
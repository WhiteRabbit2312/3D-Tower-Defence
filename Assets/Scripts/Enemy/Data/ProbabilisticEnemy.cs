using TowerDefense.Data;
using UnityEngine;

namespace TowerDefense.Data
{
    [System.Serializable]
    public class ProbabilisticEnemy
    {
        public EnemyData EnemyData;

        [Tooltip("The chance for this enemy to be picked for spawning (e.g., 50 for 50%).")] [Range(0f, 100f)]
        public float SpawnChance;
    }
}
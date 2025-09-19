using TowerDefense.Data;
using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Interfaces
{
    /// <summary>
    /// Interface for the enemy factory.
    /// This allows the WaveManager to be completely decoupled from how enemies are created.
    /// </summary>
    public interface IEnemyFactory
    {
        BaseEnemy Create(EnemyData enemyData, Vector3 position);
    }
}
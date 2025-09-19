using TowerDefense.Data;
using TowerDefense.Enemies;
using UnityEngine;

namespace TowerDefense.Interfaces
{
    /// <summary>
    /// Interface for the enemy factory.
    /// </summary>
    public interface IEnemyFactory
    {
        BaseEnemy Create(EnemyData enemyData, Vector3 position);
    }
}
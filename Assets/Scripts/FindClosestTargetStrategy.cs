using UnityEngine;
using TowerDefense.Interfaces;
using TowerDefense.Managers;
using System.Linq;

namespace TowerDefense.Towers.Targeting
{
    /// <summary>
    /// A concrete implementation of the targeting strategy.
    /// Now highly optimized. It gets a reference to the EnemyManager and
    /// searches a small, managed list of enemies instead of the entire scene.
    /// </summary>
    public class FindClosestTargetStrategy : ITargetingStrategy
    {
        private readonly EnemyManager _enemyManager;
        public FindClosestTargetStrategy(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }

        public ITargetable FindTarget(Vector3 towerPosition, float range)
        {
            ITargetable closestEnemy = null;
            float minDistanceSqr = float.MaxValue;
            
            foreach (var enemy in _enemyManager.ActiveEnemies)
            {
                float distanceSqr = (towerPosition - enemy.Position).sqrMagnitude;
                if (distanceSqr < minDistanceSqr && distanceSqr <= range * range)
                {
                    minDistanceSqr = distanceSqr;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }
    }
}
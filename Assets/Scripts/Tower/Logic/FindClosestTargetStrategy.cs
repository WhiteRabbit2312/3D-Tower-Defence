using UnityEngine;
using TowerDefense.Interfaces;
using TowerDefense.Managers;
using System.Linq;

namespace TowerDefense.Towers.Targeting
{
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
using TowerDefense.Data;
using TowerDefense.Enemies;
using TowerDefense.Interfaces;
using UnityEngine;
using Zenject;

namespace TowerDefense.Factories
{
    /// <summary>
    /// Concrete implementation of the IEnemyFactory.
    /// </summary>
    public class EnemyFactory : IEnemyFactory
    {
        private readonly DiContainer _container;

        public EnemyFactory(DiContainer container)
        {
            _container = container;
        }

        public BaseEnemy Create(EnemyData enemyData, Vector3 position)
        {
            if (enemyData == null || enemyData.EnemyPrefab == null)
            {
                return null;
            }

            BaseEnemy enemyInstance = _container.InstantiatePrefabForComponent<BaseEnemy>(
                enemyData.EnemyPrefab, 
                position, 
                Quaternion.identity, 
                null);

            return enemyInstance;
        }
    }
}
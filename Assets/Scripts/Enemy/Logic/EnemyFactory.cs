using TowerDefense.Data;
using TowerDefense.Enemies;
using TowerDefense.Interfaces;
using UnityEngine;
using Zenject;

namespace TowerDefense.Factories
{
    /// <summary>
    /// Concrete implementation of the IEnemyFactory.
    /// Uses Zenject's DiContainer to instantiate prefabs, which automatically handles
    /// dependency injection for the created enemies.
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
                Debug.LogError("[EnemyFactory] EnemyData or its prefab is null. Cannot create enemy.");
                return null;
            }

            // Using InstantiatePrefabForComponent ensures that Zenject dependencies on the enemy's scripts are resolved.
            BaseEnemy enemyInstance = _container.InstantiatePrefabForComponent<BaseEnemy>(
                enemyData.EnemyPrefab, 
                position, 
                Quaternion.identity, 
                null);

            return enemyInstance;
        }
    }
}
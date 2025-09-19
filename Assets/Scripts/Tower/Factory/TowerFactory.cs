using TowerDefense.Data;
using TowerDefense.Interfaces;
using TowerDefense.Towers;
using UnityEngine;
using Zenject;

namespace TowerDefense.Factories
{
    /// <summary>
    /// A factory for creating towers.
    /// It uses Zenject's DiContainer to properly instantiate tower prefabs
    /// and resolve all their dependencies.
    /// </summary>
    public class TowerFactory : ITowerFactory
    {
        private readonly DiContainer _container;

        public TowerFactory(DiContainer container)
        {
            _container = container;
        }
        
        public BaseTower CreateTower(TowerData towerData, Vector3 position)
        {
            if (towerData == null || towerData.TowerPrefab == null)
            {
                return null;
            }

            BaseTower towerInstance = _container.InstantiatePrefabForComponent<BaseTower>(
                towerData.TowerPrefab,
                position,
                Quaternion.identity,
                null);

            return towerInstance;
        }
    }
}
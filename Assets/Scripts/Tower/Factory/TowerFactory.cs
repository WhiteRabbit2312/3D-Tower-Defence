using TowerDefense.Data;
using TowerDefense.Interfaces;
using TowerDefense.Towers;
using UnityEngine;
using Zenject;

namespace TowerDefense.Factories
{
    /// <summary>
    /// A Plain Old C# Object (POCO) factory for creating towers.
    /// It uses Zenject's DiContainer to properly instantiate tower prefabs
    /// and resolve all their dependencies.
    /// </summary>
    public class TowerFactory : ITowerFactory
    {
        private readonly DiContainer _container;

        // The container is injected by Zenject when the factory itself is created.
        public TowerFactory(DiContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates a new tower instance from the provided data at the given position.
        /// </summary>
        /// <param name="towerData">The ScriptableObject defining the tower to create.</param>
        /// <param name="position">The world position to spawn the tower at.</param>
        /// <returns>The created BaseTower component, or null if creation failed.</returns>
        public BaseTower CreateTower(TowerData towerData, Vector3 position)
        {
            if (towerData == null || towerData.TowerPrefab == null)
            {
                Debug.LogError("[TowerFactory] TowerData or its prefab is null. Cannot create tower.");
                return null;
            }

            // Use InstantiatePrefabForComponent to create the tower.
            // Zenject will handle dependency injection on the tower's scripts automatically.
            BaseTower towerInstance = _container.InstantiatePrefabForComponent<BaseTower>(
                towerData.TowerPrefab,
                position,
                Quaternion.identity,
                null); // Parent can be set here if needed

            // Here you would typically initialize the tower with stats from TowerData
            // e.g., towerInstance.Setup(towerData);
                
            return towerInstance;
        }
    }
}
using TowerDefense.Data;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Interfaces
{
    /// <summary>
    /// Interface for the tower factory.
    /// This decouples any tower-building logic (like a UI build manager)
    /// from the concrete implementation of how towers are created.
    /// </summary>
    public interface ITowerFactory
    {
        BaseTower CreateTower(TowerData towerData, Vector3 position);
    }
}
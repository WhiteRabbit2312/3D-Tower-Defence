using TowerDefense.Data;
using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Interfaces
{
    /// <summary>
    /// Interface for the tower factory.
    /// </summary>
    public interface ITowerFactory
    {
        BaseTower CreateTower(TowerData towerData, Vector3 position);
    }
}
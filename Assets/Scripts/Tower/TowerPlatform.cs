using TowerDefense.Data;
using TowerDefense.Factories;
using TowerDefense.Interfaces;
using TowerDefense.Managers;
using UnityEngine;
using Zenject;

namespace TowerDefense.Towers
{
    /// <summary>
    /// A clickable platform where a player can build a tower.
    /// </summary>
    public class TowerPlatform : MonoBehaviour
    {
        public BaseTower PlacedTower { get; private set; }
        public bool IsOccupied => PlacedTower != null;
        
        /// <summary>
        /// Assigns a tower to this platform, marking it as occupied.
        /// </summary>
        public void SetPlacedTower(BaseTower tower)
        {
            PlacedTower = tower;
        }

        /// <summary>
        /// Called by BuildManager when the player clicks on an occupied platform.
        /// </summary>
        public void TryUpgradeTower()
        {
            if (IsOccupied)
            {
                PlacedTower.Upgrade();
            }
        }
        
        public void ClearPlacedTower()
        {
            PlacedTower = null;
        }
    }
}
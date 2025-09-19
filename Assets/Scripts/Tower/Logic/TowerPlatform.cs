using UnityEngine;

namespace TowerDefense.Towers
{
    /// <summary>
    /// A clickable platform where a player can build a tower.
    /// </summary>
    public class TowerPlatform : MonoBehaviour
    {
        public BaseTower PlacedTower { get; private set; }
        public bool IsOccupied => PlacedTower != null;
        
        public void SetPlacedTower(BaseTower tower)
        {
            PlacedTower = tower;
        }
        
        public void ClearPlacedTower()
        {
            PlacedTower = null;
        }
    }
}
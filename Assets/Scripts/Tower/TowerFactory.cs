using UnityEngine;
using TowerDefense.Towers;
using UnityEngine.Serialization; // Assuming your tower prefabs are in this namespace

namespace TowerDefense.Core
{
    /// <summary>
    /// Factory pattern implementation for creating towers.
    /// This decouples the game logic from the concrete tower prefabs.
    /// You would typically load prefabs from Resources or an Addressables system here.
    /// </summary>
    public class TowerFactory : MonoBehaviour
    {
        [SerializeField] private BaseTower _machineGunTowerPrefab;
        [SerializeField] private BaseTower _slowingTowerPrefab;
        
        public enum TowerType
        {
            MachineGun,
            Slowing
        }

        public BaseTower CreateTower(TowerType type, Vector3 position)
        {
            BaseTower prefab = GetPrefab(type);
            if (prefab == null)
            {
                Debug.LogError($"Prefab for tower type {type} not found!");
                return null;
            }
            
            return Instantiate(prefab, position, Quaternion.identity);
        }

        private BaseTower GetPrefab(TowerType type)
        {
            switch (type)
            {
                case TowerType.MachineGun:
                    return _machineGunTowerPrefab;
                case TowerType.Slowing:
                    return _slowingTowerPrefab;
                default:
                    return null;
            }
        }
    }
}
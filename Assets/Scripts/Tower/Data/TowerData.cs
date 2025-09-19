using TowerDefense.Towers;
using UnityEngine;

namespace TowerDefense.Data
{
    /// <summary>
    /// Base abstract class for all tower configuration ScriptableObjects.
    /// It defines the common data and the contract for how stats are retrieved.
    /// </summary>
    public abstract class TowerData : ScriptableObject
    {
        [Header("General")]
        [Tooltip("The prefab for this tower, containing the model and a BaseTower-derived component.")]
        public BaseTower TowerPrefab;

        [Tooltip("Cost to build the tower at level 1.")]
        public int BuildCost = 100;

        // Abstract methods to get stats for a given level, now calculated procedurally.
        public abstract int GetUpgradeCost(int toLevel);
        public abstract float GetDamage(int level);
        public abstract float GetRange(int level);
        public abstract float GetFireRate(int level);
        public abstract float GetSlowMultiplier(int level);
        public abstract float GetSlowDuration(int level);
    }
}
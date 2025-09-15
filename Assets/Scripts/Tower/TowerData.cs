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

        // Abstract property to get the total number of defined upgrade levels.
        public abstract int GetMaxLevel();

        // Abstract methods to get specific stats for a given level.
        // Each child class will implement these based on its own upgrade structure.
        public abstract int GetUpgradeCost(int level);
        public abstract float GetDamage(int level);
        public abstract float GetRange(int level);
        public abstract float GetFireRate(int level);
        public abstract float GetSlowMultiplier(int level);
        public abstract float GetSlowDuration(int level);
    }
}
using UnityEngine;
using System;
using System.Collections.Generic;

namespace TowerDefense.Data
{
    /// <summary>
    /// A concrete TowerData for towers that apply a slowing effect.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSlowingTowerData", menuName = "Tower Defense/Tower Data/Slowing Tower")]
    public class SlowingTowerData : TowerData
    {
        // This struct only contains fields relevant to a slowing tower.
        [Serializable]
        public struct SlowUpgradeLevel
        {
            [Tooltip("Cost to upgrade to this level.")]
            public int Cost;
            [Tooltip("Attack range in Unity units for this level.")]
            public float Range;
            [Tooltip("Time in seconds between shots for this level.")]
            public float FireRate;
            [Tooltip("The speed multiplier (e.g., 0.5 for 50% slow). 1 means no slow.")]
            [Range(0f, 1f)] public float SlowMultiplier;
            [Tooltip("How long the slow effect lasts in seconds.")]
            public float SlowDuration;
        }

        [Header("Slowing Tower Upgrades")]
        public List<SlowUpgradeLevel> UpgradeLevels = new List<SlowUpgradeLevel>();

        // Implementation of the abstract contract from TowerData
        public override int GetMaxLevel() => UpgradeLevels.Count;
        public override int GetUpgradeCost(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].Cost : int.MaxValue;
        public override float GetRange(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].Range : 0;
        public override float GetFireRate(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].FireRate : 0;
        public override float GetSlowMultiplier(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].SlowMultiplier : 1f;
        public override float GetSlowDuration(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].SlowDuration : 0f;

        // This tower doesn't deal damage, so we return a default value.
        public override float GetDamage(int level) => 0f;
    }
}

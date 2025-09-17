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
        [SerializeField] private float _baseRange = 12f;
        [SerializeField] private float _baseFireRate = 1.5f; // Time between shots
        [SerializeField, Range(0f, 1f)] private float _baseSlowMultiplier = 0.7f; // 30% slow
        [SerializeField] private float _baseSlowDuration = 2f;

        [Header("Infinite Upgrade Scaling")]
        [SerializeField] private int _baseUpgradeCost = 75;
        [Tooltip("Each level costs this much more than the last (e.g., 1.3 = +30% cost per level).")]
        [SerializeField] private float _costGrowthFactor = 1.3f;
        [SerializeField] private float _rangeIncreasePerLevel = 1.5f;
        [Tooltip("How much the slow effect improves per level (closer to 0).")]
        [SerializeField] private float _slowEffectIncreasePerLevel = 0.02f;
        [Tooltip("How much the slow duration increases per level.")]
        [SerializeField] private float _slowDurationIncreasePerLevel = 0.1f;


        // --- Method Implementations ---

        public override int GetUpgradeCost(int toLevel)
        {
            // Cost grows exponentially
            return (int)(_baseUpgradeCost * Mathf.Pow(_costGrowthFactor, toLevel - 1));
        }

        public override float GetRange(int level)
        {
            // Range grows linearly
            return _baseRange + (_rangeIncreasePerLevel * level);
        }

        public override float GetFireRate(int level)
        {
            // For this tower, fire rate does not change with upgrades.
            return _baseFireRate;
        }

        public override float GetSlowMultiplier(int level)
        {
            // The slow multiplier decreases (gets stronger) with each level.
            // We use Mathf.Max to ensure it doesn't become too strong (e.g., less than 10%).
            return Mathf.Max(0.1f, _baseSlowMultiplier - (_slowEffectIncreasePerLevel * level));
        }

        public override float GetSlowDuration(int level)
        {
            // Slow duration increases linearly
            return _baseSlowDuration + (_slowDurationIncreasePerLevel * level);
        }

        // This tower type does not deal damage, so it returns a neutral value.
        public override float GetDamage(int level) => 0f;
    }
}

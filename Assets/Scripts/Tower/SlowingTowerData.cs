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
        [Header("Base Stats")]
        [SerializeField] private float _baseDamage = 5f; // Now has base damage
        [SerializeField] private float _baseRange = 12f;
        [SerializeField] private float _baseFireRate = 1.5f;
        [SerializeField, Range(0f, 1f)] private float _baseSlowMultiplier = 0.7f; // 30% slow
        [SerializeField] private float _baseSlowDuration = 2f;

        [Header("Infinite Upgrade Scaling")]
        [SerializeField] private int _baseUpgradeCost = 75;
        [SerializeField] private float _costGrowthFactor = 1.3f;
        [SerializeField] private float _damageIncreasePerLevel = 2f; // Damage now scales
        [Tooltip("The strongest the slow can be (e.g., 0.2 for 80% slow max).")]
        [SerializeField, Range(0f, 1f)] private float _maxSlowMultiplier = 0.2f;
        [Tooltip("How much the slow effect improves per level (closer to 0).")]
        [SerializeField] private float _slowEffectIncreasePerLevel = 0.02f;

        // --- Method Implementations ---

        public override int GetUpgradeCost(int toLevel)
        {
            return (int)(_baseUpgradeCost * Mathf.Pow(_costGrowthFactor, toLevel - 1));
        }
        
        // This tower now deals damage.
        public override float GetDamage(int level)
        {
            return _baseDamage + (_damageIncreasePerLevel * level);
        }

        // Range does not scale for this tower type according to requirements.
        public override float GetRange(int level)
        {
            return _baseRange;
        }

        public override float GetFireRate(int level)
        {
            return _baseFireRate;
        }

        public override float GetSlowMultiplier(int level)
        {
            // The slow multiplier decreases (gets stronger) but is capped by _maxSlowMultiplier.
            return Mathf.Max(_maxSlowMultiplier, _baseSlowMultiplier - (_slowEffectIncreasePerLevel * level));
        }

        public override float GetSlowDuration(int level)
        {
            return _baseSlowDuration; // Duration does not scale
        }
    }
}

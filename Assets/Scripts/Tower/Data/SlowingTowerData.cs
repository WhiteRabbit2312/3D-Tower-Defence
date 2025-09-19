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
        [SerializeField] private float _baseDamage = 10f;
        [SerializeField] private float _baseRange = 12f;
        [SerializeField] private float _baseFireRate = 1.5f;
        [SerializeField, Range(0.1f, 1f)] private float _baseSlowMultiplier = 0.7f;
        [SerializeField] private float _baseSlowDuration = 2f;

        [Header("Upgrade Scaling")]
        [SerializeField] private int _baseUpgradeCost = 100;
        [SerializeField] private float _costGrowthFactor = 1.3f;

        [Header("Damage Upgrade (Infinite)")]
        [SerializeField] private float _damageIncreasePerLevel = 5f;

        [Header("Slow Effect Upgrade (Limited)")]
        [Tooltip("The strongest the slow can become.")]
        [SerializeField, Range(0.1f, 1f)] private float _maxSlowMultiplier = 0.3f;
        [Tooltip("How much the slow multiplier decreases each level.")]
        [SerializeField] private float _slowEffectIncreasePerLevel = 0.05f;

        public override int GetUpgradeCost(int toLevel)
        {
            return (int)(_baseUpgradeCost * Mathf.Pow(_costGrowthFactor, toLevel - 1));
        }

        public override float GetDamage(int level)
        {
            return _baseDamage + (_damageIncreasePerLevel * level);
        }

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
            float potentialSlow = _baseSlowMultiplier - (_slowEffectIncreasePerLevel * level);
            return Mathf.Max(_maxSlowMultiplier, potentialSlow);
        }

        public override float GetSlowDuration(int level)
        {
            return _baseSlowDuration;
        }
    }
}

using UnityEngine;
using System;
using System.Collections.Generic;

namespace TowerDefense.Data
{
    /// <summary>
    /// A concrete TowerData for towers that deal direct damage.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMachineGunTowerData", menuName = "Tower Defense/Tower Data/Machine Gun Tower")]
    public class MachineGunTowerData : TowerData
    {
        [Header("Base Stats")]
        [SerializeField] private float _baseDamage = 25f;
        [SerializeField] private float _baseRange = 15f;
        [SerializeField] private float _baseFireRate = 1f;

        [Header("Upgrade Scaling")]
        [SerializeField] private int _baseUpgradeCost = 60;
        [SerializeField] private float _costGrowthFactor = 1.3f;
        
        [Header("Damage Upgrade (Infinite)")]
        [SerializeField] private float _damageIncreasePerLevel = 15f;
        
        [Header("Range Upgrade (Limited)")]
        [Tooltip("The level at which range upgrades will stop.")]
        [SerializeField] private int _maxRangeLevel = 5;
        [SerializeField] private float _rangeIncreasePerLevel = 2f;
        

        // --- Method Implementations ---

        public override int GetUpgradeCost(int toLevel)
        {
            return (int)(_baseUpgradeCost * Mathf.Pow(_costGrowthFactor, toLevel - 1));
        }

        public override float GetDamage(int level)
        {
            // Damage grows infinitely
            return _baseDamage + (_damageIncreasePerLevel * level);
        }

        public override float GetRange(int level)
        {
            // The effective level for range is capped at MaxRangeLevel
            int effectiveLevel = Mathf.Min(level, _maxRangeLevel);
            return _baseRange + (_rangeIncreasePerLevel * effectiveLevel);
        }

        public override float GetFireRate(int level)
        {
            // For simplicity, fire rate is constant in this version.
            return _baseFireRate;
        }

        // This tower type does not slow enemies, so it returns neutral values.
        public override float GetSlowMultiplier(int level) => 1f;
        public override float GetSlowDuration(int level) => 0f;
    }
}
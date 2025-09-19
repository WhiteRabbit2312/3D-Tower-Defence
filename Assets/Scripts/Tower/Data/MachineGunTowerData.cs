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
        [SerializeField] private float _baseDamage = 10f;
        [SerializeField] private float _baseRange = 15f;
        [SerializeField] private float _baseFireRate = 1f; // Time between shots

        [Header("Infinite Upgrade Scaling")]
        [SerializeField] private int _baseUpgradeCost = 50;
        [Tooltip("Each level costs this much more than the last (e.g., 1.2 = +20% cost per level).")]
        [SerializeField] private float _costGrowthFactor = 1.2f;
        [SerializeField] private float _damageIncreasePerLevel = 5f;
        [SerializeField] private float _rangeIncreasePerLevel = 1f;
        [Tooltip("How much the time between shots decreases per level.")]
        [SerializeField] private float _fireRateDecreasePerLevel = 0.05f;

        // --- Method Implementations ---

        public override int GetUpgradeCost(int toLevel)
        {
            // Cost grows exponentially based on the level
            return (int)(_baseUpgradeCost * Mathf.Pow(_costGrowthFactor, toLevel - 1));
        }

        public override float GetDamage(int level)
        {
            // Damage grows linearly
            return _baseDamage + (_damageIncreasePerLevel * level);
        }

        public override float GetRange(int level)
        {
            // Range grows linearly
            return _baseRange + (_rangeIncreasePerLevel * level);
        }

        public override float GetFireRate(int level)
        {
            // Fire rate (time between shots) decreases, making the tower faster.
            // We use Mathf.Max to ensure it never drops below a minimum value.
            return Mathf.Max(0.1f, _baseFireRate - (_fireRateDecreasePerLevel * level));
        }

        // This tower type does not slow enemies, so it returns neutral values.
        public override float GetSlowMultiplier(int level) => 1f;
        public override float GetSlowDuration(int level) => 0f;
    }
}
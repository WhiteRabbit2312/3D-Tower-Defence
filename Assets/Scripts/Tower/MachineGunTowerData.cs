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
        // This struct now only contains fields relevant to a damage tower.
        [Serializable]
        public struct DamageUpgradeLevel
        {
            [Tooltip("Cost to upgrade to this level.")]
            public int Cost;
            [Tooltip("Damage per shot for this level.")]
            public float Damage;
            [Tooltip("Attack range in Unity units for this level.")]
            public float Range;
            [Tooltip("Time in seconds between shots for this level.")]
            public float FireRate;
        }

        [Header("Machine Gun Upgrades")]
        public List<DamageUpgradeLevel> UpgradeLevels = new List<DamageUpgradeLevel>();

        // Implementation of the abstract contract from TowerData
        public override int GetMaxLevel() => UpgradeLevels.Count;
        public override int GetUpgradeCost(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].Cost : int.MaxValue;
        public override float GetDamage(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].Damage : 0;
        public override float GetRange(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].Range : 0;
        public override float GetFireRate(int level) => (level < UpgradeLevels.Count) ? UpgradeLevels[level].FireRate : 0;

        // This tower doesn't slow, so we return default "no-effect" values.
        public override float GetSlowMultiplier(int level) => 1f;
        public override float GetSlowDuration(int level) => 0f;
    }
}
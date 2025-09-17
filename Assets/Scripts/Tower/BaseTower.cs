using System.Linq;
using TowerDefense.Data;
using TowerDefense.Enemies;
using TowerDefense.Interfaces;
using TowerDefense.Managers;
using TowerDefense.Towers.Targeting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace TowerDefense.Towers
{
    /// <summary>
    /// Abstract base class for all tower types.
    /// Handles target acquisition and firing logic.
    /// Uses the Strategy pattern for targeting.
    /// </summary>
    public abstract class BaseTower : MonoBehaviour
    {
        [Header("Core References")]
        [Tooltip("The part of the tower that rotates to face the target.")]
        [SerializeField] protected Transform TurretHead;
        [Tooltip("The point from which projectiles are fired.")]
        [SerializeField] protected Transform MuzzlePoint;
        [Header("Projectile Settings")]
        [Tooltip("The speed at which projectiles from this tower travel.")]
        [SerializeField] protected float ProjectileSpeed = 50f;

        // Public property to hold tower-specific data, assigned by BuildManager
        public TowerData TowerData { get; private set; }
        public TowerPlatform Platform { get; private set; }
        public int CurrentLevel { get; private set; } = 0;

        // Protected fields for subclasses, capitalized as per convention
        protected ITargetable CurrentTarget;
        protected EnemyManager EnemyManager;
        protected EconomyManager EconomyManager;
        protected float FireCooldown = 0f;
        
        // Current stats, derived from TowerData and current level
        protected float CurrentDamage;
        protected float CurrentRange;
        protected float CurrentFireRate;
        protected float CurrentSlowMultiplier;
        protected float CurrentSlowDuration;

        public TargetingPriority CurrentPriority { get; set; } = TargetingPriority.Closest;

        [Inject]
        public void Construct(EnemyManager enemyManager, EconomyManager economyManager)
        {
            EnemyManager = enemyManager;
            EconomyManager = economyManager;
        }

        /// <summary>
        /// Called by the BuildManager right after the tower is placed.
        /// </summary>
        public void Initialize(TowerData data)
        {
            TowerData = data;
            ApplyUpgrade(0); // Set initial stats for level 0
        }

        protected virtual void Update()
        {
            FireCooldown -= Time.deltaTime;

            if (!IsTargetValid())
            {
                FindTarget();
            }
            
            if (CurrentTarget != null)
            {
                RotateTurret();
                if (FireCooldown <= 0f)
                {
                    Fire();
                    FireCooldown = CurrentFireRate;
                }
            }
        }
        
        protected void FindTarget()
        {
            var potentialTargets = EnemyManager.ActiveEnemies
                .Where(e => Vector3.Distance(transform.position, e.Position) <= CurrentRange)
                .ToList();

            if (!potentialTargets.Any())
            {
                CurrentTarget = null;
                return;
            }

            switch (CurrentPriority)
            {
                case TargetingPriority.Closest:
                    CurrentTarget = potentialTargets
                        .OrderBy(e => Vector3.Distance(transform.position, e.Position))
                        .FirstOrDefault();
                    break;
                case TargetingPriority.Weakest:
                    CurrentTarget = potentialTargets
                        .OrderBy(e => (e as BaseEnemy)?.CurrentHealth ?? float.MaxValue)
                        .FirstOrDefault();
                    break;
            }
        }

        protected void RotateTurret()
        {
            if (TurretHead == null || CurrentTarget == null) return;
            
            Vector3 direction = CurrentTarget.Position - TurretHead.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(TurretHead.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            TurretHead.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        protected bool IsTargetValid()
        {
            if (CurrentTarget == null || !CurrentTarget.IsAlive) return false;
            if (Vector3.Distance(transform.position, CurrentTarget.Position) > CurrentRange) return false;
            return true;
        }
        
        public void Upgrade()
        {
            int nextLevel = CurrentLevel + 1;
            if (nextLevel >= TowerData.GetMaxLevel())
            {
                Debug.Log("Tower is at max level.");
                return;
            }

            int upgradeCost = TowerData.GetUpgradeCost(nextLevel);
            if (EconomyManager.TrySpendCurrency(upgradeCost))
            {
                ApplyUpgrade(nextLevel);
            }
            else
            {
                Debug.Log("Not enough currency to upgrade.");
            }
        }
        
        public int GetTotalInvestedCost()
        {
            int totalCost = TowerData.BuildCost;
            // Sums up the cost of all upgrades up to the current level
            for (int i = 1; i <= CurrentLevel; i++)
            {
                totalCost += TowerData.GetUpgradeCost(i);
            }
            return totalCost;
        }
        
        protected void ApplyUpgrade(int level)
        {
            CurrentLevel = level;
            CurrentDamage = TowerData.GetDamage(level);
            CurrentRange = TowerData.GetRange(level);
            CurrentFireRate = TowerData.GetFireRate(level);
            CurrentSlowMultiplier = TowerData.GetSlowMultiplier(level);
            CurrentSlowDuration = TowerData.GetSlowDuration(level);
        }
        
        /// <summary>
        /// Abstract method for firing. Each subclass MUST implement this
        /// to define its specific firing behavior (e.g., create a projectile).
        /// </summary>
        protected abstract void Fire();
        
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, CurrentRange);
        }
    }
}

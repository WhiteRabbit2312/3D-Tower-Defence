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

        public TowerData TowerData { get; private set; }
        public TowerPlatform Platform { get; private set; }
        public int CurrentLevel { get; private set; } = 0;

        protected ITargetable CurrentTarget;
        protected EnemyManager EnemyManager;
        protected EconomyManager EconomyManager;
        protected float FireCooldown = 0f;
        
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

        public void Initialize(TowerData data, TowerPlatform platform)
        {
            TowerData = data;
            Platform = platform;
            ApplyUpgrade(0); 
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
        
        public void CycleTargetingPriority()
        {
            int nextPriority = ((int)CurrentPriority + 1) % System.Enum.GetValues(typeof(TargetingPriority)).Length;
            CurrentPriority = (TargetingPriority)nextPriority;
            
            // Force the tower to find a new target based on the new priority
            FindTarget();
            Debug.Log($"Tower targeting priority switched to: {CurrentPriority}");
        }
        
        protected void FindTarget()
        {
            var potentialTargets = EnemyManager.ActiveEnemies
                .Where(e => e != null && e.IsAlive && Vector3.Distance(transform.position, e.Position) <= CurrentRange)
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
            if (CurrentTarget == null || (CurrentTarget as UnityEngine.Object) == null || !CurrentTarget.IsAlive)
            {
                return false;
            }

            if (Vector3.Distance(transform.position, CurrentTarget.Position) > CurrentRange)
            {
                return false;
            }
            
            return true;
        }
        
        public void Upgrade()
        {
            int nextLevel = CurrentLevel + 1;
            
            // The check for max level is now removed to support infinite upgrades.

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
        
        protected abstract void Fire();
        
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, CurrentRange);
        }
    }
}

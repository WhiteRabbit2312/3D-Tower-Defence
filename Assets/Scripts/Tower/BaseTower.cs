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
        [Header("Configuration")]
        [SerializeField] protected TowerData TowerData;
        [SerializeField] protected Transform TurretHead;

        protected ITargetable CurrentTarget;
        protected EnemyManager EnemyManager;
        protected EconomyManager EconomyManager;

        protected int CurrentLevel = 0;
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

        protected virtual void Start()
        {
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
        
        protected void ApplyUpgrade(int level)
        {
            CurrentLevel = level;
            // The tower doesn't care about the type of TowerData, it just gets the stats.
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
            Gizmos.DrawWireSphere(transform.position, TowerData != null ? TowerData.GetRange(CurrentLevel) : 10f);
        }
    }
}

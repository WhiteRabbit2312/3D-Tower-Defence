using TowerDefense.Interfaces;
using TowerDefense.Managers;
using TowerDefense.Towers.Targeting;
using UnityEngine;
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
        [Header("Tower Stats")]
        [SerializeField] protected float range = 10f;
        [SerializeField] protected float fireRate = 1f;

        protected ITargetable currentTarget;
        private float fireCountdown = 0f;
        
        protected ITargetingStrategy targetingStrategy;
        private EnemyManager _enemyManager;

        [Inject]
        public void Construct(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }

        protected virtual void Start()
        {
            // Default targeting strategy now receives the enemy manager for efficient lookups.
            SetTargetingStrategy(new FindClosestTargetStrategy(_enemyManager));
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
        }

        protected virtual void Update()
        {
            if (currentTarget == null || !currentTarget.IsAlive)
            {
                currentTarget = null;
                return;
            }

            AimAtTarget();
            
            if (fireCountdown <= 0f)
            {
                Fire();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }

        public void SetTargetingStrategy(ITargetingStrategy newStrategy)
        {
            targetingStrategy = newStrategy;
        }

        protected void UpdateTarget()
        {
            if (targetingStrategy != null)
            {
                currentTarget = targetingStrategy.FindTarget(transform.position, range);
            }
        }

        protected abstract void AimAtTarget();
        protected abstract void Fire();
        
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}

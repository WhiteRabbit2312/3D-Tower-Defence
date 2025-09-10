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
        [Header("Tower Stats")]
        [SerializeField] protected float _range = 10f;
        [SerializeField] protected float _fireRate = 1f;

        protected ITargetable CurrentTarget;
        private float _fireCountdown = 0f;
        
        protected ITargetingStrategy TargetingStrategy;
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
            if (CurrentTarget == null || !CurrentTarget.IsAlive)
            {
                CurrentTarget = null;
                return;
            }

            AimAtTarget();
            
            if (_fireCountdown <= 0f)
            {
                Fire();
                _fireCountdown = 1f / _fireRate;
            }

            _fireCountdown -= Time.deltaTime;
        }

        public void SetTargetingStrategy(ITargetingStrategy newStrategy)
        {
            TargetingStrategy = newStrategy;
        }

        protected void UpdateTarget()
        {
            if (TargetingStrategy != null)
            {
                CurrentTarget = TargetingStrategy.FindTarget(transform.position, _range);
            }
        }

        protected abstract void AimAtTarget();
        protected abstract void Fire();
        
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}

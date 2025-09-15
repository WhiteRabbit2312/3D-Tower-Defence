using TowerDefense.Data;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Abstract base class for all enemy types.
    /// Handles health, movement via NavMeshAgent, and death.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshAgent))] // MOVED from EnemyMovement
    public abstract class BaseEnemy : MonoBehaviour, ITargetable
    {
        public int CurrencyValue { get; private set; }
        
        public Vector3 Position => transform.position;
        public bool IsAlive => _currentHealth > 0;
        
        protected float _maxHealth;
        protected float _currentHealth;
        protected EnemyData _enemyData;
        protected NavMeshAgent _agent; // ADDED

        private SignalBus _signalBus;
        private Transform _target; // ADDED
        
        [Inject]
        public void Construct(SignalBus signalBus, [Inject(Id = "PathTarget")] Transform target)
        {
            _signalBus = signalBus;
            _target = target;
        }

        protected virtual void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        protected virtual void Update()
        {
            // Check if the agent is active, has a path, and the remaining distance is less than a small threshold.
            if (_agent.hasPath && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                ReachEnd();
                // To prevent this from firing multiple times, we disable the component.
                this.enabled = false; 
            }
        }

        public virtual void Setup(EnemyData data, float healthMultiplier)
        {
            _enemyData = data;

            _maxHealth = _enemyData.BaseHealth * healthMultiplier;
            _currentHealth = _maxHealth;
            CurrencyValue = _enemyData.CurrencyValue;
            
            // Configure and start the agent's movement
            if (_target != null)
            {
                _agent.speed = _enemyData.BaseMoveSpeed;
                _agent.SetDestination(_target.position);
            }
            else
            {
                Debug.LogError("Path Target is not injected! Enemy cannot move.", this);
            }
            
            _signalBus.Fire(new EnemySpawnedSignal(this));
        }

        public virtual void TakeDamage(float amount)
        {
            if (!IsAlive) return;

            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            // Prevent agent from moving after death
            if (_agent.hasPath)
            {
                _agent.isStopped = true;
            }
            _signalBus.Fire(new EnemyDiedSignal(this));
            Destroy(gameObject);
        }

        private void ReachEnd()
        {
            _signalBus.Fire(new EnemyReachedEndSignal(this));
            Destroy(gameObject);
        }
        
        public Transform GetTransform() => transform;
    }
}
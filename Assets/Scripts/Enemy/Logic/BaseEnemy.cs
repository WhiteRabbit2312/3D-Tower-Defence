using System.Collections;
using TowerDefense.Core;
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
    /// Manages core enemy logic including health, movement via NavMeshAgent,
    /// taking damage, and applying status effects.
    /// It communicates with other game systems via signals.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseEnemy : MonoBehaviour, ITargetable, IEffectable 
    {
        public int CurrencyValue { get; private set; }
        public float CurrentHealth => _currentHealth;
        public Vector3 Position => transform.position;
        public bool IsAlive => _currentHealth > 0 && !_isDying;

        protected float _maxHealth;
        protected float _currentHealth;
        protected EnemyData _enemyData;

        private SignalBus _signalBus;
        protected NavMeshAgent _agent;
        private Transform _target;
        
        private Coroutine _speedModifierCoroutine;
        private bool _isDying = false; 

        [Inject]
        public void Construct(SignalBus signalBus, [Inject(Id = Constants.PathTargetKey)] Transform target)
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
            if (IsAlive && _agent.isOnNavMesh && _agent.hasPath && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                ReachEnd();
            }
        }

        public virtual void Setup(EnemyData data, float healthMultiplier)
        {
            _enemyData = data;
            _maxHealth = _enemyData.BaseHealth * healthMultiplier;
            _currentHealth = _maxHealth;
            CurrencyValue = _enemyData.CurrencyValue;

            _agent.speed = _enemyData.BaseMoveSpeed;
            if (_target != null)
            {
                _agent.SetDestination(_target.position);
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
        
        public void ApplySpeedModifier(float multiplier, float duration)
        {
            if (!IsAlive) return;
            
            if (_speedModifierCoroutine != null)
            {
                StopCoroutine(_speedModifierCoroutine);
            }
            _speedModifierCoroutine = StartCoroutine(SpeedModifierRoutine(multiplier, duration));
        }

        // A coroutine to temporarily change the enemy's speed.
        private IEnumerator SpeedModifierRoutine(float multiplier, float duration)
        {
            float originalSpeed = _enemyData.BaseMoveSpeed;
            _agent.speed = originalSpeed * multiplier;
            yield return new WaitForSeconds(duration);
            if (_agent.isOnNavMesh) 
            {
                _agent.speed = originalSpeed;
            }
            _speedModifierCoroutine = null;
        }
        
        protected virtual void Die()
        {
            if (_isDying) return;
            _isDying = true;
            
            if(_agent.isOnNavMesh) _agent.enabled = false;
            
            _signalBus.Fire(new EnemyDiedSignal(this));
            Destroy(gameObject);
        }

        private void ReachEnd()
        {
            if (_isDying) return;
            _isDying = true;

            _signalBus.Fire(new EnemyReachedEndSignal(this));
            Destroy(gameObject);
        }

        // Required by the ITargetable interface for towers to get a reference to the transform.
        public Transform GetTransform() => transform;
    }
}
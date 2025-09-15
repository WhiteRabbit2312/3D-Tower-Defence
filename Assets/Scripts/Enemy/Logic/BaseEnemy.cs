using System.Collections;
using TowerDefense.Data;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace TowerDefense.Enemies
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseEnemy : MonoBehaviour, ITargetable, IEffectable // Now implements IEffectable
    {
        public int CurrencyValue { get; private set; }
        public float CurrentHealth => _currentHealth;
        public Vector3 Position => transform.position;
        public bool IsAlive => _currentHealth > 0;

        protected float _maxHealth;
        protected float _currentHealth;
        protected EnemyData _enemyData;

        private SignalBus _signalBus;
        protected NavMeshAgent _agent;
        private Transform _target;
        
        private Coroutine _speedModifierCoroutine;

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
            if (IsAlive && _agent.hasPath && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
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
            else
            {
                Debug.LogError("Path Target is null for enemy!", this);
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

        /// <summary>
        /// Applies a speed-altering effect for a specific duration.
        /// </summary>
        public void ApplySpeedModifier(float multiplier, float duration)
        {
            if (_speedModifierCoroutine != null)
            {
                StopCoroutine(_speedModifierCoroutine);
            }
            _speedModifierCoroutine = StartCoroutine(SpeedModifierRoutine(multiplier, duration));
        }

        private IEnumerator SpeedModifierRoutine(float multiplier, float duration)
        {
            float originalSpeed = _enemyData.BaseMoveSpeed;
            _agent.speed = originalSpeed * multiplier;
            yield return new WaitForSeconds(duration);
            _agent.speed = originalSpeed;
            _speedModifierCoroutine = null;
        }
        
        protected virtual void Die()
        {
            if (!IsAlive) return; // Prevent multiple death signals
            _currentHealth = 0; // Ensure IsAlive is false
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
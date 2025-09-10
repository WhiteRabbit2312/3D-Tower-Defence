using UnityEngine;
using System;
using TowerDefense.Interfaces;
using TowerDefense.Signals;
using UnityEngine.Serialization;
using Zenject;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Abstract base class for all enemy types.
    /// Implements ITargetable so towers can target it.
    /// Handles health, movement, and death.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class BaseEnemy : MonoBehaviour, ITargetable
    {
        [Header("Enemy Stats")]
        [SerializeField] protected float MaxHealth = 100f;
        [SerializeField] protected float CurrentHealth;
        [SerializeField] public float MoveSpeed = 2f;
        [SerializeField] public int CurrencyValue = 10;
        
        private SignalBus _signalBus;

        public Vector3 Position => transform.position;
        public bool IsAlive => CurrentHealth > 0;
        public Transform GetTransform() => transform;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected virtual void Start()
        {
            CurrentHealth = MaxHealth;
            // Fire a signal to announce that this enemy has spawned.
            _signalBus.Fire(new EnemySpawnedSignal(this));
        }

        public virtual void TakeDamage(float amount)
        {
            if (!IsAlive) return;
            CurrentHealth -= amount;
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            // Fire a signal to announce that this enemy has died.
            _signalBus.Fire(new EnemyDiedSignal(this));
            Destroy(gameObject);
        }
    }
}
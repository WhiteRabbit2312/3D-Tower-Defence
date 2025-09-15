using TowerDefense.Data;
using UnityEngine;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// An enemy that moves faster but has less health.
    /// </summary>
    public class FastEnemy : BaseEnemy
    {
        [Header("Fast Enemy Settings")]
        [Tooltip("Multiplier for the base movement speed.")]
        [SerializeField] private float _speedMultiplier = 1.75f;
        [Tooltip("Multiplier for the base health. Less than 1 reduces health.")]
        [SerializeField] private float _healthPenaltyMultiplier = 0.7f; // Has 70% of base health

        /// <summary>
        /// Overrides the base setup to apply its unique stat modifications.
        /// </summary>
        public override void Setup(EnemyData data, float healthMultiplier)
        {
            // Call the base Setup method first. This initializes stats and sets the base speed on the NavMeshAgent.
            base.Setup(data, healthMultiplier);
            
            // Now, apply this enemy type's specific modifiers to the already calculated stats.
            _maxHealth *= _healthPenaltyMultiplier;
            _currentHealth = _maxHealth; // Ensure current health matches the new, lower max health.
            
            // The base Setup already set the agent's speed. We now overwrite it with our faster value.
            if (_agent != null)
            {
                _agent.speed = _enemyData.BaseMoveSpeed * _speedMultiplier;
            }
        }
    }
}
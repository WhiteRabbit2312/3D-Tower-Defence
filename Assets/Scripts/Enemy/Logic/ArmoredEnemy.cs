using TowerDefense.Data;
using UnityEngine;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// An enemy that takes reduced damage and is physically larger.
    /// </summary>
    public class ArmoredEnemy : BaseEnemy
    {
        [Header("Armored Enemy Settings")]
        [Tooltip("The multiplier for incoming damage. 0.5 means it takes 50% of the damage.")]
        [SerializeField] private float _damageMultiplier = 0.5f;

        /// <summary>
        /// Overrides the base TakeDamage method to apply the armor reduction.
        /// </summary>
        /// <param name="amount">The initial amount of damage.</param>
        public override void TakeDamage(float amount)
        {
            float damageToTake = amount * _damageMultiplier;
            
            // Call the base method with the modified damage value.
            base.TakeDamage(damageToTake);
        }
    }
}
using TowerDefense.Enemies;
using TowerDefense.Interfaces;

namespace TowerDefense.Towers.Projectiles
{
    /// <summary>
    /// A projectile that both deals damage and applies a slowing effect upon hitting a target.
    /// </summary>
    public class DamageAndSlowProjectile : BaseProjectile
    {
        private float _damage;
        private float _slowMultiplier;
        private float _slowDuration;

        /// <summary>
        ///- Initializes the projectile with all its necessary stats.
        /// </summary>
        public void Initialize(ITargetable target, float speed, float damage, float slowMultiplier, float slowDuration)
        {
            base.Initialize(target, speed);
            _damage = damage;
            _slowMultiplier = slowMultiplier;
            _slowDuration = slowDuration;
        }

        /// <summary>
        ///- Overrides the base hit logic to apply both effects.
        /// </summary>
        protected override void OnHitTarget()
        {
            // Deal damage first by casting to BaseEnemy
            if (Target is BaseEnemy enemy)
            {
                enemy.TakeDamage(_damage);
            }

            // Then apply the slow effect by casting to IEffectable
            if (Target is IEffectable effectable)
            {
                effectable.ApplySpeedModifier(_slowMultiplier, _slowDuration);
            }

            // Destroy the projectile after applying effects
            Destroy(gameObject);
        }
    }
}
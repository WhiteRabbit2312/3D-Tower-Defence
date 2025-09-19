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

        public void Initialize(ITargetable target, float speed, float damage, float slowMultiplier, float slowDuration)
        {
            base.Initialize(target, speed);
            _damage = damage;
            _slowMultiplier = slowMultiplier;
            _slowDuration = slowDuration;
        }

        protected override void OnHitTarget()
        {
            if (Target is BaseEnemy enemy)
            {
                enemy.TakeDamage(_damage);
            }

            if (Target is IEffectable effectable)
            {
                effectable.ApplySpeedModifier(_slowMultiplier, _slowDuration);
            }

            Destroy(gameObject);
        }
    }
}
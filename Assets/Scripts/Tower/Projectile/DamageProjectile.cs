using TowerDefense.Interfaces;

namespace TowerDefense.Towers.Projectiles
{
    /// <summary>
    /// A projectile that deals damage upon hitting a target.
    /// </summary>
    public class DamageProjectile : BaseProjectile
    {
        private float _damage;

        public void Initialize(ITargetable target, float speed, float damage)
        {
            base.Initialize(target, speed);
            _damage = damage;
        }

        protected override void OnHitTarget()
        {
            if (Target is IEffectable effectableTarget)
            {
                effectableTarget.TakeDamage(_damage);
            }
            
            // Here you can spawn an impact effect.
            
            Destroy(gameObject);
        }
    }
}
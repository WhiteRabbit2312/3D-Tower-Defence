using TowerDefense.Interfaces;

namespace TowerDefense.Towers.Projectiles
{
    /// <summary>
    /// A projectile that applies a slowing effect upon hitting a target.
    /// </summary>
    public class SlowingProjectile : BaseProjectile
    {
        private float _slowMultiplier;
        private float _slowDuration;

        public void Initialize(ITargetable target, float speed, float slowMultiplier, float slowDuration)
        {
            base.Initialize(target, speed);
            _slowMultiplier = slowMultiplier;
            _slowDuration = slowDuration;
        }
        
        protected override void OnHitTarget()
        {
            if (Target is IEffectable effectableTarget)
            {
                effectableTarget.ApplySpeedModifier(_slowMultiplier, _slowDuration);
            }

            Destroy(gameObject);
        }
    }
}
using TowerDefense.Interfaces;

namespace TowerDefense.Towers
{
    public class SlowingTower : BaseTower
    {
        protected override void Fire()
        {
            // Cast the target to IEffectable to apply the slow.
            if (CurrentTarget is IEffectable effectable)
            {
                effectable.ApplySpeedModifier(CurrentSlowMultiplier, CurrentSlowDuration);
                // Here you would spawn a slowing projectile effect.
            }
        }
    }
}
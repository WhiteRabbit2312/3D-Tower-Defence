using TowerDefense.Enemies;

namespace TowerDefense.Towers
{
    public class MachineGunTower : BaseTower
    {
        protected override void Fire()
        {
            // The logic to find and validate the target is already in the base class.
            // We just need to apply the damage.
            if (CurrentTarget is BaseEnemy enemy)
            {
                enemy.TakeDamage(CurrentDamage);
                // Here you would also spawn a projectile or a muzzle flash effect.
            }
        }
    }
}
using TowerDefense.Enemies;
using TowerDefense.Towers.Projectiles;

namespace TowerDefense.Towers
{
    public class MachineGunTower : BaseTower
    {
        protected override void Fire()
        {
            DamageProjectile newProjectile = _projectileFactory.Create(_projectilePrefab, MuzzlePoint.position, MuzzlePoint.rotation);
    
            // Ініціалізація створеного снаряда
            newProjectile.Initialize(CurrentTarget, _projectileSpeed, CurrentDamage);

        }
    }
}
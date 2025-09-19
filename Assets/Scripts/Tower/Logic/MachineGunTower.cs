using TowerDefense.Enemies;
using TowerDefense.Interfaces;
using TowerDefense.Towers.Projectiles;
using UnityEngine;
using Zenject;

namespace TowerDefense.Towers
{
    /// <summary>
    /// A concrete tower implementation that fires projectiles dealing damage.
    /// </summary>
    public class MachineGunTower : BaseTower
    {
        [Header("Machine Gun Specifics")]
        [Tooltip("The prefab for the projectile this tower fires.")]
        [SerializeField] private DamageProjectile _projectilePrefab;

        private IProjectileFactory _projectileFactory;

        // Using a constructor injection for the factory.
        [Inject]
        public void Construct(IProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }
        
        /// <summary>
        /// Implements the firing logic for this specific tower.
        /// It creates a damage projectile using the factory.
        /// </summary>
        protected override void Fire()
        {
            if (CurrentTarget == null || MuzzlePoint == null || _projectilePrefab == null) return;
            
            DamageProjectile newProjectile = _projectileFactory.Create(_projectilePrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            newProjectile.Initialize(CurrentTarget, ProjectileSpeed, CurrentDamage);
        }
    }
}
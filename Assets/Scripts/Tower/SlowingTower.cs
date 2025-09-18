using TowerDefense.Interfaces;
using TowerDefense.Towers.Projectiles;
using UnityEngine;
using Zenject;

namespace TowerDefense.Towers
{
    public class SlowingTower : BaseTower
    {
        [Header("Slowing Tower Specifics")]
        [Tooltip("The prefab for the projectile this tower fires (must have a DamageAndSlowProjectile component).")]
        [SerializeField] private DamageAndSlowProjectile _projectilePrefab;
        
        private IProjectileFactory _projectileFactory;

        [Inject]
        public void Construct(IProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        /// <summary>
        /// Implements the firing logic for this specific tower.
        /// It creates a projectile that damages and slows, using the factory.
        /// </summary>
        protected override void Fire()
        {
            if (CurrentTarget == null || MuzzlePoint == null || _projectilePrefab == null) return;
            DamageAndSlowProjectile newProjectile = _projectileFactory.Create(_projectilePrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            newProjectile.Initialize(CurrentTarget, ProjectileSpeed, CurrentDamage, CurrentSlowMultiplier, CurrentSlowDuration);
        }
    }
}
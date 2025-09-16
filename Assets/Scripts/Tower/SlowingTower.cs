using TowerDefense.Interfaces;
using TowerDefense.Towers.Projectiles;
using UnityEngine;
using Zenject;

namespace TowerDefense.Towers
{
    public class SlowingTower : BaseTower
    {
        [Header("Slowing Tower Specifics")]
        [Tooltip("The prefab for the slowing projectile this tower fires.")]
        [SerializeField] private SlowingProjectile _projectilePrefab;
        
        private IProjectileFactory _projectileFactory;

        [Inject]
        public void Construct(IProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;
        }

        /// <summary>
        /// Implements the firing logic for this specific tower.
        /// It creates a slowing projectile using the factory.
        /// </summary>
        protected override void Fire()
        {
            if (CurrentTarget == null || MuzzlePoint == null || _projectilePrefab == null) return;
            
            SlowingProjectile newProjectile = _projectileFactory.Create(_projectilePrefab, MuzzlePoint.position, MuzzlePoint.rotation);

            newProjectile.Initialize(CurrentTarget, ProjectileSpeed, CurrentSlowMultiplier, CurrentSlowDuration);
        }
    }
}
using TowerDefense.Interfaces;
using TowerDefense.Towers.Projectiles;
using UnityEngine;
using Zenject;

namespace TowerDefense.Factories
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly DiContainer _container;

        public ProjectileFactory(DiContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates any type of projectile as long as it inherits from BaseProjectile.
        /// </summary>
        public T Create<T>(T projectilePrefab, Vector3 position, Quaternion rotation) where T : BaseProjectile
        {
            return _container.InstantiatePrefabForComponent<T>(projectilePrefab, position, rotation, null);
        }
    }
}
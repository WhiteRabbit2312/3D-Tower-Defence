using TowerDefense.Towers.Projectiles;
using UnityEngine;

namespace TowerDefense.Interfaces
{
    /// <summary>
    /// Generic interface for a projectile factory.
    /// </summary>
    public interface IProjectileFactory
    {
        T Create<T>(T projectilePrefab, Vector3 position, Quaternion rotation) where T : BaseProjectile;
    }
}
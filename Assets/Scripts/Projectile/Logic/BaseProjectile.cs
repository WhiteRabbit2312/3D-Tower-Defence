using TowerDefense.Interfaces;
using UnityEngine;

namespace TowerDefense.Towers.Projectiles
{
    /// <summary>
    /// Abstract base class for all projectiles.
    /// Handles movement towards a target and self-destruction if the target is lost.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseProjectile : MonoBehaviour
    {
        protected ITargetable Target;
        protected float Speed;

        public virtual void Initialize(ITargetable target, float speed)
        {
            Target = target;
            Speed = speed;
        }

        protected virtual void Update()
        {
            if (Target == null || !Target.IsAlive)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 targetPosition = Target.Position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
            transform.LookAt(targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                OnHitTarget();
            }
        }

        /// <summary>
        /// This method is called when the projectile reaches its target.
        /// Subclasses must implement this to define the on-hit effect (damage, slow, etc.).
        /// </summary>
        protected abstract void OnHitTarget();
    }
}
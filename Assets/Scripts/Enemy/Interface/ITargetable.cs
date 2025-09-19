using UnityEngine;

namespace TowerDefense.Interfaces
{
    /// <summary>
    /// Interface for any object that can be targeted by a tower.
    /// </summary>
    public interface ITargetable
    {
        Vector3 Position { get; }
        bool IsAlive { get; }
        Transform GetTransform();
    }
}

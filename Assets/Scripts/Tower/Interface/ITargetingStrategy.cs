using UnityEngine;
using TowerDefense.Interfaces;

namespace TowerDefense.Towers.Targeting
{
    /// <summary>
    /// Strategy pattern interface for defining how a tower finds a target.
    /// </summary>
    public interface ITargetingStrategy
    {
        ITargetable FindTarget(Vector3 towerPosition, float range);
    }
}

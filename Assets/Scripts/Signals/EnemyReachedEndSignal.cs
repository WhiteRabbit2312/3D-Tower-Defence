using TowerDefense.Enemies;

namespace TowerDefense.Signals
{
    /// <summary>
    /// A signal that is fired when an enemy successfully reaches the end of the path.
    /// </summary>
    public class EnemyReachedEndSignal
    {
        public readonly BaseEnemy Enemy;
        public EnemyReachedEndSignal(BaseEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}
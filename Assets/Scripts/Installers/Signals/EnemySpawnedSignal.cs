using TowerDefense.Enemies;

namespace TowerDefense.Signals
{
    /// <summary>
    /// A signal that is fired when a new enemy is spawned.
    /// It carries a reference to the enemy instance.
    /// </summary>
    public class EnemySpawnedSignal
    {
        public readonly BaseEnemy Enemy;
        
        public EnemySpawnedSignal(BaseEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}